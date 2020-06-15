using System.Linq;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Models;
using OnlineStore.Database;
using OnlineStore.Repositories.Interfaces;
using X.PagedList;

namespace OnlineStore.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private OnlineStoreContext database;
        private IAddressRepository addressRepository;

        public CustomerRepository(OnlineStoreContext database, IAddressRepository addressRepository)
        {
            this.database = database;
            this.addressRepository = addressRepository;
        }

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

        public Customer GetCustomer(int id) => database.Customers.Include(c => c.Addresses).SingleOrDefault(c => c.Id == id);

        public IPagedList<Customer> GetAllCustomers(int? page, int pageSize, string searchParameter)
        {
            var customersFromDB = database.Customers.Include(c => c.Addresses).AsQueryable();
            
            if (!string.IsNullOrEmpty(searchParameter))
            {
                searchParameter = searchParameter.Trim();
                customersFromDB = 
                    customersFromDB
                    .Where(c => c.Name.Contains(searchParameter) 
                    || c.Email.Contains(searchParameter));
            }

            return customersFromDB.ToPagedList<Customer>(page ?? 1, pageSize);
        }

        public void Delete(int id)
        {
            Customer item = GetCustomer(id);

            foreach (var address in item.Addresses)
                addressRepository.Delete(address.Id);

            database.Remove(item);
            database.SaveChanges();
        }

        public Customer Login(string email, string password)
        {
            return 
                database.Customers
                .Include(c => c.Addresses)
                .Where(item => item.Email == email && item.Password == password)
                .FirstOrDefault();
        }
    }
}