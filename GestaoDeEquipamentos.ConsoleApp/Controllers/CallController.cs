using GestaoDeEquipamentos.ConsoleApp.CallModule;
using GestaoDeEquipamentos.ConsoleApp.EquipmentModule;
using GestaoDeEquipamentos.ConsoleApp.Models;
using GestaoDeEquipamentos.ConsoleApp.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GestaoDeEquipamentos.ConsoleApp.Controllers;

public class CallController : Controller
{
    private readonly CallRepo callRepo;
    private readonly EquipmentRepo equipmentRepo;

    public CallController()
    {
        JsonContext context = new();
        context.Load();
        callRepo = new(context);
        equipmentRepo = new(context);
    }

    [HttpGet]
    public ActionResult Index(string? status)
    {
        string? selectedStatus = status?.ToLower();

        IEnumerable<MaintenanceCall> calls;

        if (selectedStatus == "em-aberto")
            calls = callRepo.Where(c => !c.IsDone);

        else if (selectedStatus == "concluidos")
            calls = callRepo.Where(c => c.IsDone);

        else
            calls = callRepo.Entities;

        var vms = calls.Select(c => new CallShowViewModel(c.Title, c.Description, c.Equipment.Name, c.OpeningDate, c.IntElapsedTime, c.IsDone));

        ViewBag.StatusSelecionado = selectedStatus;

        return View(vms);
    }

    [HttpGet]
    public ActionResult Add()
    {
        ViewBag.Equipments = LoadEquipments();

        CallViewModel vm = new(string.Empty, null, Guid.Empty);

        return View(vm);
    }

    [HttpPost]
    public ActionResult Add(CallViewModel vm)
    {
        Equipment? e = equipmentRepo.GetById(vm.EquipmentId);

        if (e is null || vm.EquipmentId == Guid.Empty)
            ModelState.AddModelError(nameof(vm.EquipmentId), "Selecione um equipamento válido.");

        if (!ModelState.IsValid)
        {
            ViewBag.Equipments = LoadEquipments();
            return View(vm);
        }

        MaintenanceCall mc = new(vm.Title, e!, vm.IsDone, vm.Description);

        callRepo.Add(mc);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public ActionResult Edit(Guid id)
    {
        MaintenanceCall? mc = callRepo.GetById(id);

        if (mc is null)
            return RedirectToAction(nameof(Index));

        CallViewModel vm = new(mc.Title, mc.Description, mc.Equipment.Id, mc.IsDone, mc.Id);

        ViewBag.Equipments = LoadEquipments();

        return View(vm);
    }

    [HttpPost]
    public ActionResult Edit(CallViewModel vm)
    {
        Equipment? e = equipmentRepo.GetById(vm.EquipmentId);

        if (e is null || vm.EquipmentId == Guid.Empty)
            ModelState.AddModelError(nameof(vm.EquipmentId), "Selecione um equipamento válido.");

        if (!ModelState.IsValid)
        {
            ViewBag.Equipments = LoadEquipments();
            return View(vm);
        }

        MaintenanceCall edited = new(vm.Title, e!, vm.IsDone, vm.Description);

        callRepo.Edit(vm.Id, edited);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public ActionResult Remove(Guid id)
    {
        MaintenanceCall? mc = callRepo.GetById(id);

        if (mc is null)
            return RedirectToAction(nameof(Index));

        CallShowViewModel vm = new(mc.Title, mc.Description, mc.Equipment.Name, mc.OpeningDate, mc.IntElapsedTime, mc.IsDone);

        ViewBag.Equipments = LoadEquipments();

        return View(vm);
    }

    [HttpPost]
    public ActionResult Remove(CallShowViewModel vm)
    {
        callRepo.Remove(vm.Id);

        return RedirectToAction(nameof(Index));
    }

    private List<SelectListItem> LoadEquipments()
    {
        return equipmentRepo.Entities.Select(e => new SelectListItem(e.Name, e.Id.ToString())).ToList();
    }
}
