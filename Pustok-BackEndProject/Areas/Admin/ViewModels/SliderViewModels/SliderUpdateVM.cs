namespace Pustok_BackEndProject.Areas.Admin.ViewModels;

public class SliderUpdateVM
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Button { get; set; } = null!;
    public string ImagePath { get; set; } = null!;
    public IFormFile? Image { get; set; }
}
