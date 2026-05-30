using System.Diagnostics;
using static F1StrategySimulator.Bolid;

namespace F1StrategySimulator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Bolid bl = new Bolid();

            while (bl.aging == 0)
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
            bl.StartRace();
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
                default: Console.WriteLine("Неверный выбор! Попробуйте снова"); ; break;
            }
        }

        public void StartRace()
        {
            Console.Clear();
            int temp = aging;
            Console.WriteLine("=== Гонка началась ===");
            for (int lap = 0; lap<10; lap++)
            {
                if (temp == 100)
                {
                    aging -= 15;
                }
                else if (temp == 150)
                {
                    aging -= 10;
                }
                else if (temp == 200)
                {
                    aging -= 5;
                }

                Console.WriteLine($"Текущий круг {lap+1}");

                if (aging < 0)
                {
                    Console.WriteLine($"Состояние шин 0%. Шины умерли");
                }
                else
                {
                    Console.WriteLine($"Состояние шин {(aging * 100) / temp}%");
                }

                Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                Console.ReadKey();
            }
        }


    }

}
