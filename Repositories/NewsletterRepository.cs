using System.Collections.Generic;
using OnlineStore.Models;
using OnlineStore.Repositories.Interfaces;
using OnlineStore.Database;

namespace OnlineStore.Repositories
{
    public class NewsletterRepository : INewsletterRepository
    {
        private OnlineStoreContext database;

        public NewsletterRepository(OnlineStoreContext database) => this.database = database;
        
        public void Subscribe(NewsletterEmail newsletter)
        {
            database.NewsletterEmails.Add(newsletter);
            database.SaveChanges();
        }

        public IEnumerable<NewsletterEmail> GetAllNewsletterEmails() => database.NewsletterEmails;
    }
}