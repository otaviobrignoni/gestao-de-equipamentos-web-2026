using GestaoDeEquipamentos.ConsoleApp.Shared;
using GestaoDeEquipamentos.ConsoleApp.Shared.BaseModule;

namespace GestaoDeEquipamentos.ConsoleApp.CallModule;

public class CallRepo : BaseRepository<MaintenanceCall>, ICallRepo
{
    public CallRepo(JsonContext context) : base(context) { }

    public override Dictionary<Guid, MaintenanceCall> LoadContext()
    {
        return context.Calls;
    }
}

