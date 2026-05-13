using GestaoDeEquipamentos.ConsoleApp.EquipmentModule;
using GestaoDeEquipamentos.ConsoleApp.Shared.BaseModule;

namespace GestaoDeEquipamentos.ConsoleApp.CallModule;

public class MaintenanceCall : BaseEntity<MaintenanceCall>
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Equipment Equipment { get; set; } = null!;
    public DateTime OpeningDate { get; set; }

    public MaintenanceCall() { }
    public MaintenanceCall(string title, string description, Equipment equipment, DateTime date)
    {
        Title = title;
        Description = description;
        Equipment = equipment;
        OpeningDate = date;
    }
    public MaintenanceCall(MaintenanceCall maintenanceCall) : this(maintenanceCall.Title, maintenanceCall.Description, maintenanceCall.Equipment, maintenanceCall.OpeningDate) { }

    public string ElapsedTime
    {
        get
        {
            double elapsedSeconds = (DateTime.Now - OpeningDate).TotalSeconds;

            return elapsedSeconds switch
            {
                < 12 => "Agora mesmo",
                < 60 => $"há {elapsedSeconds:F0} segundo(s)",
                < 3600 => $"há {elapsedSeconds / 60:F0} minuto(s)", // 60 seconds in a minute
                < 86400 => $"há {elapsedSeconds / 3600:F0} hora(s)", // 3600 seconds in an hour
                < 2629800 => $"há {elapsedSeconds / 86400:F0} dia(s)", // 86400 seconds in a day
                < 31557600 => $"há {elapsedSeconds / 2629800:F0} mês(es)", // 2629800 seconds in a month (30.44 days)
                _ => $"há {elapsedSeconds / 31557600:F0} ano(s)" // 31557600 seconds in a year (365.25 days)
            };
        }
    }
    public override void UpdateEntity(MaintenanceCall updatedCall)
    {
        if (Equipment != updatedCall.Equipment)
        {
            Equipment.RemoveCall(this);
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
