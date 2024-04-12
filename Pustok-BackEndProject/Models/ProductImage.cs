using Pustok_BackEndProject.Models.Common;

namespace Pustok_BackEndProject.Models;

public class ProductImage : BaseModel
{
    public string Url { get; set; } = null!;
    public bool IsMain { get; set; } = false;
    public bool IsHover { get; set; } = false;
    public Product Product { get; set; } = null!;
    public int ProductId { get; set; }
}
