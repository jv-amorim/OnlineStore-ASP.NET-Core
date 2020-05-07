using Microsoft.EntityFrameworkCore;
using OnlineStore.Models;
using OnlineStore.Database;
using OnlineStore.Repositories.Interfaces;
using X.PagedList;
using System.Collections.Generic;

namespace OnlineStore.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private OnlineStoreContext database;

        public CategoryRepository(OnlineStoreContext database) => this.database = database;

        public void Register(Category category)
        {
            database.Add(category);
            database.SaveChanges();
        }

        public void Update(Category category)
        {
            database.Update(category);
            database.SaveChanges();
        }

        public Category GetCategory(int id) => database.Categories.Find(id);

        public IEnumerable<Category> GetAllCategories() => database.Categories;

        public IPagedList<Category> GetAllCategories(int? page, int pageSize) => 
            database.Categories.Include(c => c.ParentCategory)
            .ToPagedList<Category>(page ?? 1, pageSize);

        public void Delete(int id)
        {
            Category item = GetCategory(id);
            database.Remove(item);
            database.SaveChanges();
        }
    }
}