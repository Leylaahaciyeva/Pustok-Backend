using Pustok_BackEndProject.Models;

namespace Pustok_BackEndProject.ViewModels;

public class HomeVM
{
    public List<Slider> Sliders { get; set; } = new();
    public List<Service> Services { get; set; } = new();
    public List<Product> NewProducts { get; set; } = new();
    public List<Product> DiscountedProducts{ get; set; } = new();
    public List<Product> UndiscountedProducts{ get; set; } = new();
    public List<Product> ChildrenProducts{ get; set; } = new();
    public List<Product> TwentyDiscountProducts{ get; set; } = new();

}
