using System;
using TED;
using Scripts.Simulog;
using Scripts.Time;
using Scripts.ValueTypes;
using UnityEngine;

namespace Scripts.Simulator {
    /// <summary>Commonly used TED.Vars for types used in this simulator.</summary>
    /// <remarks>Variables will be lowercase for style/identification purposes.</remarks>
    public static class Variables {
        // ReSharper disable InconsistentNaming

        // FILL IN WITH VARIABLES

        // ********************************** Socialog Variables **********************************

        public static readonly Var<bool> state = (Var<bool>)"state";
        public static readonly Var<bool> exists = (Var<bool>)"exists";
        public static readonly Var<int> count = (Var<int>)"count";
        public static readonly Var<TimePoint> time = (Var<TimePoint>)"time";
        public static readonly Var<TimePoint> start = (Var<TimePoint>)"start";
        public static readonly Var<TimePoint> end = (Var<TimePoint>)"end";
    }
}
