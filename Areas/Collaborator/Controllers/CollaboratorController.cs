using Microsoft.AspNetCore.Mvc;
using OnlineStore.Models;
using OnlineStore.Repositories.Interfaces;
using OnlineStore.Libraries.Language;
using OnlineStore.Libraries.Session;
using OnlineStore.Libraries.Filters;
using X.PagedList;

namespace OnlineStore.Areas.Collaborator.Controllers
{
    [Area("Collaborator")]
    [CollaboratorAuthorization]
    public class CollaboratorController : Controller
    {
        private ICollaboratorRepository collaboratorRepository;
        private  CollaboratorSession collaboratorSession;
        private const int NumberOfItemsPerPage = 10;
        
        public CollaboratorController(ICollaboratorRepository collaboratorRepository, CollaboratorSession collaboratorSession)
        {
            this.collaboratorRepository = collaboratorRepository;
            this.collaboratorSession = collaboratorSession;
        }

        public IActionResult Index(int? page)
        {
            Models.Collaborator loggedInCollaborator = collaboratorSession.GetLoggedInCollaborator();
            ViewData["IsTheLoggedInCollaboratorAnAdmin"] = loggedInCollaborator.IsAdministrator;
            ViewData["LoggedInCollaboratorID"] = loggedInCollaborator.Id;

            IPagedList<Models.Collaborator> collaborators = 
                collaboratorRepository.GetAllCollaborators(page, NumberOfItemsPerPage);

            return View(collaborators);
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public IActionResult Register([FromForm] Models.Collaborator collaborator)
        {
            if (ModelState.IsValid)
            {
                collaborator.IsAdministrator = false;
                collaboratorRepository.Register(collaborator);
                TempData["MSG_OK"] = Message.MSG_OK_001;
                return RedirectToAction(nameof(Index));
            }
            
            return View();
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            Models.Collaborator loggedInCollaborator = collaboratorSession.GetLoggedInCollaborator();
            bool isTheLoggedInCollaboratorAnAdmin = loggedInCollaborator.IsAdministrator == true;
            bool isTheLoggedInCollaboratorTheSameToUpdate = loggedInCollaborator.Id == id;

            if (isTheLoggedInCollaboratorAnAdmin || isTheLoggedInCollaboratorTheSameToUpdate)
            {
                Models.Collaborator collaboratorToUpdate = collaboratorRepository.GetCollaborator(id);
                collaboratorToUpdate.Password = null;
                return View(collaboratorToUpdate);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Update([FromForm] Models.Collaborator collaborator)
        {
            if (ModelState.IsValid)
            {
                collaboratorRepository.Update(collaborator);
                TempData["MSG_OK"] = Message.MSG_OK_002;
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            Models.Collaborator loggedInCollaborator = collaboratorSession.GetLoggedInCollaborator();
            bool isTheLoggedInCollaboratorAnAdmin = loggedInCollaborator.IsAdministrator == true;
            bool isTheLoggedInCollaboratorTheSameToDelete = loggedInCollaborator.Id == id;
            
            if (isTheLoggedInCollaboratorAnAdmin && !isTheLoggedInCollaboratorTheSameToDelete)
            {
                collaboratorRepository.Delete(id);
                TempData["MSG_OK"] = Message.MSG_OK_003;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}