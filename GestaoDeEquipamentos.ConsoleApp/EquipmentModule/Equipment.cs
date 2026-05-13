using GestaoDeEquipamentos.ConsoleApp.CallModule;
using GestaoDeEquipamentos.ConsoleApp.ManufacturerModule;
using GestaoDeEquipamentos.ConsoleApp.Shared.BaseModule;

namespace GestaoDeEquipamentos.ConsoleApp.EquipmentModule;

public class Equipment : BaseEntity<Equipment>
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public Manufacturer Manufacturer { get; set; } = null!;
    public DateOnly Date { get; set; }
    public List<MaintenanceCall> OpenCalls { get; } = [];

    public Equipment() { }
    public Equipment(string name, decimal price, Manufacturer manufacturer, DateOnly date) : this()
    {
        Name = name;
        Price = price;
        Manufacturer = manufacturer;
        Date = date;
    }
    public Equipment(Equipment equipment) : this(equipment.Name, equipment.Price, equipment.Manufacturer, equipment.Date) { }
    public override void UpdateEntity(Equipment updatedEquipment)
    {
        if (Manufacturer != updatedEquipment.Manufacturer)
        {
            Manufacturer.RemoveEquipment(this);
            updatedEquipment.Manufacturer.AddEquipment(this);
            Manufacturer = updatedEquipment.Manufacturer;
        }
        Name = updatedEquipment.Name;
        Price = updatedEquipment.Price;
        Date = updatedEquipment.Date;
    }
    public void AddCall(MaintenanceCall maintenanceCall)
    {
        OpenCalls.Add(maintenanceCall);
    }

    public bool RemoveCall(MaintenanceCall? call)
    {
        if (call is null) return false;
        return OpenCalls.Remove(call);
    }

    public override bool Equals(Equipment equipment)
    {
        if (equipment is null) return false;
        if (equipment.Name != Name
            || equipment.Price != Price
            || equipment.Manufacturer != Manufacturer
            || equipment.Date != Date)
            return false;
        return true;
    }
}
