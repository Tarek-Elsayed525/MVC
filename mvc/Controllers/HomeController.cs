using Microsoft.AspNetCore.Mvc;
using mvc.Data;
using mvc.Models;
using mvc.ViewModel;
using System.Diagnostics;
namespace mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDBContext _context = new ApplicationDBContext();
        public IActionResult Index(filtreiationVM filtreiationVM)
        {
            var product = _context.Products.AsQueryable();
            if (filtreiationVM.ProductName != null)
            {
                product = product.Where(p => p.Name.Contains(filtreiationVM.ProductName));

            }
            if (filtreiationVM.MinPrice > 0)
            {
                product = product.Where(p => p.price >= filtreiationVM.MinPrice);

            }
            if (filtreiationVM.MaxPrice > 0)
            {
                product = product.Where(p => p.price <= filtreiationVM.MaxPrice);
            }
            if (filtreiationVM.CategoryId > 0)
            {
                product = product.Where(p => p.CategoryId == filtreiationVM.CategoryId);
            }
            if (filtreiationVM.BrandId > 0)
            {
                product = product.Where(p => p.BrandId == filtreiationVM.BrandId);
            }
            if (filtreiationVM.IsHot)
            {
                product = product.Where(p => p.Discount > 40);
            }

            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Brands = _context.Brands.ToList();
            ViewBag.TotalPages = (int)Math.Ceiling(product.Count() / 8.0);
            ViewBag.CurrentPage = filtreiationVM.Page;
            if (filtreiationVM.Page < 1)
            {
                filtreiationVM.Page = 1;
            }
            if (filtreiationVM.Page > ViewBag.TotalPages)
            {
                filtreiationVM.Page = ViewBag.TotalPages;
            }

            product = product.Skip((filtreiationVM.Page - 1) * 8).Take(8);


            return View(product.ToList());
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Welcome()
        {
            return View();
        }
        public IActionResult personalinfo(decimal amount, string name)
        {
            var personsID = new List<Person>()
            {
                new Person(){id = 1 , name = "tarek" , salary = 500},
                new Person(){id = 2 , name = "mohamed" , salary = 1000},
                new Person(){id = 3 , name = "ahmed" , salary = 850}
            };
            var persons = personsID.Where(p => p.salary > amount);
            if (name != null)
            {
                persons = persons.Where(p => p.name.Contains(name));
            }
            int count = persons.Count();


            return View(new PersonVM
            {
                persons = persons,
                count = count
            });
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
