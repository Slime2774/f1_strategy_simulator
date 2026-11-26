namespace F1StrategySimulator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            RaceEngine re = new RaceEngine();
            re.AddPlayer("Max", TyreCompound.Soft);
            re.AddPlayer("Lewis", TyreCompound.Hard);

            while (!re.IsFinished)
            {
                Console.WriteLine("==========================================");
                Console.WriteLine($"Круг {re.CurrentLap + 1} из {re.TotalLaps} | Погода: {re.CurrentWeather}");
                Console.WriteLine("------------------------------------------");

                // Вывод телеметрии каждого болида перед кругом
                foreach (var p in re.Players)
                {
                    string status = p.IsDNF ? "DNF" : $"{p.CurrentTyre} ({p.TyreHealth:F1}%) | Пит-стопов: {p.PitStopsCount}";
                    Console.WriteLine($"[{p.Id}] {p.Name}: {status}");
                }
                Console.WriteLine();

                var decisions = new List<LapDecision>();

                foreach (var player in re.Players)
                {
                    if (player.IsDNF) continue;

                    Console.WriteLine($"--- Ход игрока {player.Name} ---");
                    Console.Write("Выберите темп (1 - Balanced, 2 - Push, 3 - Save): ");
                    string? paceInput = Console.ReadLine();

                    PaceMode pace = paceInput switch
                    {
                        "2" => PaceMode.Push,
                        "3" => PaceMode.Save,
                        _ => PaceMode.Balanced
                    };

                    Console.Write("Пит-стоп? (0 - Нет, 1 - Soft, 2 - Medium, 3 - Hard, 4 - Inter, 5 - Wet): ");
                    string? pitInput = Console.ReadLine();

                    TyreCompound? pitTyre = pitInput switch
                    {
                        "1" => TyreCompound.Soft,
                        "2" => TyreCompound.Medium,
                        "3" => TyreCompound.Hard,
                        "4" => TyreCompound.Intermediate,
                        "5" => TyreCompound.Wet,
                        _ => null
                    };

                    decisions.Add(new LapDecision(player.Id, pace, pitTyre));
                }

                // Расчёт круга
                var results = re.SimulateLap(decisions);

                Console.WriteLine("\n--- Итоги круга ---");
                foreach (var r in results)
                {
                    if (r.IsDNF)
                    {
                        Console.WriteLine($"P{r.Position} {r.PlayerName} — СХОД ({r.Event})");
                        continue;
                    }

                    string gap = r.Position == 1 ? "Лидер" : $"+{r.GapToLeader:F3}с";
                    string eventText = r.Event != RaceEventType.None ? $" | Событие: {r.Event}" : "";
                    Console.WriteLine($"P{r.Position} {r.PlayerName} | Круг: {r.LapTime:F3}с | {gap} | Шины: {r.CurrentTyre} ({r.TyreHealth:F1}%){eventText}");
                }
                Console.WriteLine();
            }

            Console.WriteLine("Гонка завершена!");
        }
    }
}