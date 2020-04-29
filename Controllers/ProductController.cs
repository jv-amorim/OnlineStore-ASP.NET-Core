using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Models;

namespace OnlineStore.Controllers
{
    public class ProductController : Controller
    {
        public ActionResult ViewProduct()
        {
            Product newProduct = GetProduct();
            
            return View(newProduct);
        }

        private Product GetProduct()
        {
            return new Product()
            {
                Id = 1,
                Name = "Xbox",
                Description = "Play games!",
                Value = 2000.00M
            };
        }
    }
}