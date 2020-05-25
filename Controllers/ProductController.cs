using Microsoft.AspNetCore.Mvc;
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
    }
}