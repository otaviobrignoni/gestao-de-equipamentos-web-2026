using GestaoDeEquipamentos.ConsoleApp.EquipmentModule;
using GestaoDeEquipamentos.ConsoleApp.Shared.BaseModule;

namespace GestaoDeEquipamentos.ConsoleApp.CallModule;

public class MaintenanceCall : BaseEntity<MaintenanceCall>
{
    public string Title;
    public string Description;
    public Equipment Equipment;
    public DateTime OpeningDate;
    public MaintenanceCall(string title, string description, Equipment equipment, DateTime date)
    {
        Title = title;
        Description = description;
        Equipment = equipment;
        OpeningDate = date;
    }
    public MaintenanceCall(MaintenanceCall maintenanceCall) : this(maintenanceCall.Title, maintenanceCall.Description, maintenanceCall.Equipment, maintenanceCall.OpeningDate) { }

    public string ElapsedTime()
    {
        TimeSpan elapsedTime = DateTime.Now - OpeningDate;

        if (elapsedTime.TotalSeconds < 12)
            return "Agora mesmo";
        else if (elapsedTime.TotalSeconds < 60)
            return $"há {(int)elapsedTime.TotalSeconds} segundo(s)";
        if (elapsedTime.TotalMinutes < 60)
            return $"há {(int)elapsedTime.TotalMinutes} minuto(s)";
        if (elapsedTime.TotalHours < 24)
            return $"há {(int)elapsedTime.TotalHours} hora(s)";
        if (elapsedTime.TotalDays < 30)
            return $"há {(int)elapsedTime.TotalDays} dia(s)";
        if (elapsedTime.TotalDays < 365)
            return $"há {(int)(elapsedTime.TotalDays / 30)} mês(es)";

        return $"há {(int)(elapsedTime.TotalDays / 365)} ano(s)";
    }
    public override void UpdateEntity(MaintenanceCall updatedCall)
    {
        if (Equipment != updatedCall.Equipment)
        {
            Equipment.RemoveCall(Id);
            updatedCall.Equipment.AddCall(this);
            Equipment = updatedCall.Equipment;
        }
        Title = updatedCall.Title;
        Description = updatedCall.Description;
        OpeningDate = updatedCall.OpeningDate;
    }
    public override bool Equals(MaintenanceCall maintenanceCall)
    {
        if (maintenanceCall is null) return false;
        if (maintenanceCall.Title != Title
            || maintenanceCall.Description != Description
            || maintenanceCall.Equipment != Equipment
            || maintenanceCall.OpeningDate != OpeningDate)
            return false;
        return true;

    }
}
