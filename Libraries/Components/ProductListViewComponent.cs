using Microsoft.AspNetCore.Mvc;
using OnlineStore.Models;
using OnlineStore.Models.ViewModels;
using OnlineStore.Repositories.Interfaces;

namespace OnlineStore.Libraries.Components
{
    public class ProductListViewComponent : ViewComponent
    {
        private IProductRepository productRepository;
        private ICategoryRepository categoryRepository;

        public ProductListViewComponent(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            this.productRepository = productRepository;
            this.categoryRepository = categoryRepository;
        }

        public IViewComponentResult Invoke(int? numberOfItemsPerPage)
        {
            int? page = 1;
            string searchParameter = "";
            string sortingOption = null;
            Category category = null;

            var query = HttpContext.Request.Query;

            if (query.ContainsKey("page"))
                page = int.Parse(HttpContext.Request.Query["page"]);
            if (query.ContainsKey("searchParameter"))
                searchParameter = HttpContext.Request.Query["searchParameter"];
            if (query.ContainsKey("sortingOption"))
                sortingOption = HttpContext.Request.Query["sortingOption"];
                
            if (ViewContext.RouteData.Values.ContainsKey("slug"))
            {
                string slug = ViewContext.RouteData.Values["slug"].ToString();
                category = categoryRepository.GetCategory(slug);
                if (category != null)
                    ViewData["CategoryName"] = category.Name;
            }

            var productList = new ProductListViewModel()
            {
                Products = productRepository.GetAllProducts(page, numberOfItemsPerPage ?? 12, searchParameter, sortingOption, category)
            };

            return View(productList);
        }
    }
}