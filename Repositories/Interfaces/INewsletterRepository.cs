using System.Collections.Generic;
using OnlineStore.Models;

namespace OnlineStore.Repositories.Interfaces
{
    public interface INewsletterRepository
    {
        void Subscribe(NewsletterEmail newsletter);
        IEnumerable<NewsletterEmail> GetAllNewsletterEmails();
    }
}