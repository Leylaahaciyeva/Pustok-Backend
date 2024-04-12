using Pustok_BackEndProject.Models.Common;

namespace Pustok_BackEndProject.Models;

public class Category : BaseModel
{
    public string Name { get; set; } = null!;

    public ICollection<Product> Products { get; set; } = new List<Product>();
}
