using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BSUIR.BL.Interfaces.Models.Discounts;
using BSUIR.BL.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BSUIR.Web.Areas.Identity.Pages.Account.Manage
{
    public class DiscountsModel : PageModel
    {
        private readonly IDiscountsService _discountsService;

        public DiscountsModel(IDiscountsService discountsService)
        {
            _discountsService = discountsService;
        }
        
        public async Task<IActionResult> OnPostDelete(int Id)
        {
            await _discountsService.DeleteDiscountAsync(Id);
            ViewData["Discounts"] = await _discountsService.GetDiscountsAsync<Discounts>();
            return Page();
        }
        public async Task<IActionResult> OnPostCreate(decimal From, decimal To, int Amount)
        {
            await _discountsService.CreateDiscountAsync<Discounts>(new Discounts()
            {
                Amount = Amount,
                From = From,
                To = To
            });
            ViewData["Discounts"] = await _discountsService.GetDiscountsAsync<Discounts>();
            return Page();
        }
        public async Task<IActionResult> OnGet()
        {
            ViewData["Discounts"] = await _discountsService.GetDiscountsAsync<Discounts>();
            return Page();
        }
    }
}
