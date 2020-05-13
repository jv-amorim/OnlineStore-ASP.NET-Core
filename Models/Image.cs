using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OnlineStore.Libraries.Language;

namespace OnlineStore.Models
{
    public class Image
    {
        public int Id { get; set; }
        public string Path { get; set; }

        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }
    }
}