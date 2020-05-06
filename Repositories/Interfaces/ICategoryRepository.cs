using System.Collections.Generic;
using OnlineStore.Models;
using X.PagedList;

namespace OnlineStore.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        void Register(Category category);
        void Update(Category category);
        void Delete(int id);
        Category GetCategory(int id);
        IPagedList<Category> GetAllCategories(int? page, int pageSize);
    }
}