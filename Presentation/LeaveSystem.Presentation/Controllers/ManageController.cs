using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using LeaveSystem.Presentation.Models;
using LeaveSystem.Presentation.Models.ManageViewModels;
using LeaveSystem.Presentation.Services;
using LeaveSystem.Business;
using LeaveSystem.Data.Model;
using LeaveSystem.Presentation.Helpers;

namespace LeaveSystem.Presentation.Controllers
{
    
    [Authorize]
    public class ManageController : Controller
    {
        private readonly IEmployeeManager _employeeManager;
        private readonly SignInManager<Employee> _signInManager;
        private readonly ILogger _logger;
        private readonly UrlEncoder _urlEncoder;

        private const string AuthenicatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";

        public ManageController(
          IEmployeeManager employeeManager,
          SignInManager<Employee> signInManager,
          ILogger<ManageController> logger,
          UrlEncoder urlEncoder)
        {
            _employeeManager = employeeManager;
            _signInManager = signInManager;
            _logger = logger;
            _urlEncoder = urlEncoder;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _employeeManager.GetEmployeeByUserNameAsync(User.Identity.Name);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_employeeManager.GetEmployeeByUserNameAsync(User.Identity.Name)}'.");
            }

            var model = new IndexViewModel
            {
                Username = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                IsEmailConfirmed = user.EmailConfirmed,
                StatusMessage = StatusMessage
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(IndexViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _employeeManager.GetEmployeeByUserNameAsync(User.Identity.Name);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{await _employeeManager.GetEmployeeByUserNameAsync(User.Identity.Name)}'.");
            }

            StatusMessage = "Your profile has been updated";
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            var user = await _employeeManager.GetEmployeeByUserNameAsync(User.Identity.Name);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{await _employeeManager.GetEmployeeByUserNameAsync(User.Identity.Name)}'.");
            }

            var hasPassword = await _employeeManager.HasPasswordAsync(user);
            if (!hasPassword)
            {
                return RedirectToAction(nameof(SetPassword));
            }

            var model = new ChangePasswordViewModel { StatusMessage = StatusMessage };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _employeeManager.GetEmployeeByUserNameAsync(User.Identity.Name);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{await _employeeManager.GetEmployeeByUserNameAsync(User.Identity.Name)}'.");
            }

            var changePasswordResult = await _employeeManager.UpdatePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (!changePasswordResult.Item1)
            {
                AddErrors(changePasswordResult.Item2);
                return View(model);
            }

            await _signInManager.SignInAsync(user, isPersistent: false);
            _logger.LogInformation("User changed their password successfully.");
            StatusMessage = "Your password has been changed.";

            return RedirectToAction(nameof(ChangePassword));
        }

        [HttpGet]
        public async Task<IActionResult> SetPassword()
        {
            var user = await _employeeManager.GetEmployeeByUserNameAsync(User.Identity.Name);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{await _employeeManager.GetEmployeeByUserNameAsync(User.Identity.Name)}'.");
            }

            var hasPassword = await _employeeManager.HasPasswordAsync(user);

            if (hasPassword)
            {
                return RedirectToAction(nameof(ChangePassword));
            }

            var model = new SetPasswordViewModel { StatusMessage = StatusMessage };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _employeeManager.GetEmployeeByUserNameAsync(User.Identity.Name);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{await _employeeManager.GetEmployeeByUserNameAsync(User.Identity.Name)}'.");
            }

            var addPasswordResult = await _employeeManager.ResetPasswordAsync(user, model.NewPassword);
            if (!addPasswordResult.Item1)
            {
                AddErrors(addPasswordResult.Item2);
                return View(model);
            }

            await _signInManager.SignInAsync(user, isPersistent: false);
            StatusMessage = "Your password has been set.";

            return RedirectToAction(nameof(SetPassword));
        }


  

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        private void AddErrors(IEnumerable<string> result)
        {
            foreach (var error in result)
            {
                ModelState.AddModelError(string.Empty, error);
            }
        }

        private string FormatKey(string unformattedKey)
        {
            var result = new StringBuilder();
            int currentPosition = 0;
            while (currentPosition + 4 < unformattedKey.Length)
            {
                result.Append(unformattedKey.Substring(currentPosition, 4)).Append(" ");
                currentPosition += 4;
            }
            if (currentPosition < unformattedKey.Length)
            {
                result.Append(unformattedKey.Substring(currentPosition));
            }

            return result.ToString().ToLowerInvariant();
        }

        private string GenerateQrCodeUri(string email, string unformattedKey)
        {
            return string.Format(
                AuthenicatorUriFormat,
                _urlEncoder.Encode("LeaveSystem.Presentation"),
                _urlEncoder.Encode(email),
                unformattedKey);
        }

        #endregion
    }
}
