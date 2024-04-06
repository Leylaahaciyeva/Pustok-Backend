using Pustok_BackEndProject.Models.Common;

namespace Pustok_BackEndProject.Models;

public partial class Service : BaseEntity
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Icon { get; set; } = null!;
}


