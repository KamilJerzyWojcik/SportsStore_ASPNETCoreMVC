using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using SportStore.Models.ViewModels;


namespace SportStore.Controllers
{
	[Authorize]
	public class AccountController : Controller
	{
		private UserManager<IdentityUser> _userManager;
		private SignInManager<IdentityUser> _signInManager;

		public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
		}

		[AllowAnonymous]
		public ViewResult Login(string returnUrl)
		{
			Login login = new Login { ReturnUrl = returnUrl };
			return View(login);
		}

		[HttpPost]
		[AllowAnonymous]
		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> Login(Login login)
		{
			if (ModelState.IsValid)
			{
				IdentityUser user = await _userManager.FindByNameAsync(login.Name);
				if (user != null)
				{
					await _signInManager.SignOutAsync();
					if ((await _signInManager.PasswordSignInAsync(user, login.Password, false, false)).Succeeded)
					{
						return Redirect(login?.ReturnUrl ?? "/Admin/Index");
					}
				}
			}

			ModelState.AddModelError("", "Nieprawidłowa nazwa użytkownika lub hasła");
			return View(login);
		}

		public async Task<RedirectResult> Logout(string returnUrl = "/")
		{
			await _signInManager.SignOutAsync();
			return Redirect(returnUrl);
		}
	}
}
