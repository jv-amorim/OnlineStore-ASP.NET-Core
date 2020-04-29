using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using OnlineStore.Libraries.Language;

namespace OnlineStore.Models
{
    public class Contact
    {
        [Required(ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "MSG_ERROR_001")]
        [MinLength(4, ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "MSG_ERROR_002")]
        public string Name { get; set; }

        [Required(ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "MSG_ERROR_001")]
        [EmailAddress(ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "MSG_ERROR_004")]
        public string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "MSG_ERROR_001")]
        [MinLength(10, ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "MSG_ERROR_002")]
        [MaxLength(1000, ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "MSG_ERROR_003")]
        public string Text { get; set; }
    }
}