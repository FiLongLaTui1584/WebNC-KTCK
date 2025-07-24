using Microsoft.AspNetCore.Mvc;
using _22DH112015_TranPhiLong.Models;
using Microsoft.EntityFrameworkCore;
using System.IO;

public class ProductsController : Controller
{
    private readonly SportProductContext _context;

    public ProductsController(SportProductContext context)
    {
        _context = context;
    }

    public IActionResult Details(int? id)
    {
        Product product;
        if (id.HasValue)
        {
            product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }
        }
        else
        {
            product = _context.Products.OrderBy(p => p.ProductId).FirstOrDefault();
            if (product == null)
            {
                return NotFound();
            }
        }
        return View(product);
    }
        
    [HttpGet]
    public IActionResult Edit(int id)
    {
        var product = _context.Products.Find(id);
        if (product == null)
        {
            return NotFound();
        }
        ViewBag.Categories = new List<string> { "Vợt", "Bóng", "Cầu", "Đệm", "Quần áo" };
        return View(product);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Product product, IFormFile? ImageProFile)
    {
        if (id != product.ProductId)
        {
            return NotFound();
        }

        if (ImageProFile != null)
        {
            // Giới hạn kích thước file (ví dụ: 5MB)
            if (ImageProFile.Length > 5 * 1024 * 1024)
            {
                ModelState.AddModelError("ImageProFile", "File ảnh không được lớn hơn 5MB.");
            }
            // Giới hạn loại file
            if (!ImageProFile.ContentType.StartsWith("image/"))
            {
                ModelState.AddModelError("ImageProFile", "Vui lòng chọn file ảnh (jpg, png, v.v.).");
            }
        }

        if (ModelState.IsValid)
        {
            try
            {
                // Xử lý file ảnh
                if (ImageProFile != null && ImageProFile.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await ImageProFile.CopyToAsync(memoryStream);
                        var fileBytes = memoryStream.ToArray();
                        product.ImagePro = $"data:image/jpeg;base64,{Convert.ToBase64String(fileBytes)}";
                    }
                }
                else
                {
                    var existingProduct = _context.Products.AsNoTracking().FirstOrDefault(p => p.ProductId == id);
                    product.ImagePro = existingProduct?.ImagePro;
                }

                _context.Update(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new { id = product.ProductId });
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Không thể lưu thay đổi. Vui lòng thử lại.");
            }
        }
        ViewBag.Categories = new List<string> { "Vợt", "Bóng", "Cầu", "Đệm", "Quần áo" };
        return View(product);
    }
}