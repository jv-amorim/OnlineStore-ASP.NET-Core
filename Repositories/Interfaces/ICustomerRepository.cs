using System.Collections.Generic;
using OnlineStore.Models;

namespace OnlineStore.Repositories.Interfaces
{
    public interface ICustomerRepository
    {
        Customer Login(string email, string password);

        void Register(Customer customer);
        void Update(Customer customer);
        void Delete(int id);
        Customer GetCustomer(int id);
        IEnumerable<Customer> GetAllCustomers();
    }
}