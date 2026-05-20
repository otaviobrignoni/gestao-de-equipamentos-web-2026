using GestaoDeEquipamentos.ConsoleApp.ManufacturerModule;
using GestaoDeEquipamentos.ConsoleApp.Shared;
using Microsoft.AspNetCore.Mvc;
using GestaoDeEquipamentos.ConsoleApp.Models;

namespace GestaoDeEquipamentos.ConsoleApp.Controllers
{
    public class ManufacturerController : Controller
    {
        private readonly ManufacturerRepo manufacturerRepo;
        public ManufacturerController()
        {
            JsonContext context = new();
            context.Load();

            manufacturerRepo = new ManufacturerRepo(context);
        }

        [HttpGet]
        public ActionResult Index()
        {
            var vms = manufacturerRepo.Entities.Select(m => new ManufacturerViewModel(m.Name, m.Email, m.PhoneNumber, m.Id));

            return View(vms);
        }

        [HttpGet]
        public ActionResult Add()
        {
            ManufacturerViewModel vm = new(string.Empty, string.Empty, string.Empty);
            return View(vm);
        }

        [HttpPost]
        public ActionResult Add(ManufacturerViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);
                
            Manufacturer m = new(vm.Name, vm.Email, vm.PhoneNumber);

            manufacturerRepo.Add(m);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            Manufacturer? m = manufacturerRepo.GetById(id);

            if (m is null)
                return RedirectToAction(nameof(Index));

            ManufacturerViewModel vm = new(m.Name, m.Email, m.PhoneNumber, m.Id);

            return View(vm);
        }

        [HttpPost]
        public ActionResult Edit(ManufacturerViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);
            
            Manufacturer m = new(vm.Name, vm.Email, vm.PhoneNumber);

            manufacturerRepo.Edit(vm.Id, m);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public ActionResult Remove(Guid id)
        {
            Manufacturer? m = manufacturerRepo.GetById(id);

            if (m is null)
                return RedirectToAction(nameof(Index));

            ManufacturerViewModel vm = new(m.Name, m.Email, m.PhoneNumber, m.Id);

            return View(vm);
        }

        [HttpPost]
        public ActionResult Remove(ManufacturerViewModel vm)
        {       
            manufacturerRepo.Remove(vm.Id);

            return RedirectToAction(nameof(Index));
        }
    }
}
