using System;
using System.Collections.Generic;
using Scripts.ValueTypes;
using Scripts.Utilities;

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

        private readonly Dictionary<Person, float> _affinity = new Dictionary<Person, float>();

        public float this[Person other]
        {
            get
            {
                if (_affinity.TryGetValue(other, out var a))
                    return a;
                _affinity[other] = a = Personality.Affinity(other.Personality);
                return a;
            }
            set => _affinity[other] = value;
        }

        public readonly Location WorkLocation = ValueTypes.Location.Random();
        public readonly bool DayShift = Randomize.Boolean(Randomize.RngForInitialization);
        public Fingerprint Mood;

        public void UpdateMood() => Mood = Fingerprint.Mood();

        public void UpdateLocation(bool isDaytime)
        {
            if (isDaytime ^ DayShift)
            {
                // We're off work
                Location bestPlace = null!;
                float bestScore = float.MinValue;
                foreach (var l in Location.Everywhere)
                {
                    var match = Personality.Affinity(l.Personality, Mood);
                    if (match > bestScore)
                    {
                        bestScore = match;
                        bestPlace = l;
                    }
                }
                Location = bestPlace;
            }
            else
            {
                // We're at work
                Location = WorkLocation;
            }
        }

        public void Socialize()
        {
            Person bestPerson = null;
            float bestScore = float.MinValue;
            foreach (var p in Person.Everyone)
            {
                var match = this[p];
                // Should this be absolute value for some people?
                if (match > bestScore)
                {
                    bestScore = match;
                    bestPerson = p;
                }
            }

            var other = bestPerson;
            var outcome = Interactions.Interact(this, other, Utilities.Randomize.RngForInitialization);
            this[other] += outcome.ActorDelta;
            other[this] += outcome.OtherDelta;
        }

        public static void UpdateEveryone(bool isDaytime)
        {
            foreach (var p in Person.Everyone)
                p.UpdateMood();
            foreach (var p in Person.Everyone)
                p.UpdateLocation(isDaytime);
            foreach (var p in Person.Everyone)
                p.Socialize();
        }
    }
}