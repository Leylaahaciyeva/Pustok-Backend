using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok_BackEndProject.Areas.Admin.ViewModels;
using Pustok_BackEndProject.Contexts;
using Pustok_BackEndProject.Extensions;
using Pustok_BackEndProject.Models;

namespace Pustok_BackEndProject.Areas.Admin.Controllers;
[Area("Admin")]
public class SliderController : Controller
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _environment;

    public SliderController(AppDbContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }

    public async Task<IActionResult> Index()
    {
        var sliders = await _context.Sliders.ToListAsync();
        return View(sliders);
    }

    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(SliderCreateVM vm)
    {
        if (!ModelState.IsValid)
            return View(vm);

        if (!vm.Image.ValidateType("image"))
        {
            ModelState.AddModelError("Image", "Invalid file type");
            return View(vm);
        }
        if (!vm.Image.ValidateSize(2))
        {
            ModelState.AddModelError("Image", "Max file size-2mb");
            return View(vm);
        }

        string filename = await vm.Image.FileCreateAsync(_environment.WebRootPath, "image");

        Slider slider = new()
        {
            Title = vm.Title,
            Button = vm.Button,
            Description = vm.Description,
            ImagePath = filename
        };

        await _context.Sliders.AddAsync(slider);
        await _context.SaveChangesAsync();


        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Update(int id)
    {
        var slider = await _context.Sliders.FirstOrDefaultAsync(x => x.Id == id);
        if (slider is null)
            return NotFound();

        SliderUpdateVM vm = new()
        {
            Title = slider.Title,
            Button = slider.Button,
            Description = slider.Description,
            ImagePath = slider.ImagePath,
        };

        return View(vm);
    }
    public async Task<IActionResult> Delete(int id)
    {
        var slider = await _context.Sliders.FirstOrDefaultAsync(x => x.Id == id);
        if (slider is null)
            return NotFound();


        slider.ImagePath.FileDelete(_environment.WebRootPath, "Image");
        _context.Sliders.Remove(slider);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Update(int id, SliderUpdateVM vm)
    {
        if (!ModelState.IsValid)
            return View(vm);


        var existSlider = await _context.Sliders.FirstOrDefaultAsync(x => x.Id == id);

        if (existSlider is null)
            return NotFound();


        if (vm.Image is not null)
        {

            if (!vm.Image.ValidateType("image"))
            {
                ModelState.AddModelError("Image", "Invalid file type");
                return View(vm);
            }
            if (!vm.Image.ValidateSize(2))
            {
                ModelState.AddModelError("Image", "Max file size-2mb");
                return View(vm);
            }

            string filename = await vm.Image.FileCreateAsync(_environment.WebRootPath, "image");

            existSlider.ImagePath.FileDelete(_environment.WebRootPath, "image");
            existSlider.ImagePath = filename;
        }

        existSlider.Title = vm.Title;
        existSlider.Description = vm.Description;
        existSlider.Button = vm.Button;


        _context.Sliders.Update(existSlider);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");

    }
}
