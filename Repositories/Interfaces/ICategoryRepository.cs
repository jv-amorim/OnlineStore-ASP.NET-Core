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
        Category GetCategory(string slug);
        IEnumerable<Category> GetAllCategories();
        IEnumerable<Category> GetAllChildCategories(int parentCategoryId);
        IPagedList<Category> GetAllCategories(int? page, int pageSize);
    }
}