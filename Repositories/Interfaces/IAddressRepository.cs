using OnlineStore.Models;

namespace OnlineStore.Repositories.Interfaces
{
    public interface IAddressRepository
    {
        void Register(Address address);
        void Update(Address address);
        Address GetAddress(int id);
        void Delete(int id);
    }
}