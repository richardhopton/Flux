using System;

namespace Flux.Conditions
{
    public enum Comparison
    {
        EqualTo,
        Null,
        Empty,
        NullOrEmpty,
        Contains,
        StartsWith,
        EndsWith,
        LessThan,
        LessThanOrEqualTo,
        GreaterThan,
        GreaterThanOrEqualTo
    }

    public static class ComparisonExtensions
    {
        private static readonly Comparison[] OperandOnlyComparisons = new[] { Comparison.Null, Comparison.Empty, Comparison.NullOrEmpty };
        public static Boolean IsOperandOnlyComparison(this Comparison comparison)
        {
            return comparison.ContainedWithin(OperandOnlyComparisons);
        }
    }
}