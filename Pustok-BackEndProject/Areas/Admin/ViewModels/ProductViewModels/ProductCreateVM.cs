using Pustok_BackEndProject.Models;
using System.ComponentModel.DataAnnotations;

namespace Pustok_BackEndProject.Areas.Admin.ViewModels;

public class ProductCreateVM
{
    public string Name { get; set; } = null!;
    public int RewardPoints { get; set; }
    public string ProductCode { get; set; } = null!;
    [Range(0, 5)]
    public decimal Rating { get; set; }
    public decimal Price { get; set; }
    [Range(0,100)]
    public decimal Discount { get; set; } = 0;
    [MinLength(15)]
    public string Description { get; set; } = null!;
    public string Tags { get; set; } = null!;
    public int CategoryId { get; set; }
    public int BrandId { get; set; }

    public IFormFile MainImage { get; set; } = null!;
    public IFormFile HoverImage { get; set; } = null!;
    public ICollection<IFormFile> AdditionalImages { get; set; } = new List<IFormFile>();
}
