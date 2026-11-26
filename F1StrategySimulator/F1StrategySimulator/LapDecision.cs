using System;
using System.Collections.Generic;
using System.Text;

namespace F1StrategySimulator
{
    public record LapDecision(int BolidId, PaceMode Pace = PaceMode.Balanced, TyreCompound? PitStopTyre = null);
}
