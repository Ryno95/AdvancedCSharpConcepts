﻿using IdentityExample.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IdentityExample.Controllers.HomeController
{
	public class HomeController : Controller
	{

		private readonly UserManager<IdentityUser> _userManager;
		private readonly SignInManager<IdentityUser> _signInManager;

		public HomeController(
			UserManager<IdentityUser> userManager,
			SignInManager<IdentityUser> signInManager
			)
		{
			_userManager = userManager;
			_signInManager = signInManager;
		}
		public IActionResult Index()
		{
			return View();
		}

		[Authorize]
		public IActionResult Secure()
		{
			return View();
		}


		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Login(string username, string password)
		{
			var user = await _userManager.FindByNameAsync(username);

			if (user != null)
			{
				//sign in
				var signInResult = await _signInManager.PasswordSignInAsync(
					user,
					password,
					isPersistent: false,
					lockoutOnFailure: false
					);
				if (signInResult.Succeeded)
				{
					return RedirectToAction("Index");
				}
				else
				{
					throw new Exception("Sign in failed");
				}
			}

			return RedirectToAction("Index");
		}

		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Register(string username, string password)
		{
			var user = new IdentityUser
			{
				UserName = username,
				Email = "",
			};

			var result = await _userManager.CreateAsync(user, password);

			if (result.Succeeded)
			{
				var signInResult = await _signInManager.PasswordSignInAsync(user, password, isPersistent: false, lockoutOnFailure: false);
				if (signInResult.Succeeded)
				{
					return RedirectToAction("Index");
				}
				else
				{
					throw new Exception("Sign in failed");
				}
			}
			return RedirectToAction("Index");
		}


		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction("Index");
		}
	}
}
