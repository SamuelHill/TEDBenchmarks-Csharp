using System;
using System.Linq;
using TED;
using TED.Interpreter;
using TED.Tables;
using Scripts.Time;
using Scripts.Utilities;
using Scripts.ValueTypes;
using Scripts.TextGenerator;
using UnityEngine;
using static TED.Language;

namespace Scripts.Simulator {

    /// <summary>
    /// All static tables (Datalog style EDBs - in TED these can be extensional or intensional).
    /// </summary>
    public static class StaticTables {

        public static void InitStaticTables() {
            // FILL IN WITH STATIC TABLES
        }

        private static TablePredicate<T> EnumTable<T>(string name, IColumnSpec<T> column) where T : Enum {
            var table = Predicate(name, column);
            table.AddRows(Enum.GetValues(typeof(T)).Cast<T>());
            return table;
        }
    }
}
