using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BSUIR.BL.Interfaces.Models;
using BSUIR.BL.Interfaces.Services;
using BSUIR.DAL.Interfaces.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Address = BSUIR.BL.Interfaces.Models.Addresses.Address;
using Customer = BSUIR.BL.Interfaces.Models.Customers.Customer;
using DeliveryAddress = BSUIR.BL.Interfaces.Models.DeliveryAddresses.DeliveryAddress;
using Order = BSUIR.BL.Interfaces.Models.Orders.Order;
using OrderHasItem = BSUIR.BL.Interfaces.Models.Orders.OrderHasItem;

namespace BSUIR.Web.Controllers
{
    public class BasketController : Controller
    {
        private Basket _basket;
        private readonly IDeliveryAddressService _deliveryAddressService;
        private readonly IAddressService _addressService;
        private readonly UserManager<User> _userManager;
        private readonly ICustomerService _customerService;
        private readonly IOrderService _orderService;
        private readonly IOrderHasItemService _orderHasItemService;
        public BasketController(Basket basket, IDeliveryAddressService deliveryAddressService, IAddressService addressService, UserManager<User> userManager, ICustomerService customerService, IOrderService orderService, IOrderHasItemService orderHasItemService)
        {
            _basket = basket;
            _deliveryAddressService = deliveryAddressService;
            _addressService = addressService;
            _userManager = userManager;
            _customerService = customerService;
            _orderService = orderService;
            _orderHasItemService = orderHasItemService;
        }
        [HttpPost]
        public async Task<IActionResult> UpdateAddress(string Id, string Country, string Area, string City, string Street, string Building, string Postal, string Flat)
        {
            var user = await _userManager.GetUserAsync(User);
            await _addressService.UpdateAddressAsync<Address>(new Address()
            {
                City = City,
                Country = Country,
                CustomerId = user.Id,
                Flat = Convert.ToInt32(Flat),
                House = Convert.ToInt32(Building),
                PostIndex = Convert.ToInt32(Postal),
                Street = Street,
                Region = Area,
                Id = Convert.ToInt32(Id)
            });
            var addresses = await _addressService.GetAddressesAsync<Address>();
            ViewData["Addresses"] = addresses.Where(x => x.CustomerId == user.Id).ToList();
            var coordinates = new List<Marker>();
            foreach (var address in await _deliveryAddressService.GetDeliveryAddressesAsync<DeliveryAddress>())
            {
                var coordinatesJson = JsonConvert.DeserializeObject<Coordinates>(address.Coordinates);
                coordinates.Add(new Marker()
                {
                    Address = address.Street + ", " + address.House,
                    AddressId = address.Id,
                    Lat = coordinatesJson.Lat,
                    Lng = coordinatesJson.Lng
                });
            }
            ViewData["DeliveryAddresses"] = await _deliveryAddressService.GetDeliveryAddressesAsync<DeliveryAddress>();
            return View("Address", coordinates);
        }
        [HttpPost]
        public async Task<IActionResult> AddAddress(string Country, string Area, string City, string Street, string Building, string Postal, string Flat)
        {
            var user = await _userManager.GetUserAsync(User);
            await _addressService.CreateAddressAsync<Address>(new Address()
            {
                City = City,
                Country = Country,
                CustomerId = user.Id,
                Flat = Convert.ToInt32(Flat),
                House = Convert.ToInt32(Building),
                PostIndex = Convert.ToInt32(Postal),
                Street = Street,
                Region = Area
            });
            var addresses = await _addressService.GetAddressesAsync<Address>();
            ViewData["Addresses"] = addresses.Where(x => x.CustomerId == user.Id).ToList();
            var coordinates = new List<Marker>();
            foreach (var address in await _deliveryAddressService.GetDeliveryAddressesAsync<DeliveryAddress>())
            {
                var coordinatesJson = JsonConvert.DeserializeObject<Coordinates>(address.Coordinates);
                coordinates.Add(new Marker()
                {
                    Address = address.Street + ", " + address.House,
                    AddressId = address.Id,
                    Lat = coordinatesJson.Lat,
                    Lng = coordinatesJson.Lng
                });
            }
            ViewData["DeliveryAddresses"] = await _deliveryAddressService.GetDeliveryAddressesAsync<DeliveryAddress>();
            return View("Address", coordinates);
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Address()
        {
            var user = await _userManager.GetUserAsync(User);
            var addresses = await _addressService.GetAddressesAsync<Address>();
            ViewData["Addresses"] = addresses.Where(x => x.CustomerId == user.Id).ToList();
            var coordinates = new List<Marker>();
            foreach (var address in await _deliveryAddressService.GetDeliveryAddressesAsync<DeliveryAddress>())
            {
                var coordinatesJson = JsonConvert.DeserializeObject<Coordinates>(address.Coordinates);
                coordinates.Add(new Marker()
                {
                    Address = address.Street + ", " + address.House,
                    AddressId = address.Id,
                    Lat = coordinatesJson.Lat,
                    Lng = coordinatesJson.Lng
                });
            }
            ViewData["DeliveryAddresses"] = await _deliveryAddressService.GetDeliveryAddressesAsync<DeliveryAddress>();
            return View(coordinates);
        }

        public async Task<IActionResult> ChooseAddress(int id, bool isdelivery)
        {
            _basket.IsDelivery = isdelivery;
            _basket.AddressId = id;
            var user = await _userManager.GetUserAsync(User);
            var addresses = await _addressService.GetAddressesAsync<Address>();
            ViewData["Addresses"] = addresses.Where(x => x.CustomerId == user.Id).ToList();

            var coordinates = new List<Marker>();
            foreach (var address in await _deliveryAddressService.GetDeliveryAddressesAsync<DeliveryAddress>())
            {
                var coordinatesJson = JsonConvert.DeserializeObject<Coordinates>(address.Coordinates);
                coordinates.Add(new Marker()
                {
                    Address = address.Street + ", " + address.House,
                    AddressId = address.Id,
                    Lat = coordinatesJson.Lat,
                    Lng = coordinatesJson.Lng
                });
            }
            ViewData["DeliveryAddresses"] = await _deliveryAddressService.GetDeliveryAddressesAsync<DeliveryAddress>();
            return View("Address", coordinates);
        }
        public IActionResult RemoveItem(int id)
        {
            var deletedItem = _basket.Items.FirstOrDefault(x => x.Id == id);
            _basket.Items.Remove(deletedItem);
            _basket.Price -= deletedItem.Price;
            return View("Index");
        }
        public async Task<IActionResult> Confirm()
        {
            var user = await _userManager.GetUserAsync(User);
            ViewData["Customer"] = await _customerService.GetCustomerByIdAsync<Customer>(user.Id);
            ViewData["Email"] = user.Email;
            if (_basket.IsDelivery)
            {
                ViewData["Address"] = await _addressService.GetAddressByIdAsync<Address>(_basket.AddressId);
            }
            else
            {
                var addresses = await _deliveryAddressService.GetDeliveryAddressesAsync<DeliveryAddress>(_basket.AddressId);
                ViewData["DeliveryAddress"] = addresses.FirstOrDefault(x => x.Id == _basket.AddressId);
            }
            return View();
        }

        public async Task<IActionResult> ConfirmOrder(string Comment, DateTime Date)
        {
            var user = await _userManager.GetUserAsync(User);
            var customer = await _customerService.GetCustomerByIdAsync<Customer>(user.Id);
            var createdOrder = await _orderService.CreateOrderAsync<Order>(new Order()
            {
                CustomerId = user.Id,
                Amount = (decimal)(_basket.Price - (customer.Discount == null ? 0 : customer.Discount * _basket.Price / 100)),
                Comment = Comment,
                Date = Date,
                DeliveryAddressId = !_basket.IsDelivery ? _basket.AddressId : (int?)null,
                AddressId = !_basket.IsDelivery ? (int?)null : _basket.AddressId,
                Status = "Обрабатывается"
            });
            foreach (var item in _basket.Items)
            {
                await _orderHasItemService.CreateOrderHasItemAsync<OrderHasItem>(new BL.Interfaces.Models.Orders.OrderHasItem()
                {
                    ItemId = item.Id,
                    OrderId = createdOrder.Id
                });
            }
            _basket = new Basket();
            //var related = await _orderService.GetRelatedOrderAsync<Order>(user.Id);
            //var totalSum = 0.0;
            //foreach (var order in related)
            //{
                
            //}
            return Redirect("/Home/Index");
        }
    }
}