using GestaoDeEquipamentos.ConsoleApp.EquipmentModule;
using GestaoDeEquipamentos.ConsoleApp.Shared.BaseModule;

namespace GestaoDeEquipamentos.ConsoleApp.ManufacturerModule;

public class Manufacturer : BaseEntity<Manufacturer>
{
    public string Name;
    public string Email;
    public string PhoneNumber;
    public List<Equipment> Equipments;

    public Manufacturer() : this(string.Empty, string.Empty, string.Empty)
    {
    }
    public Manufacturer(string name, string email, string phoneNumber)
    {
        Name = name;
        Email = email;
        PhoneNumber = phoneNumber;
        Equipments = [];
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
    public bool RemoveEquipment(Guid equipmentId)
    {
        Equipment? equipment = Equipments.FirstOrDefault(e => e.Id == equipmentId);
        if (equipment is null) return false;
        Equipments.Remove(equipment);
        return true;
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
