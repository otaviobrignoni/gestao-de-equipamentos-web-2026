using GestaoDeEquipamentos.ConsoleApp.CallModule;
using GestaoDeEquipamentos.ConsoleApp.EquipmentModule;
using GestaoDeEquipamentos.ConsoleApp.ManufacturerModule;

namespace GestaoDeEquipamentos.ConsoleApp.Shared;

public static class UserInterface
{
    static EquipmentRepo equipmentRepo = new();
    static CallRepo callRepo = new();
    static ManufacturerRepo manufacturerRepo = new();
    static ManufacturerUI mUI = new(manufacturerRepo);
    static EquipmentUI eUI = new(mUI, equipmentRepo);
    static CallUI cUI = new(eUI, callRepo);
    public static void MainMenu()
    {
        string[] options = ["Fabricantes", "Equipamentos", "Chamados de Manutenção", "Sair"];
        while (true)
        {
            switch (Utils.Menu("Gestão de Equipamentos", options))
            {
                case 0:
                    mUI.Menu();
                    break;
                case 1:
                    eUI.Menu();
                    break;
                case 2:
                    cUI.Menu();
                    break;
                case 3: return;
            }
        }
    }
}
