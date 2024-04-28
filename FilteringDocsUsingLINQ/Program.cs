// Перевірка на відповідність CLS
[assembly: CLSCompliant(true)]

namespace FilteringDocsUsingLINQ
{
    internal class Program
    {
        /// <summary>
        /// Пункти меню 
        /// типу значень (ValueType)
        /// </summary>
        enum MenuOptions : byte
        {
            // Виконати програму
            Execute = 1,
            // Вийти з програми
            Exit,
        }

        static void Menu()
        {
            Console.WriteLine($"Оберіть пункт меню:");
            Console.WriteLine($"1.) Виконати програму;");
            Console.WriteLine($"2.) Завершити роботу;");

            Byte menuOption;

            while (!Byte.TryParse(Console.ReadLine(), out menuOption))
            {
                Console.WriteLine($"Введено неправильні дані, перевірте та спробуйте знову");
            }

            MenuOptions selectedOption = (MenuOptions)menuOption;

            switch (selectedOption)
            {
                case MenuOptions.Execute:
                    ExecuteProgram.Run();
                    Console.ReadLine();
                    break;
                case MenuOptions.Exit:
                    Environment.Exit(0);
                    break;
                default:
                    break;
            }

            Console.ReadLine();
        }

        static void Main(string[] args)
        {
            // Підтримка кириличних символів
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            Console.InputEncoding = System.Text.Encoding.Unicode;

            while (true)
            {
                Menu();
            }
        }
    }

}
