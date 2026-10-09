
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mvc.Data;
using mvc.Models;

namespace mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BrandController : Controller
    {
        private readonly ApplicationDBContext _context = new ApplicationDBContext();
        public IActionResult Index()
        {
            var brands = _context.Brands.AsQueryable();
            return View(brands.AsEnumerable());
        }
        [HttpGet]
        public IActionResult Create()
        {

            return View();
        }
        [HttpPost]
        public IActionResult Create(Brand brand, IFormFile ImageFile)

        {
            if (ImageFile != null && ImageFile.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + "-" + ImageFile.FileName;
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Admin\\img", fileName);
                using (var stream = System.IO.File.Create(filePath))
                {
                    ImageFile.CopyTo(stream);
                }
                brand.Logo = fileName;
            }
            _context.Brands.Add(brand);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult Update(int id)
        {
            var brand = _context.Brands.FirstOrDefault(c => c.Id == id);
            if (brand == null)
            {
                return RedirectToAction("NotFoundPage", "Home");
            }
            return View(brand);
        }
        [HttpPost]
        public IActionResult Update(Brand brand, IFormFile ImageFile)

        {
            var brandInDb = _context.Brands.AsNoTracking().FirstOrDefault(c => c.Id == brand.Id);
            if (ImageFile != null && ImageFile.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + "-" + ImageFile.FileName;
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Admin\\img", fileName);
                using (var stream = System.IO.File.Create(filePath))
                {
                    ImageFile.CopyTo(stream);
                }
                brand.Logo = fileName;

                var oldfilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Admin\\img", brandInDb.Logo);

                if (System.IO.File.Exists(oldfilePath))
                {
                    System.IO.File.Delete(oldfilePath);
                }
            }
            else
            {
                brand.Logo = brandInDb.Logo;
            }
            _context.Brands.Update(brand);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Delete(int id)

        {
            var brand = _context.Brands.FirstOrDefault(c => c.Id == id);

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Admin\\img", brand.Logo);

            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }


            if (brand == null)
            {
                return RedirectToAction("NotFoundPage", "Home");
            }
            _context.Brands.Remove(brand);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }



    }
}
