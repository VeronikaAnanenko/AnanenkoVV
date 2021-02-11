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

namespace BSUIR.Web.Areas.Identity.Pages.Account.Manage
{
    public class AddressesModel : PageModel
    {
        private readonly IAddressService _addressService;
        private readonly UserManager<User> _userManager;

        public AddressesModel(IAddressService addressService, UserManager<User> userManager)
        {
            _addressService = addressService;
            _userManager = userManager;
        }

        [TempData]
        public string StatusMessage { get; set; }
        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {

            public string Country { get; set; }
            public string Area { get; set; }
            public string City { get; set; }
            public string Street { get; set; }
            public string Building { get; set; }
            public string Flat { get; set; }
            public string Postal { get; set; }
            public int Id { get; set; }
        }
        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            var addresses = await _addressService.GetAddressesAsync<Address>();
            ViewData["Addresses"] = addresses.Where(x => x.CustomerId == user.Id).ToList();
            return Page();
        }
        public async Task<IActionResult> OnPostDelete(int id)
        {
            await _addressService.DeleteAddressAsync(id);
            var user = await _userManager.GetUserAsync(User);
            var addresses = await _addressService.GetAddressesAsync<Address>();
            ViewData["Addresses"] = addresses.Where(x => x.CustomerId == user.Id).ToList();
            return Page();
        }

        public async Task<IActionResult> OnPostUpdate()
        {
            var user = await _userManager.GetUserAsync(User);
            await _addressService.UpdateAddressAsync<Address>(new Address()
            {
                City = Input.City,
                Country = Input.Country,
                CustomerId = user.Id,
                Flat = Convert.ToInt32(Input.Flat),
                House = Convert.ToInt32(Input.Building),
                PostIndex = Convert.ToInt32(Input.Postal),
                Street = Input.Street,
                Region = Input.Area,
                Id = Input.Id
            });
            var addresses = await _addressService.GetAddressesAsync<Address>();
            ViewData["Addresses"] = addresses.Where(x => x.CustomerId == user.Id).ToList();
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            await _addressService.CreateAddressAsync<Address>(new Address()
            {
                City = Input.City,
                Country = Input.Country,
                CustomerId = user.Id,
                Flat = Convert.ToInt32(Input.Flat),
                House = Convert.ToInt32(Input.Building),
                PostIndex = Convert.ToInt32(Input.Postal),
                Street = Input.Street,
                Region = Input.Area
            });
            var addresses = await _addressService.GetAddressesAsync<Address>();
            ViewData["Addresses"] = addresses.Where(x => x.CustomerId == user.Id).ToList();
            return Page();
        }
    }
}
