using System.Collections.Generic;
using OnlineStore.Models;

namespace OnlineStore.Repositories.Interfaces
{
    public interface IImageRepository
    {
        void Register(Image image);
        void Register(List<Image> images);
        void Delete(int id);
        void Delete(string path);
        void DeleteAllProductImages(int productId);
    }
}