using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Pustok_BackEndProject.Contexts;
using Pustok_BackEndProject.Models;
using System.Security.Claims;

namespace Pustok_BackEndProject.Services;

public class LayoutService : ILayoutService
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly AppDbContext _context;
    private readonly UserManager<AppUser> _userManager;

    public LayoutService(IHttpContextAccessor contextAccessor, AppDbContext context, UserManager<AppUser> userManager)
    {
        _contextAccessor = contextAccessor;
        _context = context;
        _userManager = userManager;
    }
    public async Task<List<Category>> GetCategories()
    {
        var categories = await _context.Categories.Include(x => x.Products).Where(x => x.Products.Count > 0).ToListAsync();
        return categories;
    }
    public async Task<List<BasketItem>> GetBasketItems()
    {
        List<BasketItem> basketItems = new();

        if (_contextAccessor.HttpContext.User.Identity.IsAuthenticated)
        {
            var userId = _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await _userManager.FindByIdAsync(userId);


            if (user is null)
                return basketItems;


            basketItems = await _context.BasketItems.Include(x => x.Product).ThenInclude(x => x.ProductImages).Where(x => x.AppUserId == userId).ToListAsync();

        }
        else
        {
            basketItems = await GetBasket();
        }



        return basketItems;
    }




    private async Task<List<BasketItem>> GetBasket()
    {
        List<BasketItem> basketItems = new();
        if (_contextAccessor.HttpContext.Request.Cookies["basket"] != null)
        {
            basketItems = JsonConvert.DeserializeObject<List<BasketItem>>(_contextAccessor.HttpContext.Request.Cookies["basket"]);

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

public interface ILayoutService
{
    Task<List<BasketItem>> GetBasketItems();
    Task<List<Category>> GetCategories();

}
