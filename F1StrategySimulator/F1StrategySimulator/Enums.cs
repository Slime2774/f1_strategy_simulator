using System;
using System.Collections.Generic;
using System.Text;

namespace F1StrategySimulator
{
    /// <summary>
    /// Класс всех игровых состояний
    /// </summary>

    //Состав шин
    public enum TyreCompound 
        {
            Soft,
            Medium,
            Hard,
            Intermediate,
            Wet
        }
    // Погода
    public enum WeatherType
        {
            Dry,
            LightRain,
            HeavyRain,
        } 
    // Тип езды
    public enum PaceMode
        {
            Balanced,
            Push,
            Save
        }
    // Ивенты заезда
    public enum RaceEventType
        {
            None,
            SafetyCar,
            FrontWingDamage,
            TyreFailure
        } 

    // Настройки Шин
    public record TyreConfig(int MaxEndurance, double PaceModifier, int BaseWearPerLap); // Прочность, Модификатор Скорости, Износ За Круг

    // Коллекция шин
    public static class TyreDatabase
    {
        public static readonly Dictionary<TyreCompound, TyreConfig> Configs = new()
        {
            { TyreCompound.Soft,  new TyreConfig(100, -2.0, 15 ) },
            { TyreCompound.Medium, new TyreConfig(200, -0.5, 10)},
            { TyreCompound.Hard, new TyreConfig(300, +0.8, 6) },
            { TyreCompound.Intermediate, new TyreConfig(150, +1.5, 8)},
            { TyreCompound.Wet, new TyreConfig(120, +3.0, 8)}
        };
    }
}
