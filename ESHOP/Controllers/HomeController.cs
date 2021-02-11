using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using BSUIR.BL.Interfaces.Models;
using BSUIR.BL.Interfaces.Services;
using BSUIR.DAL.Interfaces.Models;
using BSUIR.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using DeliveryAddress = BSUIR.BL.Interfaces.Models.DeliveryAddresses.DeliveryAddress;

namespace BSUIR.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDeliveryAddressService _deliveryAddressService;
        private readonly RoleManager<IdentityRole> roleManager;
        public HomeController(ILogger<HomeController> logger, IDeliveryAddressService deliveryAddressService, RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            _deliveryAddressService = deliveryAddressService;
            this.roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var coordinates = new List<Marker>();
            foreach (var address in await _deliveryAddressService.GetDeliveryAddressesAsync<DeliveryAddress>())
            {
                var coordinatesJson = JsonConvert.DeserializeObject<Coordinates>(address.Coordinates);
                coordinates.Add(new Marker()
                {
                    Address = address.Street+", "+address.House,
                    AddressId = address.Id,
                    Lat = coordinatesJson.Lat,
                    Lng = coordinatesJson.Lng
                });
            }

            ViewData["DeliveryAddresses"] = await _deliveryAddressService.GetDeliveryAddressesAsync<DeliveryAddress>();
            return View(coordinates);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
