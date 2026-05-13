using GestaoDeEquipamentos.ConsoleApp.EquipmentModule;
using GestaoDeEquipamentos.ConsoleApp.Shared.BaseModule;

namespace GestaoDeEquipamentos.ConsoleApp.ManufacturerModule;

public class Manufacturer : BaseEntity<Manufacturer>
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public List<Equipment> Equipments { get; } = [];

    public Manufacturer() { }
    public Manufacturer(string name, string email, string phoneNumber)
    {
        Name = name;
        Email = email;
        PhoneNumber = phoneNumber;
    }
    public Manufacturer(Manufacturer manufacturer) : this(manufacturer.Name, manufacturer.Email, manufacturer.PhoneNumber) { }

    public override void UpdateEntity(Manufacturer updatedManufacturer)
    {
        Name = updatedManufacturer.Name;
        Email = updatedManufacturer.Email;
        PhoneNumber = updatedManufacturer.PhoneNumber;
    }

    public void AddEquipment(Equipment equipment)
    {
        Equipments.Add(equipment);
    }
    public bool RemoveEquipment(Equipment? equipment)
    {
        if (equipment is null) return false;
        return Equipments.Remove(equipment);
    }
    public override bool Equals(Manufacturer manufacturer)
    {
        if (manufacturer is null) return false;
        if (manufacturer.Name != Name
           || manufacturer.PhoneNumber != PhoneNumber
           || manufacturer.Email != Email)
            return false;
        return true;
    }
}
