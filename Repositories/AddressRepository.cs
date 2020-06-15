using OnlineStore.Models;
using OnlineStore.Repositories.Interfaces;
using OnlineStore.Database;

namespace OnlineStore.Repositories
{
    public class AddressRepository : IAddressRepository
    {
        private OnlineStoreContext database;

        public AddressRepository(OnlineStoreContext database) => this.database = database;

        public void Register(Address address)
        {
            database.Add(address);
            database.SaveChanges();
        }

        public void Update(Address address)
        {
            database.Update(address);
            database.SaveChanges();
        }

        public Address GetAddress(int id) => database.Addresses.Find(id);

        public void Delete(int id)
        {
            Address item = GetAddress(id);
            database.Remove(item);
            database.SaveChanges();
        }
    }
}