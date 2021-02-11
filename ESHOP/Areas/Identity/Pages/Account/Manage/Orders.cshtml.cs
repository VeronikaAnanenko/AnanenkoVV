using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BSUIR.BL.Interfaces.Services;
using BSUIR.DAL.Interfaces.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Address = BSUIR.BL.Interfaces.Models.Addresses.Address;
using Customer = BSUIR.BL.Interfaces.Models.Customers.Customer;
using DeliveryAddress = BSUIR.BL.Interfaces.Models.DeliveryAddresses.DeliveryAddress;
using Discounts = BSUIR.BL.Interfaces.Models.Discounts.Discounts;
using Item = BSUIR.BL.Interfaces.Models.Items.Item;
using Order = BSUIR.BL.Interfaces.Models.Orders.Order;
using OrderHasItem = BSUIR.BL.Interfaces.Models.Orders.OrderHasItem;
using Photo = BSUIR.BL.Interfaces.Models.Photos.Photo;

namespace BSUIR.Web.Areas.Identity.Pages.Account.Manage
{
    public class OrdersModel : PageModel
    {
        private readonly IOrderService _orderService;
        private readonly UserManager<User> _userManager;
        private readonly ICustomerService _customerService;
        private readonly IAddressService _addressService;
        private readonly IDeliveryAddressService _deliveryAddressService;
        private readonly IItemService _itemService;
        private readonly IOrderHasItemService _orderHasItemService;
        private readonly IPhotoService _photoService;
        private readonly IDiscountsService _discountsService;
        public OrdersModel(UserManager<User> userManager, IOrderService orderService, ICustomerService customerService, IDeliveryAddressService deliveryAddressService, IAddressService addressService, IItemService itemService, IOrderHasItemService orderHasItemService, IPhotoService photoService, IDiscountsService discountsService)
        {
            _userManager = userManager;
            _orderService = orderService;
            _customerService = customerService;
            _deliveryAddressService = deliveryAddressService;
            _addressService = addressService;
            _itemService = itemService;
            _orderHasItemService = orderHasItemService;
            _photoService = photoService;
            _discountsService = discountsService;
        }

        public async Task<IActionResult> OnGet()
        {
            OutputOrder = new List<OutputOrders>();
            var user = await _userManager.GetUserAsync(User);
            var orders = await _orderService.GetRelatedOrderAsync<Order>(user.Id);
            var customer = await _customerService.GetCustomerByIdAsync<Customer>(user.Id);
            foreach (var order in orders)
            {
                Address address = new Address();
                var items = new List<BL.Interfaces.Models.Items.Item>();
                var orderHasItems = await _orderHasItemService.GetOrderHasItemsAsync<OrderHasItem>();
                var sorted = orderHasItems.ToList().Where(x => x.OrderId == order.Id);
                foreach (var orderHasItem in sorted)
                {
                    var tempItem =
                        await _itemService.GetItemByIdAsync<BL.Interfaces.Models.Items.Item>(orderHasItem.ItemId);
                    var photos = await _photoService.GetRelatedPhotosAsync<Photo>(tempItem.Id);
                    tempItem.Link = photos.ElementAt(0).Link;
                    items.Add(tempItem);
                }
                DeliveryAddress deliveryAddress = new DeliveryAddress();
                if (order.AddressId != null)
                    address = await _addressService.GetAddressByIdAsync<Address>(order.AddressId.Value);
                else
                {
                    var addresses = await _deliveryAddressService.GetDeliveryAddressesAsync<DeliveryAddress>();
                    deliveryAddress = addresses.ToList().FirstOrDefault(x => x.Id == order.DeliveryAddressId);
                }
                OutputOrder.Add(new OutputOrders()
                {
                    Address = address,
                    DeliveryAddress = deliveryAddress,
                    Items = items,
                    Order = order
                });
            }

            ViewData["Orders"] = OutputOrder;
            ViewData["Customer"] = customer;
            ViewData["Email"] = user.Email;

            return Page();
        }

