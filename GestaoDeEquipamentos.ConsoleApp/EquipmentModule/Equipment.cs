using GestaoDeEquipamentos.ConsoleApp.CallModule;
using GestaoDeEquipamentos.ConsoleApp.ManufacturerModule;
using GestaoDeEquipamentos.ConsoleApp.Shared.BaseModule;

namespace GestaoDeEquipamentos.ConsoleApp.EquipmentModule;

public class Equipment : BaseEntity<Equipment>
{
    public string Name;
    public decimal Price;
    public Manufacturer Manufacturer;
    public DateOnly Date;
    public List<MaintenanceCall> OpenCalls;

    public Equipment()
    {
        Name = string.Empty;
        OpenCalls = [];
    }
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
            Manufacturer.RemoveEquipment(Id);
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

    public bool RemoveCall(Guid callId)
    {
        MaintenanceCall? call = OpenCalls.FirstOrDefault(mc => mc.Id == callId);
        if (call is null) return false;
        OpenCalls.Remove(call);
        return true;
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
