using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OnlineStore.Libraries.Language;

namespace OnlineStore.Models
{
    public class Category
    {
        [Display(Name = "ID")]
        public int Id { get; set; }

        // TODO - Create validation to ensure that there are no repeated category names in the DB.
        [Required(ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "MSG_ERROR_001")]
        [MinLength(4, ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "MSG_ERROR_002")]
        public string Name { get; set; }

        [Required(ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "MSG_ERROR_001")]
        [MinLength(4, ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "MSG_ERROR_002")]
        public string Slug { get; set; }

        [Display(Name = "Parent Category")]
        public int? ParentCategoryId { get; set; }

        [ForeignKey("ParentCategoryId")]
        public Category ParentCategory { get; set; }
    }
}