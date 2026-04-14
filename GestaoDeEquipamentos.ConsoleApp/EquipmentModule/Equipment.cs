using GestaoDeEquipamentos.ConsoleApp.CallModule;
using GestaoDeEquipamentos.ConsoleApp.Shared.BaseModule;

namespace GestaoDeEquipamentos.ConsoleApp.EquipmentModule;

public class Equipment : BaseEntity<Equipment>
{
    public string Name;
    public decimal Price;
    public string Manufacturer;
    public DateOnly Date;
    public List<MaintenanceCall> OpenCalls;

    public Equipment()
    {
        Name = string.Empty;
        Manufacturer = string.Empty;
        OpenCalls = [];
    }
    public Equipment(string name, decimal price, string manufacturer, DateOnly date) : this()
    {
        Name = name;
        Price = price;
        Manufacturer = manufacturer;
        Date = date;
    }
    public override void UpdateEntity(Equipment updatedEntity)
    {
        Name = updatedEntity.Name;
        Price = updatedEntity.Price;
        Manufacturer = updatedEntity.Manufacturer;
        Date = updatedEntity.Date;
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
