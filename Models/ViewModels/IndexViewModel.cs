using X.PagedList;

namespace OnlineStore.Models.ViewModels
{
    public class IndexViewModel
    {
        public NewsletterEmail NewsletterEmail { get; set; }
        public IPagedList<Product> Products { get; set; }
    }
}