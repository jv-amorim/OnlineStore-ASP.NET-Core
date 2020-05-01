using System;
using System.ComponentModel.DataAnnotations;
using OnlineStore.Libraries.Language;

namespace OnlineStore.Models
{
    public class Customer
    {
        public int Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "MSG_ERROR_001")]
        [MinLength(4, ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "MSG_ERROR_002")]
        public string Name { get; set; }
        
        [Required(ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "MSG_ERROR_001")]
        public string Sex { get; set; }

        [Required(ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "MSG_ERROR_001")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "MSG_ERROR_001")]
        public string CPF { get; set; }

        [Required(ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "MSG_ERROR_001")]
        public string Phone { get; set; }
        
        [Required(ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "MSG_ERROR_001")]
        [EmailAddress(ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "MSG_ERROR_004")]
        public string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "MSG_ERROR_001")]
        [MinLength(6, ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "MSG_ERROR_002")]
        public string Password { get; set; }
    }
}