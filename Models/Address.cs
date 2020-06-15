using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OnlineStore.Libraries.Language;

namespace OnlineStore.Models
{
    public class Address
    {
        public int Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "MSG_ERROR_001")]
        [MinLength(8, ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "MSG_ERROR_002")]
        [MaxLength(8, ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "MSG_ERROR_003")]
        public string Cep { get; set; }

        [Required(ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "MSG_ERROR_001")]
        public string State { get; set; }

        [Required(ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "MSG_ERROR_001")]
        public string City { get; set; }

        [Required(ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "MSG_ERROR_001")]
        public string Neighborhood { get; set; }

        [Display(Name = "Address")]
        [Required(ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "MSG_ERROR_001")]
        public string AddressLine { get; set; }

        public string Complement { get; set; }

        [Required(ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "MSG_ERROR_001")]
        public string Number { get; set; }

        [NotMapped]
        private const string EmptyAddressCep = "AAAAAAAA";

        public static Address InstantiateEmptyAddress()
        {
            return new Address()
            {
                Cep = EmptyAddressCep,
                State = "",
                City = "",
                Neighborhood = "",
                AddressLine = "",
                Complement = "",
                Number = ""
            };
        }

        public static bool IsTheAddressEmpty(Address address)
        {
            if (address.Cep == EmptyAddressCep)
                return true;
            return false;
        }
    }
}