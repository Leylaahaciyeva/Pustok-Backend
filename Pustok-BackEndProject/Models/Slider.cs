using Pustok_BackEndProject.Models.Common;

namespace Pustok_BackEndProject.Models;

public class Slider : BaseModel
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Button { get; set; } = null!;
    public string ImagePath { get; set; } = null!;
}
