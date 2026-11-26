using System;
using System.Collections.Generic;
using System.Text;

namespace F1StrategySimulator
{
    public record LapResult(int BolidId, string PlayerName, int Position, double LapTime, 
        double TotalTime, double GapToLeader, TyreCompound CurrentTyre, double TyreHealth, RaceEventType Event, bool IsDNF);
}
