namespace Pustok_BackEndProject.Areas.Admin.ViewModels;

public class SliderCreateVM
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Button { get; set; } = null!;
    public IFormFile Image { get; set; } = null!;
}
