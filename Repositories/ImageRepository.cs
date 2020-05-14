using System.Linq;
using System.Collections.Generic;
using OnlineStore.Database;
using OnlineStore.Models;
using OnlineStore.Repositories.Interfaces;

namespace OnlineStore.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private OnlineStoreContext database;

        public ImageRepository(OnlineStoreContext database) => this.database = database;

        public void Register(Image image)
        {
            database.Add(image);
            database.SaveChanges();
        }
        
        public void Delete(int id)
        {
            Image image = database.Images.Find(id);
            database.Remove(image);
            database.SaveChanges();
        }

        public void DeleteAllProductImages(int productId)
        {
            List<Image> images = 
                database.Images.Where(a => a.ProductId == productId).ToList();

            foreach (Image image in images)
                database.Remove(image);

            database.SaveChanges();
        }
    }
}