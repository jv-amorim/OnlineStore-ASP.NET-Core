using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineStore.Models
{
    public class Image
    {
        public int Id { get; set; }
        public string Path { get; set; }

        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        public static List<Image> CreateImageList(List<string> paths, int productId)
        {
            List<Image> images = new List<Image>();
            
            foreach (string path in paths)
            {
                if(string.IsNullOrEmpty(path))
                    continue;

                images.Add(new Image()
                {
                    Path = path,
                    ProductId = productId
                });
            }

            return images;
        }
    }
}