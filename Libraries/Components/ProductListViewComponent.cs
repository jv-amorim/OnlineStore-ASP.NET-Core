using Microsoft.AspNetCore.Mvc;
using OnlineStore.Models.ViewModels;
using OnlineStore.Repositories.Interfaces;

namespace OnlineStore.Libraries.Components
{
    public class ProductListViewComponent : ViewComponent
    {
        private IProductRepository productRepository;

        public ProductListViewComponent(IProductRepository productRepository) => this.productRepository = productRepository;

        public IViewComponentResult Invoke(int? numberOfItemsPerPage)
        {
            var query = HttpContext.Request.Query;
            int? page = 1;
            string searchParameter = "";
            string sortingOption = null;

            if (query.ContainsKey("page"))
                page = int.Parse(HttpContext.Request.Query["page"]);
            if (query.ContainsKey("searchParameter"))
                searchParameter = HttpContext.Request.Query["searchParameter"];
            if (query.ContainsKey("sortingOption"))
                sortingOption = HttpContext.Request.Query["sortingOption"];

            var productList = new ProductListViewModel()
            {
                Products = productRepository.GetAllProducts(page, numberOfItemsPerPage ?? 10, searchParameter, sortingOption)
            };

            return View(productList);
        }
    }
}