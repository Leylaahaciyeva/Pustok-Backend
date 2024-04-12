using Pustok_BackEndProject.Models.Common;
using System.ComponentModel.DataAnnotations;

namespace Pustok_BackEndProject.Models;

public class Product : BaseModel
{
    public string Name { get; set; } = null!;
    public int RewardPoints { get; set; }
    public string ProductCode { get; set; } = null!;
    public bool IsStock { get; set; } = true;
    [DataType(DataType.Currency)]
    public decimal Price { get; set; }
    [Range(0,100)]
    public decimal Discount { get; set; } = 0;
    [Range(0, 5)]
    public decimal Rating { get; set; } = 5;
    [MinLength(15)]
    public string Description { get; set; } = null!;
    public string Tags { get; set; } = null!;
    public Category Category { get; set; } = null!;
    public int CategoryId { get; set; }
    public Brand Brand { get; set; }=null!;
    public int BrandId{ get; set; }


    public ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();
}
