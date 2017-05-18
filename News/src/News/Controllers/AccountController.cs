using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using News.Models;
using News.Models.AccountViewModels;
using Microsoft.AspNetCore.Http;

namespace News.Controllers
{
    /// <summary>
    /// A controller for login and logoff functions
    /// </summary>
    [Authorize]
    public class AccountController : Controller
    {

        //
        // GET: /Account/Login
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }


        //
        // POST: /Account/Login
        // Fake login for now. Just for test purpose
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            //since we dont have authentication for now. put usertype in session to share among pages
            if (ModelState.IsValid)
            {
                //for test purposes we assume that login succeeded
                var result = true;
                if (result)
                {
                    HttpContext.Session.SetString("SessionKeyLoginType", model.LoginType);
                    HttpContext.Session.SetString("SessionKeyEmail", model.Email);
                    return RedirectToAction(nameof(HomeController.Index), "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult Logoff(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            HttpContext.Session.Clear();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }


        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        #endregion
    }
}
