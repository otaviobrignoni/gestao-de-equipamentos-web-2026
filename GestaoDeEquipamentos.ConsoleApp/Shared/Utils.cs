namespace GestaoDeEquipamentos.ConsoleApp.Shared;

public static class Utils
{
    public const int MenuWidth = UserInterface.MenuWidth;
    public static int Menu(string title, string[] options)
    {
        int selectedIndex = 0;
        while (true)
        {
            Console.Clear();
            Console.WriteLine("╭─ " + title + " " + new string('─', MenuWidth - title.Length - 3) + "╮ ");
            for (int i = 0; i < options.Length; i++)
            {
                string option = i == selectedIndex ? $" ❯ {options[i]}" : $"   {options[i]}";
                Console.WriteLine("│" + option.PadRight(MenuWidth) + "│");
            }
            Console.WriteLine("╰" + new string('─', MenuWidth) + "╯");
            Console.WriteLine("⇅ selecionar | ↲ Confirmar");
            ConsoleKey key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.UpArrow: selectedIndex = (selectedIndex == 0) ? options.Length - 1 : selectedIndex - 1; break;

                case ConsoleKey.DownArrow: selectedIndex = (selectedIndex + 1) % options.Length; break;

                case ConsoleKey.Enter: return selectedIndex;
            }
        }
    }
    public static string PromptBox(string title, string msg)
    {
        Console.Clear();
        Console.WriteLine("╭─ " + title + " " + new string('─', MenuWidth - title.Length - 3) + "╮ ");
        Console.WriteLine("│ " + msg.PadRight(MenuWidth - 1) + "│");
        Console.WriteLine("╰" + new string('─', MenuWidth) + "╯");
        Console.SetCursorPosition(msg.Length + 2, 1);
        string userInput = Console.ReadLine() ?? string.Empty;
        Console.SetCursorPosition(0, 0);
        return userInput;
    }
    public static void MsgBox(string title, string msg)
    {
        string[] msgArray = msg.Split('\0');
        Console.Clear();
        Console.WriteLine("╭─ " + title + " " + new string('─', MenuWidth - title.Length - 3) + "╮ ");
        foreach (string s in msgArray)
            Console.WriteLine("│ " + s.PadRight(MenuWidth - 1) + "│");
        Console.WriteLine("╰" + new string('─', MenuWidth) + "╯");
        EnterPrompt();
    }
    public static void TableBox(string title, string[] headers, string[][] rows, int[]? widths = null)
{
    widths ??= headers.Select(h => h.Length).ToArray();

    for (int i = 0; i < headers.Length; i++)
    {
        foreach (string[] row in rows)
        {
            if (i < row.Length && row[i].Length > widths[i])
                widths[i] = row[i].Length;
        }
    }

    int innerWidth = widths.Sum() + (headers.Length * 3) + 1;

    Console.Clear();
    Console.WriteLine("╭─ " + title + " " + new string('─', innerWidth - title.Length - 5) + "╮");

    Console.Write("│");
    for (int i = 0; i < headers.Length; i++)
        Console.Write(" " + headers[i].Center(widths[i]) + " │");
    Console.WriteLine();

    Console.WriteLine("├" + string.Join("┼", widths.Select(w => new string('─', w + 2))) + "┤");

    foreach (string[] row in rows)
    {
        Console.Write("│");
        for (int i = 0; i < headers.Length; i++)
        {
            string value = i < row.Length ? row[i] : "";
            Console.Write(" " + value.FitToWidth(widths[i]) + " │");
        }
        Console.WriteLine();
    }

    Console.WriteLine("╰" + string.Join("┴", widths.Select(w => new string('─', w + 2))) + "╯");

    EnterPrompt();
}
    public static void EnterPrompt(string? msg = null)
    {
        Console.WriteLine(msg ?? "Pressione ENTER para continuar…");
        while (Console.ReadKey(true).Key != ConsoleKey.Enter) ;
    }
}
