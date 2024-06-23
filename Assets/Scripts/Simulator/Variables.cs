using System;
using TED;
using Scripts.Time;
using Scripts.ValueTypes;
using UnityEngine;

namespace Scripts.Simulator {
    /// <summary>Commonly used TED.Vars for types used in this simulator.</summary>
    /// <remarks>Variables will be lowercase for style/identification purposes.</remarks>
    public static class Variables {
        // ReSharper disable InconsistentNaming
        public static readonly Var<string> firstName = (Var<string>)"firstName";
        public static readonly Var<string> lastName = (Var<string>)"lastName";

    }
}
