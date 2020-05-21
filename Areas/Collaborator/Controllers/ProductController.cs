using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Models;
using OnlineStore.Repositories.Interfaces;
using OnlineStore.Libraries.Language;
using OnlineStore.Libraries.Filters;
using OnlineStore.Libraries.Helpers.RazorHelpers;
using OnlineStore.Libraries.Helpers.FileHelpers;
using X.PagedList;

namespace OnlineStore.Areas.Collaborator.Controllers
{
    [Area("Collaborator")]
    [CollaboratorAuthorization]
    public class ProductController : Controller
    {
        private IProductRepository productRepository;
        private ICategoryRepository categoryRepository;
        private IImageRepository imageRepository;
        private const int NumberOfItemsPerPage = 10;

        public ProductController(IProductRepository productRepository, ICategoryRepository categoryRepository, IImageRepository imageRepository)
        {
            this.productRepository = productRepository;
            this.categoryRepository = categoryRepository;
            this.imageRepository = imageRepository;
        }

        public IActionResult Index(int? page, string searchParameter)
        {
            FileManager.ClearTempUploadFolder();
            
            IPagedList<Product> products =
                productRepository.GetAllProducts(page, NumberOfItemsPerPage, searchParameter);
            return View(products);
        }

        [HttpGet]
        public IActionResult Register()
        {
            IEnumerable<Category> categories = categoryRepository.GetAllCategories();
            ViewBag.Categories = SelectListItemHelpers.CreateNewListOfCategoryItems(categories);
            return View();
        }

        [HttpPost]
        public IActionResult Register([FromForm] Product product)
        {
            var tempImagesPaths = new List<string>(Request.Form["imageFilePath"]);

            if (ModelState.IsValid)
            {
                productRepository.Register(product);

                List<string> permanentPaths = FileManager.MoveImagesToThePermanentFolder(tempImagesPaths, product.Id);
                imageRepository.Register(Image.CreateImageList(permanentPaths, product.Id));

                TempData["MSG_OK"] = Message.MSG_OK_001;
                return RedirectToAction(nameof(Register));
            }

            ViewBag.ImagesPaths = tempImagesPaths;

            IEnumerable<Category> categories = categoryRepository.GetAllCategories();
            ViewBag.Categories = SelectListItemHelpers.CreateNewListOfCategoryItems(categories);
            
            return View();
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            Product productToUpdate  = productRepository.GetProduct(id);

            ViewBag.ImagesPaths = productToUpdate.Images.Select(i => i.Path).ToList();

            IEnumerable<Category> categories = categoryRepository.GetAllCategories();
            ViewBag.Categories = SelectListItemHelpers.CreateNewListOfCategoryItems(categories);

            return View(productToUpdate);
        }

        [HttpPost]
        public IActionResult Update(Product product)
        {
            var tempImagesPaths = new List<string>(Request.Form["imageFilePath"]);

            if (ModelState.IsValid)
            {
                productRepository.Update(product);
                
                List<string> permanentPaths = FileManager.MoveImagesToThePermanentFolder(tempImagesPaths, product.Id);
                imageRepository.Register(Image.CreateImageList(permanentPaths, product.Id));

                TempData["MSG_OK"] = Message.MSG_OK_002;
                return RedirectToAction(nameof(Index));
            }

            ViewBag.ImagesPaths = tempImagesPaths;

            IEnumerable<Category> categories = categoryRepository.GetAllCategories();
            ViewBag.Categories = SelectListItemHelpers.CreateNewListOfCategoryItems(categories);
            
            return View();
        }

        [HttpGet]
        [ValidateHttpReferer]
        public IActionResult Delete(int id)
        {
            imageRepository.DeleteAllProductImages(id);
            FileManager.DeleteProductImagesFolder(id);

            productRepository.Delete(id);

            TempData["MSG_OK"] = Message.MSG_OK_003;
            return RedirectToAction(nameof(Index));
        }
    }
}