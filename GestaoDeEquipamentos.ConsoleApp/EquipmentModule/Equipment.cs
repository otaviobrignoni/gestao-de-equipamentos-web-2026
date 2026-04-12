using System.Reflection.Metadata;
using GestaoDeEquipamentos.ConsoleApp.Shared.BaseModule;

namespace GestaoDeEquipamentos.ConsoleApp.EquipmentModule;

public class Equipment : BaseEntity<Equipment>
{
    public string Name;
    public decimal Price;
    public string Manufacturer;
    public DateOnly Date;

    public Equipment(string name, decimal price, string manufacturer, DateOnly date)
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
