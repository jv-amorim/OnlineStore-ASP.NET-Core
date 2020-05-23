using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineStore.Libraries.Enums;
using X.PagedList;

namespace OnlineStore.Models.ViewModels
{
    public class ProductListViewModel
    {
        public IPagedList<Product> Products { get; set; }
        public List<SelectListItem> SortingOptionsList
        { 
            get => new List<SelectListItem>()
            {
                new SelectListItem("Select an option", ""),
                new SelectListItem("Alphabetical: A-Z", SortingOptions.Alphabetical_Ascending.ToString()),
                new SelectListItem("Alphabetical: Z-A", SortingOptions.Alphabetical_Descending.ToString()),
                new SelectListItem("Price: Low to High", SortingOptions.Price_LowToHigh.ToString()),
                new SelectListItem("Price: High to Low", SortingOptions.Price_HighToLow.ToString())
            };
        }
    }
}