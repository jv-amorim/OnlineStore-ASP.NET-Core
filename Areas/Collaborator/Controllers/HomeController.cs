using System;
using Microsoft.AspNetCore.Mvc;

namespace OnlineStore.Areas.Collaborator.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult RecoverPassword()
        {
            return View();
        }

        public IActionResult RegisterNewPassword()
        {
            return View();
        }
    }
}