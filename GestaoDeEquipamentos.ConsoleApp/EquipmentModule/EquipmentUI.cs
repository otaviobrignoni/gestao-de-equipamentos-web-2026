using GestaoDeEquipamentos.ConsoleApp.ManufacturerModule;
using GestaoDeEquipamentos.ConsoleApp.Shared;
using GestaoDeEquipamentos.ConsoleApp.Shared.BaseModule;

namespace GestaoDeEquipamentos.ConsoleApp.EquipmentModule;

public class EquipmentUI : BaseUI<Equipment>
{
    private readonly ManufacturerUI manufacturerUI;
    public EquipmentUI(ManufacturerUI manufacturerUI, IEquipmentRepo equipmentRepo) : base(equipmentRepo)
    {
        this.manufacturerUI = manufacturerUI;
    }
    public override void Menu()
    {
        string[] options = ["Novo equipamento", "Editar equipamento", "Remover equipamento", "Visualizar equipamentos", "Voltar"];
        while (true)
        {
            switch (Utils.Menu("Menu de Equipamentos", options))
            {
                case 0:
                    if (manufacturerUI.RepoCount < 1)
                    {
                        Utils.MsgBox("Aviso", "Para adicionar equipamentos, primeiro registre um fabricante.");
                    }
                    Add();
                    break;
                case 1:
                    if (RepoCount < 1)
                    {
                        Utils.MsgBox("Aviso", "Nenhum equipamento registrado para editar.");
                        continue;
                    }
                    Edit();
                    break;
                case 2:
                    if (RepoCount < 1)
                    {
                        Utils.MsgBox("Aviso", "Nenhum equipamento registrado para remover.");
                        continue;
                    }
                    Remove();
                    break;
                case 3:
                    if (RepoCount < 1)
                    {
                        Utils.MsgBox("Aviso", "Nenhum equipamento registrado.");
                        continue;
                    }
                    View();
                    break;
                case 4: return;
            }
        }
    }
    public override void Add()
    {
        string title = "Registrar equipamento";
        Equipment newEquipment = new(
            Utils.GetValidString(title, "Nome do equipamento: "),
            Utils.GetValidPrice(title, "Preço de aquisição: "),
            manufacturerUI.Select(),
            Utils.DatePromptBox(title, "Data de fabricação: "));
        newEquipment.Manufacturer.AddEquipment(newEquipment);
        Repository.Add(newEquipment);
        Utils.MsgBox("Info", "✓ Equipamento registrado com sucesso!");
    }
    public override void Edit()
    {
        Equipment equipment = Select();
        Equipment editedEquipment = new(equipment);
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
                    editedEquipment.Manufacturer = manufacturerUI.Select([editedEquipment.Manufacturer]);
                    break;
                case 3:
                    editedEquipment.Date = Utils.DatePromptBox("Editar data", "Data de fabricação: ");
                    break;
                case 4:
                    if (!editedEquipment.Equals(equipment))
                    {
                        Utils.MsgBox("Info", "✓ Equipamento atualizado com sucesso!");
                        equipment.UpdateEntity(editedEquipment);
                    }
                    return;
            }
        }
    }
    public override void Remove()
    {
        Equipment equipment = Select();
        if (equipment.OpenCalls.Count > 0) Utils.MsgBox("Aviso", "Não é possível remover este equipamento porque existem chamados em aberto. Feche-os primeiro.");
        else if (Repository.Remove(equipment.Id)) Utils.MsgBox("Info", "✓ Equipamento removido com sucesso!");
        else Utils.MsgBox("Info", "✗ Erro ao remover o equipamento. Tente novamente.");
    }
    public override void View()
    {
        string[] categories = ["Nome", "Preço", "Fabricante", "Data", "Id"];
        List<string[]> equipments = [];
        foreach (Equipment e in Repository.GetAll())
            equipments.Add([e.Name, $"{e.Price:C2}", e.Manufacturer.Name, $"{e.Date:dd/MM/yyyy}", $"{e.Id}"]);
        Utils.GenerateTable("Equipamentos", categories, equipments.ToArray());
    }
    public override Equipment Select(List<Equipment>? equipments = null)
    {
        var availableEquipments = GetAvailable(equipments);

        string[] options = availableEquipments.Select(e => $"{e.Name} ID: {e.Id}").ToArray();

        return availableEquipments[Utils.Menu("Selecionar equipamento", options)];
    }
}
