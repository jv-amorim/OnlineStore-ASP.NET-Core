using System.Collections.Generic;
using System.Linq;
using OnlineStore.Database;
using OnlineStore.Models;
using OnlineStore.Repositories.Interfaces;
using OnlineStore.Libraries.Enums;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace OnlineStore.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private OnlineStoreContext database;
        private ICategoryRepository categoryRepository;

        public ProductRepository(OnlineStoreContext database, ICategoryRepository categoryRepository)
        {
            this.database = database;
            this.categoryRepository = categoryRepository;
        }

        public void Register(Product product)
        {
            database.Add(product);
            database.SaveChanges();
        }

        public void Update(Product product)
        {
            database.Update(product);
            database.SaveChanges();
        }

        public void Delete(int id)
        {
            Product item = GetProduct(id);
            database.Remove(item);
            database.SaveChanges();
        }

        public Product GetProduct(int id)
        {
            return 
                database.Products
                .Include(p => p.Images)
                .Where(p => p.Id == id)
                .FirstOrDefault();
        }
            
        public IPagedList<Product> GetAllProducts(int? page, int pageSize, string searchParameter) => 
            GetAllProducts(page, pageSize, searchParameter, null, null);

        public IPagedList<Product> GetAllProducts(int? page, int pageSize, string searchParameter, string sortingOption) => 
            GetAllProducts(page, pageSize, searchParameter, sortingOption, null);

        public IPagedList<Product> GetAllProducts(int? page, int pageSize, string searchParameter, string sortingOption, Category category)
        {
            var productsFromDB = database.Products.AsQueryable();
                        
            productsFromDB = FilterProductsBySearchParameter(productsFromDB, searchParameter);
            productsFromDB = FilterProductsBySortingOption(productsFromDB, sortingOption);
            productsFromDB = FilterProductsByCategory(productsFromDB, category);

            return productsFromDB.Include(p => p.Images).ToPagedList<Product>(page ?? 1, pageSize);
        }

        private IQueryable<Product> FilterProductsBySearchParameter(IQueryable<Product> products, string searchParameter)
        {
            if (!string.IsNullOrEmpty(searchParameter))
            {
                searchParameter = searchParameter.Trim();
                products = products.Where(p => p.Name.Contains(searchParameter));
            }
            return products;
        }

        private IQueryable<Product> FilterProductsBySortingOption(IQueryable<Product> products, string sortingOption)
        {
            if (!string.IsNullOrEmpty(sortingOption))
            {
                SortingOptions option;   
                  
                if (SortingOptions.TryParse<SortingOptions>(sortingOption, out option))
                {
                    products = option switch
                    {
                        SortingOptions.Alphabetical_Ascending => products.OrderBy(p => p.Name),
                        SortingOptions.Alphabetical_Descending => products.OrderByDescending(p => p.Name),
                        SortingOptions.Price_LowToHigh => products.OrderBy(p => p.UnitPrice),
                        SortingOptions.Price_HighToLow => products.OrderByDescending(p => p.UnitPrice),
                        _ => products
                    };
                }
            }
            return products;
        }

        private IQueryable<Product> FilterProductsByCategory(IQueryable<Product> products, Category category)
        {
            if (category != null)
            {
                List<Category> categoriesToSearchForProducts = new List<Category>();
                categoriesToSearchForProducts.Add(category);

                var childCategories = categoryRepository.GetAllChildCategories(category.Id);
                categoriesToSearchForProducts.AddRange(childCategories);

                products = products.Where(p => categoriesToSearchForProducts.Contains(p.Category));
            }
            return products;
        }
    }
}