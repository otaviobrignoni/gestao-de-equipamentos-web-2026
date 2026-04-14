using GestaoDeEquipamentos.ConsoleApp.Shared;
using GestaoDeEquipamentos.ConsoleApp.Shared.BaseModule;

namespace GestaoDeEquipamentos.ConsoleApp.EquipmentModule;

public class EquipmentUI : BaseUI<Equipment>
{
    public EquipmentUI(IEquipmentRepo equipmentRepo) : base(equipmentRepo) { }
    public override void Menu()
    {
        string[] options = ["Novo equipamento", "Editar equipamento", "Remover equipamento", "Visualizar equipamentos", "Voltar"];
        while (true)
        {
            switch (Utils.Menu("Menu de Equipamentos", options))
            {
                case 0:
                    Create();
                    break;
                case 1:
                    if (RepoCount < 1)
                    {
                        Utils.MsgBox("Aviso", "Nenhum equipamento existe para editar");
                        continue;
                    }
                    Edit();
                    break;
                case 2:
                    if (RepoCount < 1)
                    {
                        Utils.MsgBox("Aviso", "Nenhum equipamento existe para remover");
                        continue;
                    }
                    Remove();
                    break;
                case 3:
                    if (RepoCount < 1)
                    {
                        Utils.MsgBox("Aviso", "Nenhum equipamento existe para visualizar");
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
        string title = "Criar equipamento";
        Repository.Add(new Equipment(
            Utils.GetValidString(title, "Nome do equipamento: "),
            Utils.GetValidPrice(title, "Preço de aquisição: "),
            Utils.GetValidString(title, "Nome do fabricante: "),
            Utils.DatePromptBox(title, "Data de fabricação: ")));
        Utils.MsgBox("Info", "Equipamento criado com sucesso");
    }
    public override void Edit()
    {
        Equipment equipment = Select();
        Equipment editedEquipment = new Equipment(equipment.Name, equipment.Price, equipment.Manufacturer, equipment.Date);
        string[] options = ["Nome", "Preço de aquisição", "Nome do fabricante", "Data de fabricação", "Voltar"];

        while (true)
        {
            switch (Utils.Menu("Editar equipamento", options))
            {
                case 0:
                    editedEquipment.Name = Utils.GetValidString("Editar nome", "Nome do equipamento: ");
                    break;
                case 1:
                    editedEquipment.Price = Utils.GetValidPrice("Editar preço", "Preço do equipamento: ");
                    break;
                case 2:
                    editedEquipment.Manufacturer = Utils.GetValidString("Editar fabricante", "Nome do fabricante: ");
                    break;
                case 3:
                    editedEquipment.Date = Utils.DatePromptBox("Editar data", "Data de fabricação: ");
                    break;
                case 4:
                    if (!editedEquipment.Equals(equipment))
                    {
                        Utils.MsgBox("Info", "Equipamento editado com sucesso");
                        equipment.UpdateEntity(editedEquipment);
                    }
                    return;
            }
        }
    }
    public override void Remove()
    {
        Equipment equipment = Select();
        if (equipment.OpenCalls.Count > 0) Utils.MsgBox("Aviso", "Não é possível remover um equipamento com chamado aberto");
        else if (Repository.Remove(equipment.Id)) Utils.MsgBox("Info", "Equipamento removido com sucesso");
        else Utils.MsgBox("Info", "Ocorreu um erro na remoção do equipamento");
    }
    public override void View()
    {
        string[] categories = ["Nome", "Preço", "Fabricante", "Data", "Id"];
        List<string[]> equipments = [];
        foreach (Equipment e in Repository.GetAll())
            equipments.Add([e.Name, $"{e.Price:C2}", e.Manufacturer, $"{e.Date:dd/MM/yyyy}", $"{e.Id}"]);
        Utils.GenerateTable("Equipamentos", categories, equipments.ToArray());
    }
    public override Equipment Select(List<Equipment>? equipments = null)
    {
        var availableEquipments = GetAvailable(equipments);

        string[] options = availableEquipments.Select(e => $"{e.Name} ID: {e.Id}").ToArray();

        return availableEquipments[Utils.Menu("Selecionar equipamento", options)];
    }
}
