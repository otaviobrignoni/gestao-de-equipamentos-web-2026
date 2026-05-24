using System.Text.Json.Serialization;
using GestaoDeEquipamentos.ConsoleApp.EquipmentModule;
using GestaoDeEquipamentos.ConsoleApp.Shared.BaseModule;

namespace GestaoDeEquipamentos.ConsoleApp.CallModule;

public class MaintenanceCall : BaseEntity<MaintenanceCall>
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Equipment Equipment { get; set; } = null!;
    public DateTime OpeningDate { get; set; } = DateTime.Now;
    public bool IsDone { get; set; }

    public MaintenanceCall() { }
    public MaintenanceCall(string title, Equipment equipment, bool isDone, string? description = null)
    {
        Title = title;
        Description = description;
        Equipment = equipment;
        IsDone = isDone;
    }
    public MaintenanceCall(MaintenanceCall maintenanceCall) : this(maintenanceCall.Title, maintenanceCall.Equipment, maintenanceCall.IsDone, maintenanceCall.Description) { }

    [JsonIgnore]
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

    [JsonIgnore]
    public int IntElapsedTime => (DateTime.Now - OpeningDate).Days;

    public void Complete()
    {
        IsDone = true;
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
        IsDone = updatedCall.IsDone;
    }
    public override bool Equals(MaintenanceCall maintenanceCall)
    {
        if (maintenanceCall is null) return false;
        if (maintenanceCall.Title != Title
            || maintenanceCall.Description != Description
            || maintenanceCall.Equipment != Equipment
            || maintenanceCall.OpeningDate != OpeningDate
            || maintenanceCall.IsDone != IsDone)
            return false;
        return true;

    }
}
