using GestaoDeEquipamentos.ConsoleApp.EquipmentModule;
using GestaoDeEquipamentos.ConsoleApp.ManufacturerModule;
using GestaoDeEquipamentos.ConsoleApp.Shared;
using Microsoft.AspNetCore.Mvc;
using GestaoDeEquipamentos.ConsoleApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GestaoDeEquipamentos.ConsoleApp.Controllers;

public class EquipmentController : Controller
{
    private readonly EquipmentRepo equipmentRepo;
    private readonly ManufacturerRepo manufacturerRepo;

    public EquipmentController()
    {
        JsonContext context = new();
        context.Load();
        equipmentRepo = new(context);
        manufacturerRepo = new(context);
    }

    [HttpGet]
    public ActionResult Index()
    {
        var vms = equipmentRepo.Entities.Select(e => new EquipmentShowViewModel(e.Name, e.Price, e.Date, e.Manufacturer.Name, e.Id));

        return View(vms);
    }

    [HttpGet]
    public ActionResult Add()
    {
        ViewBag.Manufacturers = LoadManufacturers();
        EquipmentViewModel vm = new(string.Empty, 0, DateOnly.FromDateTime(DateTime.Today), Guid.Empty);
        return View(vm);
    }

    [HttpPost]
    public ActionResult Add(EquipmentViewModel vm)
    {
        Manufacturer? m = manufacturerRepo.GetById(vm.ManufacturerId);

        if (m is null || vm.ManufacturerId == Guid.Empty)
            ModelState.AddModelError(nameof(vm.ManufacturerId), "Selecione um fabricante válido.");

        if (!ModelState.IsValid)
        {
            ViewBag.Manufacturers = LoadManufacturers();
            return View(vm);
        }

        Equipment e = new(vm.Name, vm.Price, vm.Date, m!);

        equipmentRepo.Add(e);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public ActionResult Edit(Guid id)
    {
        Equipment? e = equipmentRepo.GetById(id);

        if (e is null)
            return RedirectToAction(nameof(Index));

        EquipmentViewModel vm = new(e.Name, e.Price, e.Date, e.Manufacturer.Id, e.Id);

        ViewBag.Manufacturers = LoadManufacturers();

        return View(vm);
    }

    [HttpPost]
    public ActionResult Edit(EquipmentViewModel vm)
    {
        Manufacturer? m = manufacturerRepo.GetById(vm.ManufacturerId);

        if (m is null || vm.ManufacturerId == Guid.Empty)
            ModelState.AddModelError(nameof(vm.ManufacturerId), "Selecione um fabricante válido.");

        if (!ModelState.IsValid)
        {
            ViewBag.Manufacturers = LoadManufacturers();
            return View(vm);
        }

        Equipment edited = new(vm.Name, vm.Price, vm.Date, m!);

        equipmentRepo.Edit(vm.Id, edited);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public ActionResult Remove(Guid id)
    {
        Equipment? e = equipmentRepo.GetById(id);

        if (e is null)
            return RedirectToAction(nameof(Index));

        EquipmentShowViewModel vm = new(e.Name, e.Price, e.Date, e.Manufacturer.Name, e.Id);

        ViewBag.Manufacturers = LoadManufacturers();

        return View(vm);
    }

    [HttpPost]
    public ActionResult Remove(EquipmentViewModel vm)
    {
        equipmentRepo.Remove(vm.Id);

        return RedirectToAction(nameof(Index));
    }

    private List<SelectListItem> LoadManufacturers()
    {
        return manufacturerRepo.Entities.Select(m => new SelectListItem(m.Name, m.Id.ToString())).ToList();
    }
}