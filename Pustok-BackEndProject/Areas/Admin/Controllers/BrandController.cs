using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok_BackEndProject.Areas.Admin.ViewModels;
using Pustok_BackEndProject.Contexts;
using Pustok_BackEndProject.Models;

namespace Pustok_BackEndProject.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles ="Admin")]
public class BrandController : Controller
{
    private readonly AppDbContext _context;

    public BrandController(AppDbContext context)
    {
        _context = context;
    }
    public async Task<IActionResult> Index()
    {
        var Brands = await _context.Brands.ToListAsync();

        return View(Brands);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(BrandCreateVM vm)
    {
        if (!ModelState.IsValid)
            return View();

        Brand Brand = new Brand()
        {

            Name = vm.Name,
        };


        await _context.Brands.AddAsync(Brand);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }
    public async Task<IActionResult> Delete(int id)
    {
        var Brand = await _context.Brands.FirstOrDefaultAsync(x => x.Id == id);

        if (Brand is null)
            return NotFound();

        _context.Brands.Remove(Brand);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }


    public async Task<IActionResult> Update(int id)
    {
        var Brand = await _context.Brands.FirstOrDefaultAsync(x => x.Id == id);
        if (Brand is null)
            return NotFound();


        BrandUpdateVM vm = new()
        {
            Name = Brand.Name,
        };

        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Update(int id, BrandUpdateVM vm)
    {
        if (!ModelState.IsValid)
            return View(vm);

        var existBrand = await _context.Brands.FirstOrDefaultAsync(x => x.Id == id);

        if (existBrand is null)
            return NotFound();

        existBrand.Name = vm.Name;

        _context.Brands.Update(existBrand);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }
}
