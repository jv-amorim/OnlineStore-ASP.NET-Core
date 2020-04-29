using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Models;

namespace OnlineStore.Database
{
    public class OnlineStoreContext : DbContext
    {
        public OnlineStoreContext(DbContextOptions<OnlineStoreContext> options) : base(options)
        {
            
        }

        public DbSet<Customer> Customers { get; set; }
    }
}