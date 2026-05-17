using GestaoDeEquipamentos.ConsoleApp.Shared;
using GestaoDeEquipamentos.ConsoleApp.Shared.BaseModule;

namespace GestaoDeEquipamentos.ConsoleApp.EquipmentModule;

public class EquipmentRepo : BaseRepository<Equipment>, IEquipmentRepo
{
    public EquipmentRepo(JsonContext context) : base(context) { }

    public override Dictionary<Guid, Equipment> LoadContext()
    {
        return context.Equipments;
    }
}


