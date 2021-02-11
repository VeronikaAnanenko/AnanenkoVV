using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BSUIR.BL.Interfaces.Services;
using BSUIR.DAL.Interfaces.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Customer = BSUIR.BL.Interfaces.Models.Customers.Customer;

namespace BSUIR.Web.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ICustomerService _customerService;

        public IndexModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager, ICustomerService customerService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _customerService = customerService;
        }


        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Phone]
            [Display(Name = "Телефон")]
            public string PhoneNumber { get; set; }
            [DataType(DataType.Password)]
            [Display(Name = "Current password")]
            public string OldPassword { get; set; }

            [StringLength(100, ErrorMessage = "{0} должен содержать минимум {2} и максимум {1} символов.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Новый пароль")]
            public string NewPassword { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm new password")]
            [Compare("NewPassword", ErrorMessage = "Новый пароль и подтверждение пароля не совпадают.")]
            public string ConfirmPassword { get; set; }
            public int Discount { get; set; }
            public string FirstName { get; set; }
            public string SecondName { get; set; }
            public string LastName { get; set; }
            public string Sex { get; set; }
            [DataType(DataType.Date)] 
            public DateTime Date { get; set; }

            public string Email { get; set; }
        }

        private async Task LoadAsync(User user)
        {
            var customer = await _customerService.GetCustomerByIdAsync<Customer>(user.Id);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var email = await _userManager.GetEmailAsync(user);

            Input = new InputModel
            {
                Discount = customer.Discount.Value,
                Sex = customer.Sex,
                LastName = customer.Lastname,
                Date = customer.Birthdate,
                FirstName = customer.Firstname,
                SecondName = customer.Secondname,
                PhoneNumber = phoneNumber,
                Email = email
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    var userId = await _userManager.GetUserIdAsync(user);
                    throw new InvalidOperationException($"Unexpected error occurred setting phone number for user with ID '{userId}'.");
                }
            }

            var customer = await _customerService.GetCustomerByIdAsync<Customer>(user.Id);
            await _customerService.UpdateCustomerAsync<Customer>(new Customer()
            {
                Id = user.Id,
                Birthdate = Input.Date,
                Firstname = Input.FirstName,
                Lastname = Input.LastName,
                MobileNumber = Input.PhoneNumber,
                Secondname = Input.SecondName,
                Sex = Input.Sex,
                Discount = customer.Discount
            });
            var changeEmail = await _userManager.SetEmailAsync(user, Input.Email);
            if (Input.OldPassword != null)
            {
                var changePasswordResult =
                    await _userManager.ChangePasswordAsync(user, Input.OldPassword, Input.NewPassword);
                if (!changePasswordResult.Succeeded)
                {
                    foreach (var error in changePasswordResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }

                    return Page();
                }
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
