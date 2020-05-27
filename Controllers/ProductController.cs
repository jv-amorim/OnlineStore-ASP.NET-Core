using Microsoft.AspNetCore.Mvc;
using OnlineStore.Models;
using OnlineStore.Repositories.Interfaces;

namespace OnlineStore.Controllers
{
    public class ProductController : Controller
    {
        private IProductRepository productRepository;
        private ICategoryRepository categoryRepository;

        public ProductController(IProductRepository productRepository,  ICategoryRepository categoryRepository)
        {
            this.productRepository = productRepository;
            this.categoryRepository = categoryRepository;
        }

        [HttpGet]
        [Route("/Product/Category/{slug}")]
        public IActionResult ProductListingByCategory() => View();

        [HttpGet]
        [Route("/Product/Details")]
        public IActionResult ProductDetails(int id)
        {
            Product product = productRepository.GetProduct(id);
            if (product != null)
                product.Category = categoryRepository.GetCategory(product.CategoryId);
            return View(product);
        }
    }
}