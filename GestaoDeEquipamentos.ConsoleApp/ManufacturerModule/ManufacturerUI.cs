using GestaoDeEquipamentos.ConsoleApp.Shared;
using GestaoDeEquipamentos.ConsoleApp.Shared.BaseModule;

namespace GestaoDeEquipamentos.ConsoleApp.ManufacturerModule;

public class ManufacturerUI : BaseUI<Manufacturer>
{
    public ManufacturerUI(IManufacturerRepo manufacturerRepo) : base(manufacturerRepo) { }

    public override void Menu()
    {
        string[] options = ["Novo fabricante", "Editar fabricante", "Remover fabricante", "Visualizar fabricantes", "Voltar"];
        while (true)
        {
            switch (Utils.Menu("Menu de fabricantes", options))
            {
                case 0:
                    Add();
                    break;
                case 1:
                    if (RepoCount < 1)
                    {
                        Utils.MsgBox("Aviso", "Nenhum fabricante registrado para editar.");
                        continue;
                    }
                    Edit();
                    break;
                case 2:
                    if (RepoCount < 1)
                    {
                        Utils.MsgBox("Aviso", "Nenhum fabricante registrado para remover.");
                        continue;
                    }
                    Remove();
                    break;
                case 3:
                    if (RepoCount < 1)
                    {
                        Utils.MsgBox("Aviso", "Nenhum fabricante registrado.");
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
        string title = "Registrar fabricante";
        Repository.Add(new Manufacturer(
            Utils.GetValidString(title, "Nome do fabricante: "),
            Utils.GetValidEmail(title, "Email do fabricante: "),
            Utils.PhoneNumberPromptBox(title, "Número do fabricante: ")));
        Utils.MsgBox("Info", "✓ Fabricante registrado com sucesso!");
    }
    public override void Edit()
    {
        Manufacturer manufacturer = Select();
        Manufacturer editedManufacturer = new(manufacturer);
        string[] options = ["Nome", "Email", "Telefone", "Voltar"];
        while (true)
        {
            switch (Utils.Menu("Editar fabricante", options))
            {
                case 0:
                    editedManufacturer.Name = Utils.GetValidString("Editar nome", "Nome do fabricante: ");
                    break;
                case 1:
                    editedManufacturer.Email = Utils.GetValidEmail("Editar email", "Email do fabricante: ");
                    break;
                case 2:
                    editedManufacturer.PhoneNumber = Utils.GetValidString("Editar telefone", "Telefone do fabricante: ");
                    break;
                case 3:
                    if (!editedManufacturer.Equals(manufacturer))
                    {
                        Utils.MsgBox("Info", "✓ Fabricante atualizado com sucesso!");
                        manufacturer.UpdateEntity(editedManufacturer);
                    }
                    return;
            }
        }
    }

    public override void Remove()
    {
        Manufacturer manufacturer = Select();
        if (manufacturer.Equipments.Count > 0) Utils.MsgBox("Aviso", "Não é possível remover este fabricante porque possui equipamentos registrados. Remova-os primeiro.");
        else if (Repository.Remove(manufacturer.Id)) Utils.MsgBox("Info", "✓ Fabricante removido com sucesso!");
        else Utils.MsgBox("Info", "✗ Erro ao remover o fabricante. Tente novamente.");
    }

    public override Manufacturer Select(List<Manufacturer>? manufacturers = null)
    {
        var availableManufacturers = GetAvailable(manufacturers);

        string[] options = availableManufacturers.Select(m => $"{m.Name}, ID: {m.Id}").ToArray();

        return availableManufacturers[Utils.Menu("Selecionar fabricante", options)];
    }

    public override void View()
    {
        string[] categories = ["Nome", "Email", "Telefone", "Equipamentos"];
        List<string[]> manufacturers = [];
        foreach (Manufacturer m in Repository.GetAll())
            manufacturers.Add([m.Name, m.Email, m.PhoneNumber, $"{m.Equipments.Count}"]);
        Utils.GenerateTable("Fabricantes", categories, manufacturers.ToArray());
    }
}
