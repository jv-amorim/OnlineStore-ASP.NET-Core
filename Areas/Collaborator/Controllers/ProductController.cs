using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Models;
using OnlineStore.Repositories.Interfaces;
using OnlineStore.Libraries.Language;
using OnlineStore.Libraries.Session;
using OnlineStore.Libraries.Filters;
using OnlineStore.Libraries.RazorUtils;
using X.PagedList;

namespace OnlineStore.Areas.Collaborator.Controllers
{
    [Area("Collaborator")]
    [CollaboratorAuthorization]
    public class ProductController : Controller
    {
        private IProductRepository productRepository;
        ICategoryRepository categoryRepository;
        private const int NumberOfItemsPerPage = 10;

        public ProductController(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            this.productRepository = productRepository;
            this.categoryRepository = categoryRepository;
        }

        public IActionResult Index(int? page, string searchParameter)
        {
            IPagedList<Product> products =
                productRepository.GetAllProducts(page, NumberOfItemsPerPage, searchParameter);
            return View(products);
        }

        [HttpGet]
        public IActionResult Register()
        {
            IEnumerable<Category> categories = categoryRepository.GetAllCategories();
            ViewBag.Categories = ListOfCategoryItems.CreateNewListOfCategoryItems(categories);
            return View();
        }

        [HttpPost]
        public IActionResult Register([FromForm] Product product)
        {
            if (ModelState.IsValid)
            {
                productRepository.Register(product);
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
            Product productToUpdate  = productRepository.GetProduct(id);

            IEnumerable<Category> categories = categoryRepository.GetAllCategories();
            ViewBag.Categories = ListOfCategoryItems.CreateNewListOfCategoryItems(categories);

            return View(productToUpdate);
        }

        [HttpPost]
        public IActionResult Update(Product product)
        {
            if (ModelState.IsValid)
            {
                productRepository.Update(product);
                TempData["MSG_OK"] = Message.MSG_OK_002;
                return RedirectToAction(nameof(Index));
            }

            IEnumerable<Category> categories = categoryRepository.GetAllCategories();
            ViewBag.Categories = ListOfCategoryItems.CreateNewListOfCategoryItems(categories);
            return View();
        }
    }
}