using OnlineStore.Models;
using X.PagedList;

namespace OnlineStore.Repositories.Interfaces
{
    public interface IImageRepository
    {
        void Register(Image image);
        void Delete(int id);
        void DeleteAllProductImages(int productId);
    }
}