using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Models
{
    public class Customer
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Sexo { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string CPF { get; set; }
        public string Phone { get; set; }

        public string Email { get; set; }
        public string Senha { get; set; }
    }
}