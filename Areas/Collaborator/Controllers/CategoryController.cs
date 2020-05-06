using System.Linq;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Models;
using OnlineStore.Repositories.Interfaces;
using OnlineStore.Libraries.Filters;
using X.PagedList;

namespace OnlineStore.Areas.Collaborator.Controllers
{
    [Area("Collaborator")]
    [CollaboratorAuthorization]
    public class CategoryController : Controller
    {
        private ICategoryRepository categoryRepository;
        private const int NumberOfItemsPerPage = 10;
        
        public CategoryController(ICategoryRepository categoryRepository) => this.categoryRepository = categoryRepository;

        public IActionResult Index(int? page)
        {
            IPagedList<Category> categories = 
                categoryRepository.GetAllCategories(page, NumberOfItemsPerPage);
            return View(categories);
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public IActionResult Register([FromForm]Category category)
        {
            return View();
        }

        [HttpGet]
        public IActionResult Update(int id) => View();

        [HttpPost]
        public IActionResult Update([FromForm]Category category)
        {
            return View();
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            return View();
        }
    }
}