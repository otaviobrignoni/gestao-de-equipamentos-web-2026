using System.Globalization;
using GestaoDeEquipamentos.ConsoleApp.CallModule;
using GestaoDeEquipamentos.ConsoleApp.EquipmentModule;

namespace GestaoDeEquipamentos.ConsoleApp.Shared;

public static class UserInterface
{
    public const int MenuWidth = 80;
    public static EquipmentRepo equipmentRepo = new();
    public static CallRepo callRepo = new();
    public static void MainMenu()
    {
        string[] options = ["Equipamentos", "Chamados de Manutenção", "Sair"];
        while (true)
        {
            switch (Utils.Menu("Gestão de Equipamentos", options))
            {
                case 0:
                    EquipmentMenu();
                    break;
                case 1:
                    CallMenu();
                    break;
                case 2: return;
            }
        }
    }
    // Equipment
    public static void EquipmentMenu()
    {
        string[] options = ["Novo equipamento", "Editar equipamento", "Remover equipamento", "Visualizar equipamentos", "Voltar"];
        while (true)
        {
            switch (Utils.Menu("Menu de Equipamentos", options))
            {
                case 0:
                    CreateNewEquipment();
                    break;
                case 1:
                    if (equipmentRepo.Count() < 1)
                    {
                        Utils.MsgBox("Aviso", "Nenhum equipamento existe para editar");
                        continue;
                    }
                    EditEquipment();
                    break;
                case 2:
                    if (equipmentRepo.Count() < 1)
                    {
                        Utils.MsgBox("Aviso", "Nenhum equipamento existe para remover");
                        continue;
                    }
                    RemoveEquipment();
                    break;
                case 3:
                    if (equipmentRepo.Count() < 1)
                    {
                        Utils.MsgBox("Aviso", "Nenhum equipamento existe para visualizar");
                        continue;
                    }
                    ViewEquipments();
                    break;
                case 4: return;
            }
        }
    }
    public static Equipment SelectEquipmentMenu(Equipment? equipment = null)
    {
        List<Equipment> availableEquipments = equipmentRepo.GetAll().Where(e => e != equipment).ToList();

        string[] options = availableEquipments.Select(e => $"{e.Name} ID: {e.Id}").ToArray();

        return availableEquipments[Utils.Menu("Selecionar equipamento", options)];
    }
    public static void CreateNewEquipment()
    {
        string title = "Criar equipamento";
        equipmentRepo.Add(new Equipment(
            GetValidString(title, "Nome do equipamento: "),
            GetValidPrice(title, "Preço de aquisição: "),
            GetValidString(title, "Nome do fabricante: "),
            GetDate(title, "Data de fabricação (__/__/____): ")));
        Utils.MsgBox("Info", "Equipamento criado com sucesso");
    }
    public static void EditEquipment()
    {
        Equipment equipment = SelectEquipmentMenu();
        Equipment editedEquipment = new Equipment(equipment.Name, equipment.Price, equipment.Manufacturer, equipment.Date);
        string[] options = ["Nome", "Preço de aquisição", "Nome do fabricante", "Data de fabricação", "Voltar"];

        while (true)
        {
            switch (Utils.Menu("Editar equipamento", options))
            {
                case 0:
                    editedEquipment.Name = GetValidString("Editar nome", "Nome do equipamento: ");
                    break;
                case 1:
                    editedEquipment.Price = GetValidPrice("Editar preço", "Preço do equipamento");
                    break;
                case 2:
                    editedEquipment.Manufacturer = GetValidString("Editar fabricante", "Nome do fabricante: ");
                    break;
                case 3:
                    editedEquipment.Date = GetDate("Editar data", "Data de aquisição");
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
    public static void RemoveEquipment()
    {
        Guid id = SelectEquipmentMenu().Id;
        if (callRepo.GetAll().Any(c => c.Equipment.Id == id)) Utils.MsgBox("Aviso", "Não é possível remover um equipamento com chamado aberto");
        else if (equipmentRepo.Remove(id)) Utils.MsgBox("Info", "Equipamento removido com sucesso");
        else Utils.MsgBox("Info", "Ocorreu um erro na remoção do equipamento");
    }
    public static void ViewEquipments()
    {
        string[] categories = ["Nome", "Preço", "Fabricante", "Data", "Id"];
        List<string[]> equipments = [];
        foreach (Equipment e in equipmentRepo.GetAll())
            equipments.Add([e.Name, $"{e.Price:C2}", e.Manufacturer, $"{e.Date}", $"{e.Id}"]);
        Utils.TableBox("Equipamentos", categories, equipments.ToArray(), [12, 6, 12, 8, 36]);
    }
    // Maintenance Call
    public static void CallMenu()
    {
        string[] options = ["Novo chamado", "Editar chamado", "Remover chamado", "Visualizar chamados", "Voltar"];
        while (true)
        {
            switch (Utils.Menu("Menu de Chamados de Manutenção", options))
            {
                case 0:
                    if (equipmentRepo.Count() < 1)
                    {
                        Utils.MsgBox("Aviso", "Nenhum equipament existe para fazer um chamdo");
                    }
                    CreateNewMaintenanceCall();
                    break;
                case 1:
                    if (callRepo.Count() < 1)
                    {
                        Utils.MsgBox("Aviso", "Nenhum chamado existe para editar");
                        continue;
                    }
                    EditMaintenanceCall();
                    break;
                case 2:
                    if (callRepo.Count() < 1)
                    {
                        Utils.MsgBox("Aviso", "Nenhum chamado existe para remover");
                        continue;
                    }
                    RemoveMaintenanceCall();
                    break;
                case 3:
                    if (callRepo.Count() < 1)
                    {
                        Utils.MsgBox("Aviso", "Nenhum chamado existe para visualizar");
                        continue;
                    }
                    ViewMaintenanceCalls();
                    break;
                case 4: return;
            }
        }
    }
    public static MaintenanceCall SelectMaintenanceCall(MaintenanceCall? maintenanceCall = null)
    {
        List<MaintenanceCall> availableCalls = callRepo.GetAll().Where(e => e != maintenanceCall).ToList();

        string[] options = availableCalls.Select(c => $"{c.Title} : {c.Equipment.Name}, ID: {c.Id}").ToArray();

        return availableCalls[Utils.Menu("Selecionar chamado", options)];
    }
    public static void CreateNewMaintenanceCall()
    {
        string title = "Abrir chamado";
        callRepo.Add(new MaintenanceCall(
            GetValidString(title, "Titulo do chamado: "),
            GetValidString(title, "Descrição do chamado: "),
            SelectEquipmentMenu(),
            DateTime.Now));
        Utils.MsgBox("Info", "Chamado de manutenção aberto com sucesso");
    }
    public static void EditMaintenanceCall()
    {
        MaintenanceCall maintenanceCall = SelectMaintenanceCall();
        MaintenanceCall editedCall = new MaintenanceCall(maintenanceCall.Title, maintenanceCall.Description, maintenanceCall.Equipment, maintenanceCall.OpeningDate);
        string[] options = ["Título", "Descrição", "Equipamento", "Voltar"];

        while (true)
        {
            switch (Utils.Menu("Editar chamado", options))
            {
                case 0:
                    editedCall.Title = GetValidString("Editar título", "Título do chamado: ");
                    break;
                case 1:
                    editedCall.Description = GetValidString("Editar descrição", "Descrição do chamado: ");
                    break;
                case 2:
                    editedCall.Equipment = SelectEquipmentMenu(editedCall.Equipment);
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
    public static void RemoveMaintenanceCall()
    {
        Guid id = SelectMaintenanceCall().Id;
        if (callRepo.Remove(id)) Utils.MsgBox("Info", "Chamado removido com sucesso");
        else Utils.MsgBox("Info", "Ocorreu um erro na remoção do chamado");
    }
    public static void ViewMaintenanceCalls()
    {
        string[] categories = ["Título", "Equipamento", "Data de Abertura", "Aberto há", "Id"];
        List<string[]> calls = [];
        foreach (MaintenanceCall c in callRepo.GetAll())
            calls.Add([c.Title, c.Equipment.Name, $"{c.OpeningDate:dd/MM/yyyy HH:mm}", c.ElapsedTime(), $"{c.Id}"]);
        Utils.TableBox("Chamados de Manutenção", categories, calls.ToArray(), [12, 13, 16, 15, 36]);
    }
    // Helper Methods
    static string GetValidString(string title, string msg)
    {
        while (true)
        {
            string name = Utils.PromptBox(title, msg);
            if (string.IsNullOrEmpty(name))
            {
                Utils.MsgBox("Aviso", "Entrada inválida");
                continue;
            }
            if (name.Length < 3)
            {
                Utils.MsgBox("Aviso", "Deve ter pelo menos 3 letras");
                continue;
            }
            return name;
        }
    }
    static decimal GetValidPrice(string title, string msg)
    {
        decimal amount;
        while (!decimal.TryParse(Utils.PromptBox(title, msg), out amount) || amount <= 0)
        {
            if (amount <= 0)
            {
                Utils.MsgBox("Aviso", "Preço deve ser maior que zero");
                continue;
            }
            Utils.MsgBox("Aviso", "Entrada inválida");
        }
        return amount;
    }
    static DateOnly GetDate(string title, string msg)
    {
        while (true)
        {
            string input = Utils.PromptBox(title, msg);
            if (string.IsNullOrEmpty(input))
            {
                Utils.MsgBox("Aviso", "Entrada inválida");
                continue;
            }
            if (!DateOnly.TryParseExact(input, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly date))
            {
                Utils.MsgBox("Aviso", "Data inválida");
                continue;
            }
            else
            {
                return date;
            }
        }
    }
}
