using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Pustok_BackEndProject.Contexts;
using Pustok_BackEndProject.Models;
using System.Security.Claims;

namespace Pustok_BackEndProject.Controllers;

public class ShopController : Controller
{
    private readonly AppDbContext _context;
    private readonly UserManager<AppUser> _userManager;

    public ShopController(AppDbContext context, UserManager<AppUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index(int? categoryId)
    {

        var query = _context.Products.Include(x => x.Category).Include(x => x.ProductImages).AsQueryable();
        if (categoryId is not null)
            query = query.Where(x => x.CategoryId == categoryId);

        var products = await query.ToListAsync();

        return View(products);
    }



    public async Task<IActionResult> Search(string search)
    {
        var products = await _context.Products.Where(x => x.Name.Trim().ToLower().Contains(search.ToLower().Trim())).Include(x=>x.Category).Include(x=>x.ProductImages).ToListAsync();
        return View("Index",products);
    }


    public async Task<IActionResult> Detail(int id)
    {
        var product = await _context.Products.Include(x => x.Category).Include(x => x.ProductImages).Include(x => x.Brand).FirstOrDefaultAsync(x => x.Id == id);

        if (product is null)
            return NotFound();

        return View(product);
    }


    public async Task<IActionResult> AddToBasket(int id)
    {
        var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);

        if (product is null)
            return NotFound();

        if (User.Identity.IsAuthenticated)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return BadRequest();


            var basket = await _context.BasketItems.Where(x => x.AppUserId == userId).ToListAsync();


            var existBasket = basket.FirstOrDefault(x => x.ProductId == id);

            if (existBasket is not null)
            {
                existBasket.Count++;
                _context.BasketItems.Update(existBasket);
            }
            else
            {
                BasketItem item = new()
                {
                    ProductId = id,
                    AppUserId = userId,
                    Count = 1
                };
                await _context.BasketItems.AddAsync(item);
            }

            await _context.SaveChangesAsync();


            return RedirectToAction("Index");
        }



        var basketItems = GetBasket();


        var existItem = basketItems.FirstOrDefault(x => x.ProductId == id);

        if (existItem is not null)
            existItem.Count++;
        else
        {
            BasketItem basketItem = new() { ProductId = id, Count = 1 };

            basketItems.Add(basketItem);
        }


        var json = JsonConvert.SerializeObject(basketItems);

        Response.Cookies.Append("basket", json);


        return RedirectToAction("Index");
    }



    public async Task<IActionResult> RemoveToBasket(int id)
    {
        if (User.Identity.IsAuthenticated)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);


            var basketItem = await _context.BasketItems.FirstOrDefaultAsync(x => x.ProductId == id && x.AppUserId == userId);

            if (basketItem is null)
                return NotFound();

            _context.BasketItems.Remove(basketItem);
            await _context.SaveChangesAsync();


            return RedirectToAction("Index");

        }

        var basketItems = GetBasket();


        var existItem = basketItems.FirstOrDefault(x => x.ProductId == id);

        if (existItem is null)
            return NotFound();

        basketItems.Remove(existItem);


        var json = JsonConvert.SerializeObject(basketItems);

        Response.Cookies.Append("basket", json);


        return RedirectToAction("Index");
    }





    private List<BasketItem> GetBasket()
    {
        List<BasketItem> basketItems = new();
        if (Request.Cookies["basket"] != null)
        {
            basketItems = JsonConvert.DeserializeObject<List<BasketItem>>(Request.Cookies["basket"]);

        }
        else basketItems = new List<BasketItem>();
        return basketItems;
    }

}
