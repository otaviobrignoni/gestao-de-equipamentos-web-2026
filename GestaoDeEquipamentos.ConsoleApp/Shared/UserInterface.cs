using GestaoDeEquipamentos.ConsoleApp.CallModule;
using GestaoDeEquipamentos.ConsoleApp.EquipmentModule;

namespace GestaoDeEquipamentos.ConsoleApp.Shared;

public static class UserInterface
{
    static EquipmentRepo equipmentRepo = new();
    static CallRepo callRepo = new();
    static EquipmentUI eUI = new(equipmentRepo);
    static CallUI cUI = new(eUI, callRepo);
    public static void MainMenu()
    {
        string[] options = ["Equipamentos", "Chamados de Manutenção", "Sair"];
        while (true)
        {
            switch (Utils.Menu("Gestão de Equipamentos", options))
            {
                case 0:
                    eUI.Menu();
                    break;
                case 1:
                    cUI.Menu();
                    break;
                case 2: return;
            }
        }
    }
}
