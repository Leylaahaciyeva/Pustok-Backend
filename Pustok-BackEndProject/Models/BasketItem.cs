using Pustok_BackEndProject.Models.Common;

namespace Pustok_BackEndProject.Models;

public class BasketItem : BaseModel
{
    public Product Product { get; set; } = null!;
    public int ProductId { get; set; }
    public AppUser AppUser { get; set; } = null!;
    public string AppUserId { get; set; } = null!;
    public int Count { get; set; } = 1;

}
