using System.Diagnostics;
using static F1StrategySimulator.Bolid;

namespace F1StrategySimulator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Bolid bl = new Bolid();

            while (true)
            { 
                Console.Clear();
                Console.WriteLine("=== Добро пожаловать в F1 симулятор ===\n\n");

                Console.WriteLine("=== Выберите ваши шины ===\n");
                Console.WriteLine("1. Soft (Быстрые, но быстро изнашиваются)\n");
                Console.WriteLine("2. Medium (Стандартные)\n");
                Console.WriteLine("3. Hard (Долговечные, но медленные)\n");
                bl.userInput = Console.ReadLine();


                try 
                { 
                    bl.SetTyreStrategy(bl.userInput);
                    Console.WriteLine($"Вы выбрали шины. Стартовый ресурс: {bl.aging} кругов/у.е.");
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }
                Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                Console.ReadKey();
            }
        }
    }

    class Bolid
    {
        public string userInput { get; set; }
        
        public int aging { get; private set; }

        public void SetTyreStrategy(string answer)
        {
            userInput = answer;
            switch (answer)
            {
                case "1": aging = 100; break;
                case "2": aging = 150; break;
                case "3": aging = 200; break;
                default: aging = 0; break;
            }
        }
    }

}
