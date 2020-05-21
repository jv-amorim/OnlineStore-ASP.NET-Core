using OnlineStore.Models;
using X.PagedList;

namespace OnlineStore.Repositories.Interfaces
{
    public interface IProductRepository
    {
        void Register(Product product);
        void Update(Product product);
        void Delete(int id);
        Product GetProduct(int id);
        IPagedList<Product> GetAllProducts(int? page, int pageSize, string searchParameter);
        IPagedList<Product> GetAllProducts(int? page, int pageSize, string searchParameter, string sortingOption);
    }
}