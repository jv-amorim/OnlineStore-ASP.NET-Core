using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OnlineStore.Libraries.Language;
using OnlineStore.Libraries.Validation.Collaborator;

namespace OnlineStore.Models
{
    public class Collaborator
    {
        [Display(Name = "ID")]
        public int Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "MSG_ERROR_001")]
        [MinLength(4, ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "MSG_ERROR_002")]
        public string Name { get; set; }

        [Required(ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "MSG_ERROR_001")]
        [EmailAddress(ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "MSG_ERROR_004")]
        [UniqueEmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "MSG_ERROR_001")]
        [MinLength(6, ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "MSG_ERROR_002")]
        public string Password { get; set; }

        [NotMapped]
        [Display(Name = "Password Confirmation")]
        [Compare("Password", ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "MSG_ERROR_005")]
        public string PasswordConfirmation { get; set; }

        [Display(Name = "Administrator")]
        public bool IsAdministrator { get; set; }
    }
}