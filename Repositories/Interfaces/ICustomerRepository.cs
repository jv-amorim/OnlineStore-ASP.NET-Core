using OnlineStore.Models;
using X.PagedList;

namespace OnlineStore.Repositories.Interfaces
{
    public interface ICustomerRepository
    {
        Customer Login(string email, string password);

        void Register(Customer customer);
        void Update(Customer customer);
        void Delete(int id);
        Customer GetCustomer(int id);
        IPagedList<Customer> GetAllCustomers(int? page, int pageSize);
    }
}