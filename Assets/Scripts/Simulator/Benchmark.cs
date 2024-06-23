using System;
using System.Collections.Generic;
using System.IO;
using TED;
using TED.Interpreter;
using TED.Tables;
using TED.Utilities;
using Scripts.TextGenerator;
using Scripts.Time;
using Scripts.Unity;
using Scripts.Utilities;
using Scripts.ValueTypes;
using UnityEngine;
using static TED.Language;

namespace Scripts.Simulator {
    // "Utils" (Utilities and Time)
    using static BindingList;    // Parameters for name generation
    using static Clock;          // All current time functions
    using TimePoint = TimePoint; // (clock hides TimePoint with a method, this fixes that)
    using static CsvManager;     // DeclareParsers
    using static SaveManager;    // Save and Load simulation
    using static File;           // Performance CSV output
    using static Generators;     // Name generation
    using static Randomize;      // Seed and RandomElement
    // GUI/graphics
    using static Color;
    using static GUIManager;       // Colorizers and Pop tables
    using static SimulationGraphs; // Visualize___ functions
    // The following offload static components of the TED code...
    using static StaticTables; // non dynamic tables - classic datalog EDB
    using static Variables;    // named variables

    public static class Benchmark {
        private const int Seed = 349571286;
        public static Simulation Simulation = null!;
        public static bool RecordingPerformance;

        static Benchmark() {
            DeclareParsers(); // Parsers used in the FromCsv calls in InitStaticTables
            DeclareWriters();
            Seed(Seed);
            SetDefaultColorizers();
            SetDescriptionMethods();
            if (RecordingPerformance) {
                using var file = CreateText("PerformanceData.csv");
            }
            ReadSaveData = reader => ClockTick = uint.Parse(reader.ReadToEnd());
            WriteSaveData = writer => writer.Write(ClockTick.ToString());
        }

        // ReSharper disable InconsistentNaming
        // Tables, despite being local or private variables, will be capitalized for style/identification purposes.
        

        public static void InitSimulator() {
            Simulation = new Simulation("Benchmark");
            Simulation.Exceptions.Colorize(_ => red);
            Simulation.Problems.Colorize(_ => red);
            Simulation.BeginPredicates();
            InitStaticTables();
            // ********************************************* BEGIN TABLES *********************************************

            // FILL IN WITH TABLES

            // ********************************************** END TABLES **********************************************
            // ReSharper restore InconsistentNaming
            Simulation.EndPredicates();
            DataflowVisualizer.MakeGraph(Simulation, "Visualizations/Dataflow.dot");
            UpdateFlowVisualizer.MakeGraph(Simulation, "Visualizations/UpdateFlow.dot");
            Simulation.Update(); // optional, not necessary to call Update after EndPredicates
            Simulation.CheckForProblems = true;
        }

        public static void UpdateSimulator() {
#if ParallelUpdate
            if (update == null) LoopSimulator();
#else
            Tick();
            if (RecordingPerformance) {
                using var file = AppendText("PerformanceData.csv");
                // TODO : replace 0 with population count
                file.WriteLine($"{0}, {ClockTick - InitialClockTick}, {Simulation.RuleExecutionTime}");
            }
            Simulation.Update();
            PopTableIfNewActivity(Simulation.Problems);
            PopTableIfNewActivity(Simulation.Exceptions);
#endif
        }

#if ParallelUpdate
        private static Task update;

        static void LoopSimulator(){
            Clock.Tick();
            Simulation.Update();
            update = Simulation.UpdateAsync().ContinueWith((_) => LoopSimulator());
        }
#endif
    }
}
