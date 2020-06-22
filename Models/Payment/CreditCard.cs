using System.ComponentModel.DataAnnotations;
using OnlineStore.Libraries.Language;

namespace OnlineStore.Models.Payment
{
    public class CreditCard
    {
        [Display(Name = "Holder Name")]
        [Required(ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "MSG_ERROR_001")]
        public string HolderName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "MSG_ERROR_001")]
        [CreditCard(ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "MSG_ERROR_004")]
        public string Number { get; set; }

        [Display(Name = "Expiration Date")]
        [Required(ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "MSG_ERROR_001")]
        public string ExpirationDate { get; set; }

        [Display(Name = "CVV")]
        [Required(ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "MSG_ERROR_001")]
        [MinLength(3, ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "MSG_ERROR_002")]
        [MaxLength(3, ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "MSG_ERROR_003")]
        public string Cvv { get; set; }
    }
}