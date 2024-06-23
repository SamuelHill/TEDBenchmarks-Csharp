using System;
using System.Collections.Generic;

namespace Scripts.ValueTypes
{
    /// <summary>
    /// Members of Person that are specific to the C# implementation of the benchmark
    /// </summary>
    public partial class Person : IComparable<Person>, IEquatable<Person>
    {
        public static readonly List<Person> Everyone = new List<Person>();

        public static void CreateAll(int count)
        {
            for (var i = 0; i < count; i++)
            {
                Everyone.Add(new Person("Bob", $"Mc{i}", TED.Utilities.Random.MakeRng()));
            }
        }

        private Location _location;

        public Location Location
        {
            get => _location;
            set
            {
                if (_location == value)
                    return;
                if (!ReferenceEquals(_location, null))
                    _location.Occupants.Remove(this);
                _location = value;
                _location.Occupants.Add(this);
            }
        }
    }
}