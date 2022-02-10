using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMS.Application.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult LoginOrRegister(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (String.IsNullOrEmpty(returnUrl))
            {
                ViewData["ReturnUrl"] = "/Home/Index";
            }
            return View();
        }

    }
}
