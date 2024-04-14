using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Pustok_BackEndProject.Contexts;
using Pustok_BackEndProject.Models;
using System.Security.Claims;

namespace Pustok_BackEndProject.Controllers;

public class BasketController : Controller
{

    private readonly AppDbContext _context;

    public BasketController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        List<BasketItem> basketItems = new();

        if (User.Identity.IsAuthenticated)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);



            basketItems = await _context.BasketItems.Include(x => x.Product).ThenInclude(x => x.ProductImages).Where(x => x.AppUserId == userId).ToListAsync();

        }
        else
        {
            basketItems = await GetBasket();





            foreach (var item in basketItems)
            {
                var product = await _context.Products.Include(x => x.ProductImages).FirstOrDefaultAsync(x => x.Id == item.ProductId);
                if (product is null)
                    basketItems.Remove(item);
                else
                    item.Product = product;
            }
        }

        return View(basketItems);
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

        var basketItems = await GetBasket();


        var existItem = basketItems.FirstOrDefault(x => x.ProductId == id);

        if (existItem is null)
            return NotFound();

        basketItems.Remove(existItem);


        var json = JsonConvert.SerializeObject(basketItems);

        Response.Cookies.Append("basket", json);


        return RedirectToAction("Index");
    }






    private async Task<List<BasketItem>> GetBasket()
    {
        List<BasketItem> basketItems = new();
        if (Request.Cookies["basket"] != null)
        {
            basketItems = JsonConvert.DeserializeObject<List<BasketItem>>(Request.Cookies["basket"]);

        }
        else basketItems = new List<BasketItem>();

        foreach (var item in basketItems)
        {
            var product = await _context.Products.Include(x => x.ProductImages).FirstOrDefaultAsync(x => x.Id == item.ProductId);
            if (product is null)
                basketItems.Remove(item);

            item.Product = product;
        }

        return basketItems;
    }
}
