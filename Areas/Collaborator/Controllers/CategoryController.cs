using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Models;
using OnlineStore.Repositories.Interfaces;
using OnlineStore.Libraries.Language;
using OnlineStore.Libraries.RazorUtils;
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
        
        public CategoryController(ICategoryRepository categoryRepository) => 
            this.categoryRepository = categoryRepository;

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
        public IActionResult Register([FromForm] Category category)
        {
            if (ModelState.IsValid)
            {
                categoryRepository.Register(category);
                TempData["MSG_OK"] = Message.MSG_OK_001;
                return RedirectToAction(nameof(Register));
            }
            
            IEnumerable<Category> categories = categoryRepository.GetAllCategories();
            ViewBag.Categories = ListOfCategoryItems.CreateNewListOfCategoryItems(categories);
            return View();
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            Category categoryToUpdate = categoryRepository.GetCategory(id);
            
            IEnumerable<Category> categories = categoryRepository.GetAllCategories();
            categories = categories.ToList().Where(c => c.Id != id);
            ViewBag.Categories = ListOfCategoryItems.CreateNewListOfCategoryItems(categories);

            return View(categoryToUpdate);
        }

        [HttpPost]
        public IActionResult Update([FromForm] Category category)
        {
            if (ModelState.IsValid)
            {
                categoryRepository.Update(category);
                TempData["MSG_OK"] = Message.MSG_OK_002;
                return RedirectToAction(nameof(Index));
            }
            
            IEnumerable<Category> categories = categoryRepository.GetAllCategories();
            categories = categories.ToList().Where(c => c.Id != category.Id);
            ViewBag.Categories = ListOfCategoryItems.CreateNewListOfCategoryItems(categories);
            
            return View();
        }

        [HttpGet]
        [ValidateHttpReferer]
        public IActionResult Delete(int id)
        {
            IEnumerable<Category> childCategory = 
                categoryRepository.GetAllCategories()
                .ToList().Where(c => c.ParentCategoryId == id);

            bool isParentCategory = childCategory.ToList().Count > 0;

            if (isParentCategory)
            {
                TempData["MSG_ERROR"] = 
                    "The category cannot be deleted, because it is a parent category. " +
                    "To delete this category, remove all references to it in other categories.";
            }
            else
            {
                categoryRepository.Delete(id);
                TempData["MSG_OK"] = Message.MSG_OK_003;
            }
            
            return RedirectToAction(nameof(Index));
        }
    }
}