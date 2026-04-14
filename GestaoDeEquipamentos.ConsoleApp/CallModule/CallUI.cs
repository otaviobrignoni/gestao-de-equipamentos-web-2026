using GestaoDeEquipamentos.ConsoleApp.EquipmentModule;
using GestaoDeEquipamentos.ConsoleApp.Shared;
using GestaoDeEquipamentos.ConsoleApp.Shared.BaseModule;

namespace GestaoDeEquipamentos.ConsoleApp.CallModule;

public class CallUI : BaseUI<MaintenanceCall>
{
    private readonly EquipmentUI EquipmentUI;
    public CallUI(EquipmentUI equipmentUI, ICallRepo callRepo) : base(callRepo)
    {
        EquipmentUI = equipmentUI;
    }
    public override void Menu()
    {
        string[] options = ["Novo chamado", "Editar chamado", "Remover chamado", "Visualizar chamados", "Voltar"];
        while (true)
        {
            switch (Utils.Menu("Menu de Chamados de Manutenção", options))
            {
                case 0:
                    if (EquipmentUI.RepoCount < 1)
                    {
                        Utils.MsgBox("Aviso", "Nenhum equipamento existe para fazer um chamado");
                        continue;
                    }
                    Create();
                    break;
                case 1:
                    if (RepoCount < 1)
                    {
                        Utils.MsgBox("Aviso", "Nenhum chamado existe para editar");
                        continue;
                    }
                    Edit();
                    break;
                case 2:
                    if (RepoCount < 1)
                    {
                        Utils.MsgBox("Aviso", "Nenhum chamado existe para remover");
                        continue;
                    }
                    Remove();
                    break;
                case 3:
                    if (RepoCount < 1)
                    {
                        Utils.MsgBox("Aviso", "Nenhum chamado existe para visualizar");
                        continue;
                    }
                    View();
                    break;
                case 4: return;
            }
        }
    }
    public override void Create()
    {
        string title = "Abrir chamado";
        MaintenanceCall newCall = new(
            Utils.GetValidString(title, "Título do chamado: "),
            Utils.GetValidString(title, "Descrição do chamado: "),
            EquipmentUI.Select(),
            DateTime.Now);
        newCall.Equipment.AddCall(newCall);
        Repository.Add(newCall);
        Utils.MsgBox("Info", "Chamado de manutenção aberto com sucesso");
    }
    public override void Edit()
    {
        MaintenanceCall maintenanceCall = Select();
        MaintenanceCall editedCall = new(maintenanceCall.Title, maintenanceCall.Description, maintenanceCall.Equipment, maintenanceCall.OpeningDate);
        string[] options = ["Título", "Descrição", "Equipamento", "Voltar"];

        while (true)
        {
            switch (Utils.Menu("Editar chamado", options))
            {
                case 0:
                    editedCall.Title = Utils.GetValidString("Editar título", "Título do chamado: ");
                    break;
                case 1:
                    editedCall.Description = Utils.GetValidString("Editar descrição", "Descrição do chamado: ");
                    break;
                case 2:
                    editedCall.Equipment = EquipmentUI.Select([editedCall.Equipment]);
                    break;
                case 3:
                    if (!editedCall.Equals(maintenanceCall))
                    {
                        Utils.MsgBox("Info", "Chamado editado com sucesso");
                        maintenanceCall.UpdateEntity(editedCall);
                    }
                    return;
            }
        }
    }
    public override void Remove()
    {
        MaintenanceCall call = Select();
        call.Equipment.RemoveCall(call.Id);
        if (Repository.Remove(call.Id)) Utils.MsgBox("Info", "Chamado removido com sucesso");
        else Utils.MsgBox("Info", "Ocorreu um erro na remoção do chamado");
    }
    public override void View()
    {
        string[] categories = ["Título", "Equipamento", "Data de Abertura", "Aberto", "Id"];
        List<string[]> calls = [];
        foreach (MaintenanceCall c in Repository.GetAll())
            calls.Add([c.Title, c.Equipment.Name, $"{c.OpeningDate:dd/MM/yyyy HH:mm}", c.ElapsedTime(), $"{c.Id}"]);
        Utils.GenerateTable("Chamados de Manutenção", categories, calls.ToArray());
    }
    public override MaintenanceCall Select(List<MaintenanceCall>? calls = null)
    {
        var availableCalls = GetAvailable(calls);

        string[] options = availableCalls.Select(c => $"{c.Title} : {c.Equipment.Name}, ID: {c.Id}").ToArray();

        return availableCalls[Utils.Menu("Selecionar chamado", options)];
    }
}
