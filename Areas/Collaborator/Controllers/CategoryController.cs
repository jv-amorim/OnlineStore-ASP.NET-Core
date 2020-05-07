using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Models;
using OnlineStore.Repositories.Interfaces;
using OnlineStore.Libraries.Filters;
using OnlineStore.Libraries.RazorUtils;
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
        public IActionResult Register()
        {
            IEnumerable<Category> categories = categoryRepository.GetAllCategories();
            ViewBag.Categories = ListOfCategoryItems.CreateNewListOfCategoryItems(categories);
            return View();
        }

        [HttpPost]
        public IActionResult Register([FromForm]Category category)
        {
            if (ModelState.IsValid)
            {
                categoryRepository.Register(category);
                TempData["MSG_OK"] = "Successful registration!";
                return RedirectToAction(nameof(Register));
            }
            
            IEnumerable<Category> categories = categoryRepository.GetAllCategories();
            ViewBag.Categories = ListOfCategoryItems.CreateNewListOfCategoryItems(categories);
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