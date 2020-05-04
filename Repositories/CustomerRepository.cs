using System.Collections.Generic;
using System.Linq;
using OnlineStore.Models;
using OnlineStore.Database;
using OnlineStore.Repositories.Interfaces;

namespace OnlineStore.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private OnlineStoreContext database;

        public CustomerRepository(OnlineStoreContext database) => this.database = database;

        public void Register(Customer customer)
        {
            database.Add(customer);
            database.SaveChanges();
        }

        public void Update(Customer customer)
        {
            database.Update(customer);
            database.SaveChanges();
        }

        public Customer GetCustomer(int id) => database.Customers.Find(id);

        public IEnumerable<Customer> GetAllCustomers() => database.Customers;

        public void Delete(int id)
        {
            Customer item = GetCustomer(id);
            database.Remove(item);
            database.SaveChanges();
        }

        public Customer Login(string email, string password)
        {
            return 
                database.Customers
                .Where(item => item.Email == email && item.Password == password)
                .FirstOrDefault();
        }
    }
}