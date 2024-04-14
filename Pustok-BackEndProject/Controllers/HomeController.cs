using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok_BackEndProject.Contexts;
using Pustok_BackEndProject.ViewModels;

namespace Pustok_BackEndProject.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _context;

    public HomeController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {

        HomeVM vm = new();
        vm.Sliders = await _context.Sliders.ToListAsync();
        vm.Services = await _context.Services.ToListAsync();
        vm.NewProducts = await _context.Products.Include(x => x.ProductImages).Include(x => x.Category).Include(x=>x.Brand).OrderByDescending(x => x.Id).Take(12).ToListAsync();
        vm.DiscountedProducts = await _context.Products.Include(x => x.ProductImages).Include(x => x.Category).Include(x => x.Brand).OrderByDescending(x => x.Discount).Take(12).ToListAsync();
        vm.UndiscountedProducts=await _context.Products.Include(x=>x.ProductImages).Include(x=>x.Category).Include(x => x.Brand).Where(x=>x.Discount==0).Take(12).ToListAsync();
        vm.ChildrenProducts=await _context.Products.Include(x=>x.ProductImages).Include(x=>x.Category).Include(x => x.Brand).Where(x=>x.Category.Name=="Children").Take(6).ToListAsync();
        vm.TwentyDiscountProducts = await _context.Products.Include(x => x.ProductImages).Include(x => x.Category).Include(x => x.Brand).Where(x => x.Discount == 20).Take(6).ToListAsync();
        return View(vm);
    }


}
