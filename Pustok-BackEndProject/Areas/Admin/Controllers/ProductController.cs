using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Pustok_BackEndProject.Areas.Admin.ViewModels;
using Pustok_BackEndProject.Contexts;
using Pustok_BackEndProject.Extensions;
using Pustok_BackEndProject.Models;

namespace Pustok_BackEndProject.Areas.Admin.Controllers;
[Area("Admin")]
public class ProductController : Controller
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _environment;

    public ProductController(AppDbContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }

    public async Task<IActionResult> Index()
    {
        var products = await _context.Products.Include(x => x.ProductImages).ToListAsync();

        return View(products);
    }

    public async Task<IActionResult> Create()
    {
        var categories = await _context.Categories.ToListAsync();
        ViewBag.Categories = categories;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(ProductCreateVM vm)
    {
        var categories = await _context.Categories.ToListAsync();
        ViewBag.Categories = categories;

        if (!ModelState.IsValid)
            return View(vm);

        if (!vm.MainImage.ValidateType("image"))
        {
            ModelState.AddModelError("MainImage", "Invalid image type");
            return View(vm);
        }
        if (!vm.MainImage.ValidateSize(2))
        {
            ModelState.AddModelError("MainImage", "Image max size-2 mb");
            return View(vm);
        }


        if (!vm.HoverImage.ValidateType("image"))
        {
            ModelState.AddModelError("HoverImage", "Invalid image type");
            return View(vm);
        }
        if (!vm.HoverImage.ValidateSize(2))
        {
            ModelState.AddModelError("HoverImage", "Image max size-2 mb");
            return View(vm);
        }


        foreach (var image in vm.AdditionalImages)
        {
            if (!image.ValidateType("image"))
            {
                ModelState.AddModelError("AdditionalImages", "Invalid image type");
                return View(vm);
            }
            if (!image.ValidateSize(2))
            {
                ModelState.AddModelError("AdditionalImages", "Image max size-2 mb");
                return View(vm);
            }
        }


        Product product = new()
        {
            Name = vm.Name,
            Description = vm.Description,
            CategoryId = vm.CategoryId,
            Price = vm.Price,
            Discount = vm.Discount,
            Tags = vm.Tags,
            RewardPoints = vm.RewardPoints,
        };


        var mainImagePath = await vm.MainImage.FileCreateAsync(_environment.WebRootPath, "image");
        ProductImage mainImage = new() { IsMain = true, Url = mainImagePath, Product = product };
        product.ProductImages.Add(mainImage);


        var hoverImagePath = await vm.HoverImage.FileCreateAsync(_environment.WebRootPath, "image");
        ProductImage hoverImage = new() { IsHover = true, Url = hoverImagePath, Product = product };
        product.ProductImages.Add(hoverImage);


        foreach (var image in vm.AdditionalImages)
        {
            var imagePath = await image.FileCreateAsync(_environment.WebRootPath, "image");
            ProductImage pImage = new() { Url = imagePath, Product = product };
            product.ProductImages.Add(pImage);
        }


        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();


        return RedirectToAction("Index");

    }


    public async Task<IActionResult> Delete(int id)
    {
        var product = await _context.Products.Include(x => x.ProductImages).FirstOrDefaultAsync(x => x.Id == id);


        if (product is null)
            return NotFound();


        foreach (var image in product.ProductImages)
        {
            image.Url.FileDelete(_environment.WebRootPath, "image");
        }

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }


    public async Task<IActionResult> Update(int id)
    {
        var product = await _context.Products.Include(x => x.ProductImages).FirstOrDefaultAsync(x => x.Id == id);

        if (product is null)
            return NotFound();

        ProductUpdateVM vm = new()
        {
            AdditionalImagePaths = product.ProductImages.Where(x => !x.IsMain && !x.IsHover).Select(x => x.Url).ToList(),
            MainImagePath = product.ProductImages.FirstOrDefault(x => x.IsMain)?.Url,
            HoverImagePath = product.ProductImages.FirstOrDefault(x => x.IsHover)?.Url,
            Name = product.Name,
            Price = product.Price,
            Discount = product.Discount,
            RewardPoints = product.RewardPoints,
            Tags = product.Tags,
            CategoryId = product.CategoryId,
            Description = product.Description,
        };



        var categories = await _context.Categories.ToListAsync();
        ViewBag.Categories = categories;

        return View(vm);

    }

    [HttpPost]
    public async Task<IActionResult> Update(int id, ProductUpdateVM vm)
    {

        var categories = await _context.Categories.ToListAsync();
        ViewBag.Categories = categories;

        if (!ModelState.IsValid)
            return View(vm);

        var existProduct = await _context.Products.Include(x => x.ProductImages).FirstOrDefaultAsync(x => x.Id == id);

        if (existProduct is null)
            return NotFound();

        if (vm.MainImage is not null)
        {

            if (!vm.MainImage.ValidateType("image"))
            {
                ModelState.AddModelError("MainImage", "Invalid image type");
                return View(vm);
            }
            if (!vm.MainImage.ValidateSize(2))
            {
                ModelState.AddModelError("MainImage", "Image max size-2 mb");
                return View(vm);
            }
        }
        if (vm.HoverImage is not null)
        {


            if (!vm.HoverImage.ValidateType("image"))
            {
                ModelState.AddModelError("HoverImage", "Invalid image type");
                return View(vm);
            }
            if (!vm.HoverImage.ValidateSize(2))
            {
                ModelState.AddModelError("HoverImage", "Image max size-2 mb");
                return View(vm);
            }
        }


        foreach (var image in vm.AdditionalImages)
        {
            if (!image.ValidateType("image"))
            {
                ModelState.AddModelError("AdditionalImages", "Invalid image type");
                return View(vm);
            }
            if (!image.ValidateSize(2))
            {
                ModelState.AddModelError("AdditionalImages", "Image max size-2 mb");
                return View(vm);
            }


        }



        existProduct.Name = vm.Name;
        existProduct.RewardPoints = vm.RewardPoints;
        existProduct.Price = vm.Price;
        existProduct.Discount = vm.Discount;
        existProduct.Tags = vm.Tags;
        existProduct.CategoryId = vm.CategoryId;
        existProduct.Description = vm.Description;


        if (vm.MainImage is not null)
        {

            var mainImagePath = await vm.MainImage.FileCreateAsync(_environment.WebRootPath, "image");
            var existMainImg = existProduct.ProductImages.FirstOrDefault(x => x.IsMain);
            existMainImg?.Url.FileDelete(_environment.WebRootPath, "image");
            existMainImg.Url = mainImagePath;

        }


        if (vm.HoverImage is not null)
        {
            var hoverImagePath = await vm.HoverImage.FileCreateAsync(_environment.WebRootPath, "image");
            var existHoverImg = existProduct.ProductImages.FirstOrDefault(x => x.IsHover);
            existHoverImg?.Url.FileDelete(_environment.WebRootPath, "image");
            existHoverImg.Url = hoverImagePath;

        }





        if (vm.AdditionalImages.Count > 0)
        {
            var existImages = existProduct.ProductImages.Where(x => !x.IsMain && !x.IsHover).ToList();
            foreach (var existImage in existImages)
            {
                existProduct.ProductImages.Remove(existImage);
            }
            foreach (var image in vm.AdditionalImages)
            {


                var imagePath = await image.FileCreateAsync(_environment.WebRootPath, "image");
                ProductImage pImage = new() { Url = imagePath, Product = existProduct };
                existProduct.ProductImages.Add(pImage);
            }



        }


        _context.Products.Update(existProduct);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");


    }
}