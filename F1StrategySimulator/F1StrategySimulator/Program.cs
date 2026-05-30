namespace F1StrategySimulator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Bolid bl = new Bolid();


            Console.WriteLine("=== Добро пожаловать в F1 симулятор ===\n\n");
            Console.WriteLine("=== Выберите ваши шины ===\n");
            Console.WriteLine("1. Soft (Быстрые, но быстро изнашиваются)\n");
            Console.WriteLine("2. Medium (Стандартные)\n");
            Console.WriteLine("3. Hard (Долговечные, но медленные)\n");
            bl.userInput = Console.ReadLine();
        }
    }

    class Bolid
    {
        public string userInput { get; set; }
    }

}
