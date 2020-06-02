using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OnlineStore.Libraries.Language;

namespace OnlineStore.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "MSG_ERROR_001")]
        public string Name { get; set; }
        
        [Required(ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "MSG_ERROR_001")]
        public string Description { get; set; }
        
        [Display(Name = "Unit Price")]
        [Required(ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "MSG_ERROR_001")]
        [Column(TypeName = "decimal(18,4)")]
        public decimal UnitPrice { get; set; }

        [Display(Name = "Units In Stock")]
        [Required(ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "MSG_ERROR_001")]
        [Range(0, 1000000, ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "MSG_ERROR_010")]
        public int UnitsInStock { get; set; }

        // The range values of the four following properties are as requested in the Correios (brazilian company) specifications.
        
        [Required(ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "MSG_ERROR_001")]
        [Range(0.001, 30, ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "MSG_ERROR_010")]
        public double Weight { get; set; }

        [Required(ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "MSG_ERROR_001")]
        [Range(11, 105, ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "MSG_ERROR_010")]
        public double Width { get; set; }

        [Required(ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "MSG_ERROR_001")]
        [Range(2, 105, ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "MSG_ERROR_010")]
        public double Height { get; set; }

        [Required(ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "MSG_ERROR_001")]
        [Range(16, 105, ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "MSG_ERROR_010")]
        public double Length { get; set; }

        [Display(Name = "Category")]
        [Required(ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "MSG_ERROR_001")]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

        public ICollection<Image> Images { get; set; }
    }
}