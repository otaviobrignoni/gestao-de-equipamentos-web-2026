using System.Text;
using GestaoDeEquipamentos.ConsoleApp.Shared;

namespace GestaoDeEquipamentos.ConsoleApp;

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        UserInterface.MainMenu();
    }
}
