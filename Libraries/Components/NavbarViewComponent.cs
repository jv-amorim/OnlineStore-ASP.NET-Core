using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Models;
using OnlineStore.Repositories.Interfaces;

namespace OnlineStore.Libraries.Components
{
    public class NavbarViewComponent : ViewComponent
    {
        private ICategoryRepository categoryRepository;

        public NavbarViewComponent(ICategoryRepository categoryRepository) => this.categoryRepository = categoryRepository;

        public IViewComponentResult Invoke()
        {
            List<Category> categories = categoryRepository.GetAllCategories().ToList();
            return View(categories);
        }
    }
}