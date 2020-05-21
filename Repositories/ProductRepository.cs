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

        public ProductRepository(OnlineStoreContext database) => this.database = database;

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

        public Product GetProduct(int id)
        {
            return 
                database.Products
                .Include(p => p.Images)
                .Where(p => p.Id == id)
                .FirstOrDefault();
        }
            
        public IPagedList<Product> GetAllProducts(int? page, int pageSize, string searchParameter) => 
            GetAllProducts(page, pageSize, searchParameter, null);

        public IPagedList<Product> GetAllProducts(int? page, int pageSize, string searchParameter, string sortingOption)
        {
            var productsFromDB = database.Products.AsQueryable();
            
            if (!string.IsNullOrEmpty(searchParameter))
            {
                searchParameter = searchParameter.Trim();
                productsFromDB = 
                    productsFromDB
                    .Where(p => p.Name.Contains(searchParameter));
            }

            if (!string.IsNullOrEmpty(sortingOption))
            {
                SortingOptions option;   
                  
                if (SortingOptions.TryParse<SortingOptions>(sortingOption, out option))
                {
                    productsFromDB = option switch
                    {
                        SortingOptions.Alphabetical_Ascending => productsFromDB.OrderBy(p => p.Name),
                        SortingOptions.Alphabetical_Descending => productsFromDB.OrderByDescending(p => p.Name),
                        SortingOptions.Price_LowToHigh => productsFromDB.OrderBy(p => p.UnitPrice),
                        SortingOptions.Price_HighToLow => productsFromDB.OrderByDescending(p => p.UnitPrice),
                        _ => productsFromDB
                    };
                }
            }

            return productsFromDB.Include(p => p.Images).ToPagedList<Product>(page ?? 1, pageSize);
        }

        public void Delete(int id)
        {
            Product item = GetProduct(id);
            database.Remove(item);
            database.SaveChanges();
        }
    }
}