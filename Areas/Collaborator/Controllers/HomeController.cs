using System;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Repositories.Interfaces;
using OnlineStore.Libraries.Session;

namespace OnlineStore.Areas.Collaborator.Controllers
{
    [Area("Collaborator")]
    public class HomeController : Controller
    {
        private ICollaboratorRepository collaboratorRepository;
        private CollaboratorSession collaboratorSession;

        public HomeController(ICollaboratorRepository collaboratorRepository, CollaboratorSession collaboratorSession)
        {
            this.collaboratorRepository = collaboratorRepository;
            this.collaboratorSession = collaboratorSession;
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login([FromForm] Models.Collaborator collaborator)
        {
            Models.Collaborator collaboratorFromDB = 
                collaboratorRepository.Login(collaborator.Email, collaborator.Password);

            if (collaboratorFromDB == null)
            {
                ViewData["MSG_ERROR"] = "No account found. Your email or password may be incorrect.";
                return View();
            }
            
            collaboratorSession.Login(collaboratorFromDB);
            return RedirectToAction(nameof(CollaboratorPanel));
        }

        public IActionResult RecoverPassword() => View();

        public IActionResult RegisterNewPassword() => View();

        [HttpGet]
        public IActionResult CollaboratorPanel() => new ContentResult() { Content = "Collaborator Panel." };
    }
}