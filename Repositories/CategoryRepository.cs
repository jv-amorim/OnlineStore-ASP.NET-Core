using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Models;
using OnlineStore.Database;
using OnlineStore.Repositories.Interfaces;
using X.PagedList;

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

        public void Delete(int id)
        {
            Category item = GetCategory(id);
            database.Remove(item);
            database.SaveChanges();
        }

        public Category GetCategory(int id) => database.Categories.Find(id);
        
        public Category GetCategory(string slug) => database.Categories.Where(c => c.Slug == slug).FirstOrDefault();

        public IEnumerable<Category> GetAllCategories() => database.Categories;

        public IPagedList<Category> GetAllCategories(int? page, int pageSize) => 
            database.Categories.Include(c => c.ParentCategory)
            .ToPagedList<Category>(page ?? 1, pageSize);

        public IEnumerable<Category> GetAllChildCategories(int parentCategoryId) => 
            GetAllChildCategoriesHelper(parentCategoryId, null, null);

        private IEnumerable<Category> GetAllChildCategoriesHelper(int parentCategoryId, List<Category> allCategories, List<Category> allChildCategories)
        {
            if (allCategories == null)
            {
                allCategories = GetAllCategories().ToList();
                allChildCategories = new List<Category>();
            }
            
            var childCategories = allCategories.Where(c => c.ParentCategoryId == parentCategoryId);
            
            if (childCategories.Count() > 0)
            {
                allChildCategories.AddRange(childCategories.ToList());
                foreach (var childCategory in childCategories)
                    GetAllChildCategoriesHelper(childCategory.Id, allCategories, allChildCategories);
            }

            return allChildCategories;
        }
    }
}