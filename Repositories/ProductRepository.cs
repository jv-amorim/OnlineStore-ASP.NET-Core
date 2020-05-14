using System.Linq;
using OnlineStore.Database;
using OnlineStore.Models;
using OnlineStore.Repositories.Interfaces;
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
                .Include(i => i.Images)
                .Where(a => a.Id == id)
                .FirstOrDefault();
        }
            
        public IPagedList<Product> GetAllProducts(int? page, int pageSize, string searchParameter)
        {
            var productsFromDB = database.Products.AsQueryable();
            
            if (!string.IsNullOrEmpty(searchParameter))
            {
                searchParameter = searchParameter.Trim();
                productsFromDB = 
                    productsFromDB
                    .Where(c => c.Name.Contains(searchParameter));
            }

            return productsFromDB.Include(i => i.Images).ToPagedList<Product>(page ?? 1, pageSize);
        }

        public void Delete(int id)
        {
            Product item = GetProduct(id);
            database.Remove(item);
            database.SaveChanges();
        }
    }
}