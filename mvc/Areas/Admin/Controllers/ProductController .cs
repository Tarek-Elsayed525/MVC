using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mvc.Data;
using mvc.Models;
using mvc.ViewModel;

namespace mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly ApplicationDBContext _context = new ApplicationDBContext();
        public IActionResult Index(filtreiationVM filtreiationVM)
        {
            // 1. التحقق من رقم الصفحة أولاً وقبل أي شيء لمنع أي قيم سالبة أو صفرية
            if (filtreiationVM.Page < 1)
            {
                filtreiationVM.Page = 1;
            }

            var products = _context.Products.AsQueryable();
            products = products.Include(c => c.Category).Include(c => c.Brand);

            // تطبيق الفلاتر
            if (!string.IsNullOrEmpty(filtreiationVM.ProductName))
            {
                products = products.Where(p => p.Name.Contains(filtreiationVM.ProductName));
            }
            if (filtreiationVM.MinPrice > 0)
            {
                products = products.Where(p => p.price >= filtreiationVM.MinPrice);
            }
            if (filtreiationVM.MaxPrice > 0)
            {
                products = products.Where(p => p.price <= filtreiationVM.MaxPrice);
            }
            if (filtreiationVM.CategoryId > 0)
            {
                products = products.Where(p => p.CategoryId == filtreiationVM.CategoryId);
            }
            if (filtreiationVM.BrandId > 0)
            {
                products = products.Where(p => p.BrandId == filtreiationVM.BrandId);
            }
            if (filtreiationVM.IsHot)
            {
                products = products.Where(p => p.Quantity <= 55);
            }


            // 2. حساب عدد العناصر وإجمالي الصفحات بعد الفلترة
            int totalItems = products.Count();
            int pageSize = 8;
            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            // حماية إضافية لو إجمالي الصفحات صفر، نجعلها 1 على الأقل
            if (totalPages < 1)
            {
                totalPages = 1;
            }

            // 3. التأكد من أن الصفحة الحالية لا تتجاوز الحد الأقصى لإجمالي الصفحات
            if (filtreiationVM.Page > totalPages)
            {
                filtreiationVM.Page = totalPages;
            }

            // تجهيز الـ ViewBag
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Brands = _context.Brands.ToList();
            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = filtreiationVM.Page;

            // 4. تطبيق الـ Skip والـ Take بأمان تام بعد ضبط القيم
            products = products.Skip((filtreiationVM.Page - 1) * pageSize).Take(pageSize);

            return View(products.AsEnumerable());
        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Brands = _context.Brands.ToList();

            return View();
        }
        [HttpPost]
        public IActionResult Create(Product product, IFormFile ImageFile, List<IFormFile> SubImageFiles, List<string> Colors)
        {
            if (ImageFile != null && ImageFile.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + "-" + ImageFile.FileName;
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Admin\\img", fileName);
                using (var stream = System.IO.File.Create(filePath))
                {
                    ImageFile.CopyTo(stream);
                }
                product.MainImg = fileName;
            }
            _context.Products.Add(product);
            _context.SaveChanges();



            if (SubImageFiles != null && SubImageFiles.Count > 0)
            {
                foreach (var Image in SubImageFiles)
                {
                    if (Image != null && Image.Length > 0)
                    {
                        var fileName = Guid.NewGuid().ToString() + "-" + Image.FileName;
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Admin\\img", fileName);
                        using (var stream = System.IO.File.Create(filePath))
                        {
                            Image.CopyTo(stream);
                        }

                        _context.ProductSubImgs.Add(new ProductSubImg()
                        {
                            Image = fileName,
                            ProductId = product.Id
                        });
                    }
                }
                _context.SaveChanges();

                if (Colors != null && Colors.Count > 0)
                {
                    foreach (var color in Colors)
                    {
                        _context.ProductColors.Add(new ProductColor()
                        {
                            Color = color,
                            ProductId = product.Id
                        });
                    }
                    _context.SaveChanges();
                }
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            var product = _context.Products.Include(c => c.Category).Include(c => c.Brand).FirstOrDefault(c => c.Id == id);



            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Brands = _context.Brands.ToList();
            ViewBag.SubImages = _context.ProductSubImgs.Where(c => c.ProductId == id).ToList();
            ViewBag.Colors = _context.ProductColors.Where(c => c.ProductId == id).ToList();

            if (product == null)
            {
                return RedirectToAction("NotFoundPage", "Home");
            }
            return View(product);
        }


        [HttpPost]
        public IActionResult Update(Product product, IFormFile ImageFile, List<IFormFile> SubImageFiles, List<string> Colors, List<string> ExistingSubImages)
        {
            var productInDb = _context.Products.AsNoTracking().FirstOrDefault(c => c.Id == product.Id);
            if (productInDb == null)
            {
                return NotFound();
            }

            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Admin/img");

            // 1. معالجة الصورة الرئيسية
            if (ImageFile != null && ImageFile.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + "-" + ImageFile.FileName;
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    ImageFile.CopyTo(stream);
                }
                product.MainImg = fileName;

                if (!string.IsNullOrEmpty(productInDb.MainImg))
                {
                    var oldfilePath = Path.Combine(uploadsFolder, productInDb.MainImg);
                    if (System.IO.File.Exists(oldfilePath))
                    {
                        System.IO.File.Delete(oldfilePath);
                    }
                }
            }
            else
            {
                product.MainImg = productInDb.MainImg;
            }

            _context.Products.Update(product);
            _context.SaveChanges();

            // 2. معالجة الصور الفرعية (حذف التي أزالها المستخدم والاحتفاظ بالباقي)
            var dbSubImages = _context.ProductSubImgs.Where(c => c.ProductId == product.Id).ToList();

            if (ExistingSubImages == null)
            {
                ExistingSubImages = new List<string>();
            }

            foreach (var dbImg in dbSubImages)
            {
                // لو الصورة غير موجودة في القائمة المرسلة من الفيو، إذن المستخدم حذفها بـ ×
                if (!ExistingSubImages.Contains(dbImg.Image))
                {
                    string filePath = Path.Combine(uploadsFolder, dbImg.Image);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                    _context.ProductSubImgs.Remove(dbImg);
                }
            }

            // إضافة الصور الفرعية الجديدة لو وجد
            if (SubImageFiles != null && SubImageFiles.Count > 0)
            {
                foreach (var image in SubImageFiles)
                {
                    if (image != null && image.Length > 0)
                    {
                        var fileName = Guid.NewGuid().ToString() + "-" + image.FileName;
                        var filePath = Path.Combine(uploadsFolder, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            image.CopyTo(stream);
                        }

                        _context.ProductSubImgs.Add(new ProductSubImg()
                        {
                            Image = fileName,
                            ProductId = product.Id
                        });
                    }
                }
            }
            _context.SaveChanges();

            // 3. معالجة الألوان (مسح الألوان القديمة بالكامل وإعادة إدخال الموجودة حالياً)
            var existingColors = _context.ProductColors.Where(c => c.ProductId == product.Id).ToList();
            _context.ProductColors.RemoveRange(existingColors);

            if (Colors != null && Colors.Count > 0)
            {
                foreach (var color in Colors)
                {
                    if (!string.IsNullOrEmpty(color))
                    {
                        _context.ProductColors.Add(new ProductColor()
                        {
                            Color = color,
                            ProductId = product.Id
                        });
                    }
                }
            }
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }



        public IActionResult Delete(int id)

        {
            var product = _context.Products.FirstOrDefault(c => c.Id == id);

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Admin\\img", product.MainImg);

            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            var oldSubImages = _context.ProductSubImgs.Where(c => c.ProductId == product.Id).ToList();

            foreach (var dbImg in oldSubImages)
            {
                var oldfilepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Admin/img", dbImg.Image);

                if (System.IO.File.Exists(oldfilepath))
                {
                    System.IO.File.Delete(oldfilepath);
                }

            }

            if (product == null)
            {
                return RedirectToAction("NotFoundPage", "Home");
            }
            _context.Products.Remove(product);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }



    }
}
