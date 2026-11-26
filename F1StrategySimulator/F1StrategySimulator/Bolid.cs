using System;
using System.Collections.Generic;
using System.Text;

namespace F1StrategySimulator
{
    public class Bolid
    {
        public int Id { get; private set; } // ID болида
        public string Name { get; private set; } = string.Empty; // Имя
        public TyreCompound CurrentTyre { get; private set; } // Текущая резина
        public double TyreHealth { get; private set; } // Состояние шины (в %)
        public double TotalTime { get; private set; } // Общее время гонки
        public int PitStopsCount { get; private set; } // Колличество Пит - Стопов
        public bool IsDNF { get; private set; } // Сошёл с дистанции

        //Конструктор болида
        public Bolid(int id, string name, TyreCompound startTyre)
        {
            Id = id;
            Name = name;
            SetTyres(startTyre);
        }

        // Выбор шин
        public void SetTyres(TyreCompound compound)
        {
            CurrentTyre = compound;
            TyreHealth = 100.0;
        }

        //Пит-стоп
        public void ApplyPitStop(TyreCompound compound, double pitTime = 20.0)
        {
            SetTyres(compound);
            PitStopsCount += 1;
            TotalTime += pitTime;

        }
        //Статисика за круг
        public void ApplyLapStats(double lapTime, double wear)
        {
            TotalTime += lapTime;
            TyreHealth = Math.Max(TyreHealth - wear ,0.0);
        }
        //Сход с дистанции
        public void Retire()
        {
            IsDNF = true;
        }

    }
}
