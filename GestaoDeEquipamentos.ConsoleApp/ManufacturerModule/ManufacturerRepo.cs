using GestaoDeEquipamentos.ConsoleApp.Shared;
using GestaoDeEquipamentos.ConsoleApp.Shared.BaseModule;

namespace GestaoDeEquipamentos.ConsoleApp.ManufacturerModule;

public class ManufacturerRepo : BaseRepository<Manufacturer>, IManufacturerRepo
{
    public ManufacturerRepo(JsonContext context) : base(context) { }

    public override Dictionary<Guid, Manufacturer> LoadContext()
    {
        return context.Manufacturers;
    }
}
