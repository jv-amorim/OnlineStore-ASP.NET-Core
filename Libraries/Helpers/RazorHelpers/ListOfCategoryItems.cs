using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineStore.Models;

namespace OnlineStore.Libraries.Helpers.RazorHelpers
{
    /// <summary>Useful methods to use SelectListItem of categories in "select" HTML tag (options dropdown).</summary>
    public static class SelectListItemHelpers
    {
        /// <summary>Creates a SelectListItem of categories.</summary>
        public static IEnumerable<SelectListItem> CreateNewListOfCategoryItems(IEnumerable<Category> categories)
        {
            var SelectListItemHelpers = categories.Select(c => new SelectListItem(c.Name, c.Id.ToString()));
            return SelectListItemHelpers;
        }
    }
}