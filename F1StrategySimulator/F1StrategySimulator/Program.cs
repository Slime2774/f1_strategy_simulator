using System.Diagnostics;
using static F1StrategySimulator.Bolid;

namespace F1StrategySimulator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Bolid bl = new Bolid();
            Console.WriteLine("Введите своё имя:\n");
            bl.Name = Console.ReadLine();

            while (true)
            {
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
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ошибка: {ex.Message}");
                    }
                    Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                    Console.ReadKey();
                }
                bl.StartRace();

                Console.WriteLine($"\n{bl.Name} хочешь начать новую гонку?");
                Console.WriteLine("1. Да, погнали");
                Console.WriteLine("2. Нет, выйти из симулятора");
                string ChoiseRace = Console.ReadLine();
                
                if (ChoiseRace == "1")
                {
                    bl.RestartRace();
                }

                else if (ChoiseRace == "2")
                {
                    Console.WriteLine("Спасибо за игру! До встери на трассе!");
                    break;
                }
            }
        }
    }

    class Bolid
    {
        public string Name { get; set; }

        public string userInput { get; set; }
        
        public int aging { get; private set; }

        public double totalTime {  get; private set; }

        public void SetTyreStrategy(string answer)
        {
            totalTime = 0;
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
            double baseLapTime = 80.0;
            double lapModificator = 0;

            Random rnd = new Random();
            Console.WriteLine("=== Гонка началась ===");
            for (int lap = 0; lap<10; lap++)
            {
                if (temp == 100) // Soft
                {
                    aging -= 15;
                    lapModificator = (aging > 30) ? -2.0 : +3.5;
                }
                else if (temp == 150) //Medium
                {
                    aging -= 10;
                    lapModificator = (aging > 20) ? -0.5 : +1;
                }
                else if (temp == 200) //Hard
                {
                    aging -= 5;
                    lapModificator = 1;
                }

                double currentLapTime = baseLapTime + lapModificator + (rnd.NextDouble() * 0.5);
                int EventChance = rnd.Next(1, 6);
                if (EventChance == 3)
                {
                    Console.WriteLine("Жёлтые флаги! На трассе пейс-кар. Скорость падает!");
                    currentLapTime += 15;
                }

                totalTime += currentLapTime;
                Console.WriteLine($"Текущий круг {lap+1}");

                if (aging < 0)
                {
                    Console.WriteLine($"Состояние шин 0%. Шины умерли");
                }
                else
                {
                    Console.WriteLine($"Состояние шин {(aging * 100) / temp}%");
                }
                Console.WriteLine($"Текущее время круга {currentLapTime:F3}");
                Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                Console.ReadKey();
            }
            Console.Clear();
            Console.WriteLine("\n=== Гонка окончена! ===");

            Console.WriteLine($"Ваше время {totalTime:F3}");
            if (totalTime <= 830.0)
            {
                Console.WriteLine($"Simply Lovely. {Name} побеждает в гонке!!!");
            }
            else
            {
                Console.WriteLine("Хорошая работа, повезёт в следующий раз");
            }

        }

        public void RestartRace()
        {
            userInput = "";
            aging = 0;
        }

    }

}
