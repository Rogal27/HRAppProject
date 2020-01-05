using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNet.Identity;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using HRWebApplication.Helpers;

namespace HRWebApplication.Controllers
{
    public class ApiController : Controller
    {
        public AzureAdB2COptions AzureAdB2COptions { get; set; }

        public ApiController(IOptions<AzureAdB2COptions> b2cOptions)
        {
            AzureAdB2COptions = b2cOptions.Value;
        }

        public IActionResult LogIn()
        {
            if (User.Identity.IsAuthenticated == false)
                return RedirectToAction(nameof(SignIn));

            //TODO:
            if (User.IsInRole(UserRolesTypes.Admin))
                return RedirectToAction("Index", "Home");
            else if (User.IsInRole(UserRolesTypes.HR))
                return RedirectToAction("Index", "HR");
            else if (User.IsInRole(UserRolesTypes.Normal))
                return RedirectToAction("Index", "Home");
            else return SignOut();
        }

        [HttpGet]
        public IActionResult SignIn()
        {
            var redirectUrl = Url.Action(nameof(LogIn), "Api");
            return Challenge(
                new AuthenticationProperties { RedirectUri = redirectUrl },
                OpenIdConnectDefaults.AuthenticationScheme);
        }


        [HttpGet]
        public IActionResult SignOut()
        {
            //var callbackUrl = Url.Action(nameof(ApiController.LogIn), "Api", values: null, protocol: Request.Scheme);
            var callbackUrl = Url.Action("LogOff", "Home", values: null, protocol: Request.Scheme);
            return SignOut(new AuthenticationProperties { RedirectUri = callbackUrl },
                CookieAuthenticationDefaults.AuthenticationScheme, OpenIdConnectDefaults.AuthenticationScheme);
        }
    }
}