        public async Task<IActionResult> OnPostUpdate(int id)
        {
            var found = await _orderService.GetOrderByIdAsync<Order>(id);
            found.Status = "Выполнен";
            await _orderService.UpdateOrderAsync<Order>(found);
            decimal total = 0;
            OutputOrder = new List<OutputOrders>();
            var user = await _userManager.GetUserAsync(User);
            var orders = await _orderService.GetRelatedOrderAsync<Order>(user.Id);
            var customer = await _customerService.GetCustomerByIdAsync<Customer>(user.Id);
            foreach (var order in orders)
            {
                total += order.Amount;
                Address address = new Address();
                var items = new List<BL.Interfaces.Models.Items.Item>();
                var orderHasItems = await _orderHasItemService.GetOrderHasItemsAsync<OrderHasItem>();
                var sorted = orderHasItems.ToList().Where(x => x.OrderId == order.Id);
                foreach (var orderHasItem in sorted)
                {
                    var tempItem =
                        await _itemService.GetItemByIdAsync<BL.Interfaces.Models.Items.Item>(orderHasItem.ItemId);
                    var photos = await _photoService.GetRelatedPhotosAsync<Photo>(tempItem.Id);
                    tempItem.Link = photos.ElementAt(0).Link;
                    items.Add(tempItem);
                }
                DeliveryAddress deliveryAddress = new DeliveryAddress();
                if (order.AddressId != null)
                    address = await _addressService.GetAddressByIdAsync<Address>(order.AddressId.Value);
                else
                {
                    var addresses = await _deliveryAddressService.GetDeliveryAddressesAsync<DeliveryAddress>();
                    deliveryAddress = addresses.ToList().FirstOrDefault(x => x.Id == order.DeliveryAddressId);
                }
                OutputOrder.Add(new OutputOrders()
                {
                    Address = address,
                    DeliveryAddress = deliveryAddress,
                    Items = items,
                    Order = order
                });
            }
            var discounts = await _discountsService.GetDiscountsAsync<Discounts>();
            var discount = discounts.ToList().FirstOrDefault(x => x.From < total && x.To > total);
            if (discount != null)
            {
                customer.Discount = discount.Amount;
                await _customerService.UpdateCustomerAsync<Customer>(customer);
            }
            ViewData["Orders"] = OutputOrder;
            ViewData["Customer"] = customer;
            ViewData["Email"] = user.Email;

            return Page();
        }
        public List<OutputOrders> OutputOrder { get; set; }

        public async Task<IActionResult> OnPostDelete(int id)
        {
            await _orderService.DeleteOrderAsync(id);
            OutputOrder = new List<OutputOrders>();
            var user = await _userManager.GetUserAsync(User);
            var orders = await _orderService.GetRelatedOrderAsync<Order>(user.Id);
            var customer = await _customerService.GetCustomerByIdAsync<Customer>(user.Id);
            foreach (var order in orders)
            {
                Address address = new Address();
                var items = new List<BL.Interfaces.Models.Items.Item>();
                var orderHasItems = await _orderHasItemService.GetOrderHasItemsAsync<OrderHasItem>();
                var sorted = orderHasItems.ToList().Where(x => x.OrderId == order.Id);
                foreach (var orderHasItem in sorted)
                {
                    var tempItem =
                        await _itemService.GetItemByIdAsync<BL.Interfaces.Models.Items.Item>(orderHasItem.ItemId);
                    var photos = await _photoService.GetRelatedPhotosAsync<Photo>(tempItem.Id);
                    tempItem.Link = photos.ElementAt(0).Link;
                    items.Add(tempItem);
                }
                DeliveryAddress deliveryAddress = new DeliveryAddress();
                if (order.AddressId != null)
                    address = await _addressService.GetAddressByIdAsync<Address>(order.AddressId.Value);
                else
                {
                    var addresses = await _deliveryAddressService.GetDeliveryAddressesAsync<DeliveryAddress>();
                    deliveryAddress = addresses.ToList().FirstOrDefault(x => x.Id == order.DeliveryAddressId);
                }
                OutputOrder.Add(new OutputOrders()
                {
                    Address = address,
                    DeliveryAddress = deliveryAddress,
                    Items = items,
                    Order = order
                });
            }
            ViewData["Orders"] = OutputOrder;
            ViewData["Customer"] = customer;
            ViewData["Email"] = user.Email;

            return Page();
        }
    }
    public class OutputOrders
    {
        public Order Order { get; set; }
        public List<Item> Items { get; set; }
        public DeliveryAddress DeliveryAddress { get; set; }
        public Address Address { get; set; }
    }
}

