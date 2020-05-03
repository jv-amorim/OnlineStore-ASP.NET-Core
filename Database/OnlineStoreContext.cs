using Microsoft.EntityFrameworkCore;
using OnlineStore.Models;

namespace OnlineStore.Database
{
    public class OnlineStoreContext : DbContext
    {
        public OnlineStoreContext(DbContextOptions<OnlineStoreContext> options) : base(options) { }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<NewsletterEmail> NewsletterEmails { get; set; }
        public DbSet<Collaborator> Collaborators { get; set; }
    }
}