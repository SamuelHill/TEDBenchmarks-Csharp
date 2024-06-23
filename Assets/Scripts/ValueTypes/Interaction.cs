using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.ValueTypes
{
    public static class Interactions
    {
        public struct Outcome
        {
            public readonly float ActorDelta;
            public readonly float OtherDelta;

            public Outcome(float actorDelta, float otherDelta)
            {
                ActorDelta = actorDelta;
                OtherDelta = otherDelta;
            }
        }
        public delegate Outcome Interaction(Person actor, Person other, Random rng);

        static Outcome Chat(Person actor, Person other, Random rng)
        {
            // fill me in
            return new Outcome(0, 0);
        }

        public static readonly Interaction[] PossibleInteractions = { Chat };

        public static Outcome Interact(Person actor, Person other, Random rng) =>
            PossibleInteractions[rng.Next() % PossibleInteractions.Length](actor, other, rng);
    }
}
