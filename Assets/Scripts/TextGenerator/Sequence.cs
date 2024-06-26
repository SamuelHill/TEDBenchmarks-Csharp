﻿using System;
using System.Linq;
using System.Text;

namespace Scripts.TextGenerator {
    public class Sequence : TextGenerator {
        private readonly TextGenerator[] _generators;
        public Sequence(TextGenerator[] generators) => _generators = generators;

        public override bool Generate(StringBuilder output, BindingList b, Random rng)
            => _generators.All(g => g.Generate(output, b, rng));
    }
}
