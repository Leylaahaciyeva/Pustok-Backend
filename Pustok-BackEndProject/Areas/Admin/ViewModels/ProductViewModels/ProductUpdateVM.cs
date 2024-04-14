using System.ComponentModel.DataAnnotations;

namespace Pustok_BackEndProject.Areas.Admin.ViewModels;

public class ProductUpdateVM
{
    public string Name { get; set; } = null!;
    public int RewardPoints { get; set; }
    public decimal Price { get; set; }
    public string ProductCode { get; set; } = null!;
    [Range(0, 5)]
    public decimal Rating { get; set; }
    public decimal Discount { get; set; } = 0;
    [MinLength(15)]
    public string Description { get; set; } = null!;
    public string Tags { get; set; } = null!;
    public int CategoryId { get; set; }
    public int BrandId { get; set; }

    public IFormFile? MainImage { get; set; }
    public string? MainImagePath { get; set; }
    public IFormFile? HoverImage { get; set; }
    public string? HoverImagePath { get; set; }
    public ICollection<IFormFile> AdditionalImages { get; set; } = new List<IFormFile>();
    public List<string> AdditionalImagePaths { get; set; } = new();
}
