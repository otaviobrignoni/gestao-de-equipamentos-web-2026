using GestaoDeEquipamentos.ConsoleApp.EquipmentModule;
using GestaoDeEquipamentos.ConsoleApp.Shared.BaseModule;

namespace GestaoDeEquipamentos.ConsoleApp.CallModule;

public class MaintenanceCall : BaseEntity<MaintenanceCall>
{
    public string Title;
    public string Description;
    public Equipment Equipment;
    public DateOnly OpeningDate;

    public MaintenanceCall(string title, string description, Equipment equipment, DateOnly date)
    {
        Title = title;
        Description = description;
        Equipment = equipment;
        OpeningDate = date;
    }
    public override void UpdateEntity(MaintenanceCall updatedEntity)
    {
        Title = updatedEntity.Title;
        Description = updatedEntity.Description;
        Equipment = updatedEntity.Equipment;
        OpeningDate = updatedEntity.OpeningDate;
    }
}
