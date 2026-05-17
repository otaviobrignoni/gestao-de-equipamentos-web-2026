using System.Text.Json;
using System.Text.Json.Serialization;
using GestaoDeEquipamentos.ConsoleApp.CallModule;
using GestaoDeEquipamentos.ConsoleApp.EquipmentModule;
using GestaoDeEquipamentos.ConsoleApp.ManufacturerModule;

namespace GestaoDeEquipamentos.ConsoleApp.Shared;

public class JsonContext
{
    public Dictionary<Guid, Manufacturer> Manufacturers { get; private set; } = [];
    public Dictionary<Guid, Equipment> Equipments { get; private set; } = [];
    public Dictionary<Guid, MaintenanceCall> Calls { get; private set; } = [];
    private readonly string FilePath;
    private readonly JsonSerializerOptions options;
    public JsonContext()
    {
        string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        string folderPath = Path.Combine(appDataPath, "GestaoDeEquipamentos");

        Directory.CreateDirectory(folderPath);
        FilePath = Path.Combine(folderPath, "savedData.json");
        options = new()
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            IncludeFields = true,
            ReferenceHandler = ReferenceHandler.Preserve
        };
    }
    public void Save()
    {
        string jsonString = JsonSerializer.Serialize(this, options);
        File.WriteAllText(FilePath, jsonString);
    }
    public void Load()
    {
        if (!File.Exists(FilePath)) return;

        string jsonString = File.ReadAllText(FilePath);

        JsonContext? context = JsonSerializer.Deserialize<JsonContext>(jsonString, options);

        if (context is null) return;

        Manufacturers = context.Manufacturers;
        Equipments = context.Equipments;
        Calls = context.Calls;
    }
}
