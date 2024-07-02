using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
    using static Clock;          // All current time functions
    using static CsvManager;     // DeclareParsers
    using static SaveManager;    // Save and Load simulation
    using static File;           // Performance CSV output
    using static Randomize;      // Seed and RandomElement
    // GUI/graphics
    using static Color;
    using static GUIManager;       // Colorizers and Pop tables
    using static SimulationGraphs; // Visualize___ functions
    // The following offload static components of the TED code...
    using static StaticTables; // non dynamic tables - classic datalog EDB
    using static Variables;

    public static class Benchmark {
        private const int Seed = 349571286;
        public static Simulation Simulation = null!;
        public static bool RecordingPerformance;
        private static TablePredicate<Person, Location> _whereTheyAre;
        private static TablePredicate<Person, Person, Interactions.Outcome> _interactedWith;
        private static TablePredicate<Person, Person, float> _affinity;
        private static readonly List<(uint, double)> PerformanceData = new();

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
            Location.CreateAll(100);
            Person.CreateAll(2000);
            
            Simulation = new Simulation("Benchmark");
            Simulation.Exceptions.Colorize(_ => red);
            Simulation.Problems.Colorize(_ => red);
            Simulation.BeginPredicates();
            _whereTheyAre = Predicate("WhereTheyAre", person, location);
            _interactedWith = Predicate("InteractedWith", person, other, outcome);
            _affinity = Predicate("Affinity", person, other, affinity);
            Simulation.EndPredicates();
        }

        public static readonly Stopwatch Stopwatch = new Stopwatch();
        public static double UpdateTime;
        
        public static void UpdateSimulator() {
            Tick();
            Stopwatch.Reset();
            Stopwatch.Start();
            Person.UpdateEveryone(ClockTick % 2 == 1);
            Stopwatch.Stop();
            UpdateTime = (15*UpdateTime+Stopwatch.Elapsed.TotalMilliseconds)/16;
            
            _whereTheyAre.Clear();
            //_whereTheyAre.AddRows(Person.Everyone.Select(p => (p, p.Location)));
            _interactedWith.Clear();
            //_interactedWith.AddRows(Person.Everyone.Select(p => (p, p.Other, p.Outcome)));
            _affinity.Clear();
            //_affinity.AddRows(Person.Everyone.SelectMany(p => p.Affinity.Select(pair => (p, pair.Key, pair.Value))));
            
            if (!RecordingPerformance) return;
            switch (ClockTick) {
                case 2000: {
                    foreach (var person in Person.Everyone) person.Affinity.Clear();
                    return;
                }
                case < 3000: return;
            }
            PerformanceData.Add((ClockTick, Stopwatch.Elapsed.TotalMilliseconds));
            if (ClockTick == 4000) {
                using var file = AppendText("PerformanceData.csv");
                foreach ((var clock, var execution) in PerformanceData) file.WriteLine($"{clock}, {execution}");
                PerformanceData.Clear();
                RecordingPerformance = false;
            }
        }
    }
}
