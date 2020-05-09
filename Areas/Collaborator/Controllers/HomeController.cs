using Microsoft.AspNetCore.Mvc;
using OnlineStore.Repositories.Interfaces;
using OnlineStore.Libraries.Language;
using OnlineStore.Libraries.Session;
using OnlineStore.Libraries.Filters;

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
                ViewData["MSG_ERROR"] = Message.MSG_ERROR_006;
                return View();
            }
            
            collaboratorSession.Login(collaboratorFromDB);
            return RedirectToAction(nameof(CollaboratorDashboard));
        }

        [CollaboratorAuthorization]
        public IActionResult Logout()
        {
            collaboratorSession.Logout();
            return RedirectToAction(nameof(Login));
        }

        public IActionResult RecoverPassword() => View();

        public IActionResult RegisterNewPassword() => View();

        [CollaboratorAuthorization]
        public IActionResult CollaboratorDashboard() => View();
    }
}