using System.Linq;
using OnlineStore.Models;
using OnlineStore.Database;
using OnlineStore.Repositories.Interfaces;
using X.PagedList;

namespace OnlineStore.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private OnlineStoreContext database;

        public CustomerRepository(OnlineStoreContext database) => this.database = database;

        public void Register(Customer customer)
        {
            customer.IsTheAccountActive = true;
            database.Add(customer);
            database.SaveChanges();
        }

        public void Update(Customer customer)
        {
            database.Update(customer);
            database.SaveChanges();
        }

        public Customer GetCustomer(int id) => database.Customers.Find(id);

        public IPagedList<Customer> GetAllCustomers(int? page, int pageSize) =>
            database.Customers.ToPagedList<Customer>(page ?? 1, pageSize);

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