namespace F1StrategySimulator
{
    public class RaceEngine
    {
        private readonly Random _rnd = new Random();
        public List<Bolid> Players = new List<Bolid>();
        public int CurrentLap { get; set; } = 0;
        public int TotalLaps { get; set; } = 15;
        public WeatherType CurrentWeather { get; set; } = WeatherType.Dry;
        public bool IsFinished => CurrentLap >= TotalLaps || (Players.Count > 0 && Players.All(p => p.IsDNF));

        //Добавление игроков. 
        public void AddPlayer(string name, TyreCompound tyre)
        {
            int Id = Players.Count + 1; // Генерируем уникальный ID
            Bolid bld = new Bolid(Id, name, tyre);
            Players.Add(bld);
        }

        //Обновление погоды
        private void UpdateWeather()
        {
            double WeatherChance = _rnd.NextDouble(); // Генерируем число от 0.0 до 1.0
            const double Chance = 0.2; // Примерно 20%

            if (WeatherChance >= Chance)
                return;
            switch (CurrentWeather)
            {
                case WeatherType.Dry: CurrentWeather = WeatherType.LightRain; break;
                case WeatherType.HeavyRain: CurrentWeather = WeatherType.LightRain; break;
                case WeatherType.LightRain: CurrentWeather = (_rnd.Next(0, 2) == 0 ? WeatherType.Dry : WeatherType.HeavyRain); break;
            }
            
        }

        //Симуляция кругов
        public List<LapResult> SimulateLap(List<LapDecision> decisions)
        {
            if (IsFinished == true)
                return new List<LapResult>();

            var result = new List<LapResult>();
            CurrentLap++;
            UpdateWeather();

            bool isSafetyCar = _rnd.NextDouble() < 0.15;

            foreach (var bolid in Players)
            {
                double lapTime = 80.0;
                //Проверка на сход
                if (bolid.IsDNF)
                {
                    result.Add(new LapResult
                    (
                        bolid.Id, 
                        bolid.Name, 
                        0, 
                        0.0, 
                        bolid.TotalTime, 
                        0.0, 
                        bolid.CurrentTyre, 
                        bolid.TyreHealth, 
                        RaceEventType.None, 
                        true
                    )); continue;
                }
                // Поиск решения игрока
                var decision = decisions.FirstOrDefault(d => d.BolidId == bolid.Id)
                    ?? new LapDecision(bolid.Id, PaceMode.Balanced, null);

                //Обработка пит-стопа
                if (decision.PitStopTyre != null)
                {
                    bolid.ApplyPitStop(decision.PitStopTyre.Value);
                }

                //Расчёт износа шин
                var tyreConfig = TyreDatabase.Configs[bolid.CurrentTyre];

                lapTime += (tyreConfig.PaceModifier + (_rnd.NextDouble() * 0.5));

                double wear = tyreConfig.BaseWearPerLap;

                //Темп Пилота

                switch (decision.Pace)
                {
                    case PaceMode.Push: lapTime -= 1.2; wear *= 1.5; break;
                    case PaceMode.Save: lapTime += 1.0; wear *= 0.6; break;
                    case PaceMode.Balanced: break;
                }

                // Штрафы за не подгодящую резину
                if (CurrentWeather == WeatherType.Dry && bolid.CurrentTyre == TyreCompound.Wet)
                {
                    lapTime += 8.0;
                    wear *= 3.0;
                }
                if (CurrentWeather == WeatherType.Dry && bolid.CurrentTyre == TyreCompound.Intermediate)
                {
                    lapTime += 3.5;
                    wear *= 2.0;
                }
                if (CurrentWeather == WeatherType.LightRain && (bolid.CurrentTyre == TyreCompound.Soft 
                    || bolid.CurrentTyre == TyreCompound.Medium || bolid.CurrentTyre == TyreCompound.Hard))
                {
                    lapTime += 8.0;
                }
                if (CurrentWeather == WeatherType.HeavyRain && (bolid.CurrentTyre == TyreCompound.Soft
                    || bolid.CurrentTyre == TyreCompound.Medium || bolid.CurrentTyre == TyreCompound.Hard))
                {
                    lapTime += 18;
                }
                if (CurrentWeather == WeatherType.HeavyRain && bolid.CurrentTyre == TyreCompound.Intermediate)
                {
                    lapTime += 4;
                }

                //Штраф за мёртвую резину

                if (bolid.TyreHealth < 25.0)
                    lapTime += 3.5;

                //События на кругу
                var lapEvent = RaceEventType.None;

                double damageChance = (decision.Pace == PaceMode.Push || (CurrentWeather == WeatherType.HeavyRain && 
                    (bolid.CurrentTyre == TyreCompound.Soft || bolid.CurrentTyre == TyreCompound.Medium || bolid.CurrentTyre == TyreCompound.Hard))) ? 0.10 : 0.03;

                if (isSafetyCar)
                {
                    lapEvent = RaceEventType.SafetyCar;
                    lapTime += 15.0;
                }

                else if (_rnd.NextDouble() < damageChance)
                {
                    lapEvent = RaceEventType.FrontWingDamage;
                    lapTime += 10.0;
                }

                //Применение итогов к болиду и проверка на взрыв шины
                bolid.ApplyLapStats(lapTime, wear);
                if (bolid.TyreHealth <= 0)
                {
                    bolid.Retire();
                    lapEvent = RaceEventType.TyreFailure;
                }
                result.Add(new LapResult(
                    bolid.Id,
                    bolid.Name,
                    0,
                    lapTime,
                    bolid.TotalTime,
                    0,
                    bolid.CurrentTyre,
                    bolid.TyreHealth,
                    lapEvent,
                    bolid.IsDNF
                    ));
            }

            
            //Сортировка таблицы и расчёт отрывов
            var sortedResults = result.OrderBy(r => r.IsDNF).ThenBy(r => r.TotalTime).ToList();
            if (sortedResults.Count == 0)
                return sortedResults;

            double leaderTime = sortedResults[0].TotalTime;

            for ( int i = 0; i < sortedResults.Count; i++)
            {
                sortedResults[i] = sortedResults[i]
                    with { Position = i + 1, GapToLeader = sortedResults[i].IsDNF ? 0.0 : Math.Round(sortedResults[i].TotalTime - leaderTime, 3) };
            }


            return sortedResults;
        }

    }

}
