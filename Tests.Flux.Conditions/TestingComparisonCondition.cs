using System;
using System.Globalization;
using Flux.Conditions;
using Flux.Core.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Flux.Conditions
{
    [TestClass]
    public class TestingComparisonCondition : TestBase<ComparisonCondition>
    {
            public static readonly String ANegationOperatorOf = "a NegationOperator of";
            public static readonly String AComparisonOf = "a Comparison of";
            public static readonly String AnOperandOf = "an Operand of";
            public static readonly String AValueOf = "a Value of";
            public static readonly String TheResultShouldBe = "the Result should be";
        
        public TestingComparisonCondition()
            : base(RegisterSteps)
        {
        }


        private static void RegisterSteps(StepDefinitions stepDefinitions)
        {
            stepDefinitions
                .RegisterWith<NegationOperator>(ANegationOperatorOf, (c, arg) => c.NegationOperator = arg)
                .RegisterWith<Comparison>(AComparisonOf, (c, arg) => c.Comparison = arg)
                .RegisterWith<Object>(AnOperandOf, (c, arg) => c.Operand = arg)
                .RegisterWith<Object>(AValueOf, (c, arg) => c.Value = arg)
                .RegisterThen<Boolean>(TheResultShouldBe, (c, arg, msg) => Assert.AreEqual(arg, c.Result, msg));
        }
        
        private class Test
        {
            public Comparison Comparison { get; private set; }
            public Object Operand { get; private set; }
            public Object Value { get; private set; }
            public Boolean Result { get; private set; }

            public Test(Comparison comparison, Object operand, Object value, Boolean result)
            {
                Comparison = comparison;
                Operand = operand;
                Value = value;
                Result = result;
            }
        }

        private static readonly DateTime TestDateTimeNow = DateTime.UtcNow;
        private static readonly Object TestObject1 = new Object();
        private static readonly Object TestObject2 = new Object();

        private static Test TestCase(Comparison comparison, Object operand, Boolean result)
        {
            return new Test(comparison, operand, null, result);
        }
        private static Test TestCase(Comparison comparison, Object operand, Object value, Boolean result)
        {
            return new Test(comparison, operand, value, result);
        }

        private static readonly Test[] TestCases = new[]
        {
            TestCase(Comparison.Null, null, true),
            TestCase(Comparison.Null, String.Empty, false),
            TestCase(Comparison.Empty, null, false),
            TestCase(Comparison.Empty, String.Empty, true),
            TestCase(Comparison.NullOrEmpty, null, true),
            TestCase(Comparison.NullOrEmpty, String.Empty, true),
            TestCase(Comparison.NullOrEmpty, "  \n \t", false),
            TestCase(Comparison.EqualTo, null, null, true),
            TestCase(Comparison.EqualTo, null, String.Empty, false),
            TestCase(Comparison.EqualTo, String.Empty, String.Empty, true),
            TestCase(Comparison.EqualTo, String.Empty, "Not String.Empty", false),
            TestCase(Comparison.EqualTo, Int16.MaxValue, Int16.MaxValue, true),
            TestCase(Comparison.EqualTo, Int16.MaxValue.ToString(NumberFormatInfo.CurrentInfo), Int16.MaxValue, true),
            TestCase(Comparison.EqualTo, Int16.MinValue.ToString(NumberFormatInfo.CurrentInfo), Int16.MaxValue, false),
            TestCase(Comparison.EqualTo, "Text", Int16.MaxValue, false),
            TestCase(Comparison.EqualTo, Int32.MaxValue, Int32.MaxValue, true),
            TestCase(Comparison.EqualTo, Int32.MaxValue.ToString(NumberFormatInfo.CurrentInfo), Int32.MaxValue, true),
            TestCase(Comparison.EqualTo, Int32.MinValue.ToString(NumberFormatInfo.CurrentInfo), Int32.MaxValue, false),
            TestCase(Comparison.EqualTo, "Text", Int32.MaxValue, false),
            TestCase(Comparison.EqualTo, Int64.MaxValue, Int64.MaxValue, true),
            TestCase(Comparison.EqualTo, Int64.MaxValue.ToString(NumberFormatInfo.CurrentInfo), Int64.MaxValue, true),
            TestCase(Comparison.EqualTo, Int64.MinValue.ToString(NumberFormatInfo.CurrentInfo), Int64.MaxValue, false),
            TestCase(Comparison.EqualTo, "Text", Int64.MaxValue, false),
            TestCase(Comparison.EqualTo, TestDateTimeNow, TestDateTimeNow, true),
            TestCase(Comparison.EqualTo, TestDateTimeNow.ToString(DateTimeFormatInfo.CurrentInfo), TestDateTimeNow, true),
            TestCase(Comparison.EqualTo, TestDateTimeNow.AddDays(1).ToString(DateTimeFormatInfo.CurrentInfo), TestDateTimeNow, false),
            TestCase(Comparison.EqualTo, "Text", TestDateTimeNow, false),
            TestCase(Comparison.EqualTo, true, true, true),
            TestCase(Comparison.EqualTo, Boolean.TrueString, true, true),
            TestCase(Comparison.EqualTo, Boolean.FalseString, true, false),
            TestCase(Comparison.EqualTo, "Text", true, false),
            TestCase(Comparison.EqualTo, 3.2343, 3.2343, true),
            TestCase(Comparison.EqualTo, (3.2343).ToString(NumberFormatInfo.CurrentInfo), 3.2343, true),
            TestCase(Comparison.EqualTo, (3).ToString(NumberFormatInfo.CurrentInfo), 3.2343, false),
            TestCase(Comparison.EqualTo, "Text", 3.2343, false),
            TestCase(Comparison.EqualTo, 3, 3, true),
            TestCase(Comparison.EqualTo, 3, 3.0000, true),
            TestCase(Comparison.EqualTo, 3, 3.2343, false),
            TestCase(Comparison.EqualTo, UInt16.MaxValue, UInt16.MaxValue, true),
            TestCase(Comparison.EqualTo, (UInt16)Int16.MaxValue, Int16.MaxValue, true),
            TestCase(Comparison.EqualTo, (UInt16)Int16.MaxValue, Int16.MinValue, false),
            TestCase(Comparison.EqualTo, UInt32.MaxValue, UInt32.MaxValue, true),
            TestCase(Comparison.EqualTo, (UInt32)Int32.MaxValue, Int32.MaxValue, true),
            TestCase(Comparison.EqualTo, (UInt32)Int32.MaxValue, Int32.MinValue, false),
            TestCase(Comparison.EqualTo, UInt64.MaxValue, UInt64.MaxValue, true),
            TestCase(Comparison.EqualTo, (UInt64)Int64.MaxValue, Int64.MaxValue, true),
            TestCase(Comparison.EqualTo, (UInt64)Int64.MaxValue, Int64.MinValue, false),
            TestCase(Comparison.EqualTo, Single.MaxValue, Single.MaxValue, true),
            TestCase(Comparison.EqualTo, Double.MaxValue, Double.MaxValue, true),
            TestCase(Comparison.EqualTo, TestObject1, TestObject1, true),
            TestCase(Comparison.EqualTo, TestObject1, TestObject2, false),
            TestCase(Comparison.Contains, new[] {1, 2, 3, 4, 5}, new[] {1, 2}, true),
            TestCase(Comparison.Contains, new[] {1, 2, 3, 4, 5}, new[] {6, 7}, false),
            TestCase(Comparison.Contains, new[] {1, 2, 3, 4, 5}, new[] {"1", "2"}, true),
            TestCase(Comparison.Contains, new[] {1, 2, 3, 4, 5}, new[] {"6", "7"}, false),
            TestCase(Comparison.Contains, new[] {1, 2, 3, 4, 5}, 2, true),
            TestCase(Comparison.Contains, new[] {1, 2, 3, 4, 5}, 7, false),
            TestCase(Comparison.Contains, new[] {1, 2, 3, 4, 5}, "2", true),
            TestCase(Comparison.Contains, new[] {1, 2, 3, 4, 5}, "7", false),
            TestCase(Comparison.Contains, "Text", "x", true),
            TestCase(Comparison.Contains, "Text", "s", false),
            TestCase(Comparison.Contains, "Text", 'x', true),
            TestCase(Comparison.Contains, "Text", 's', false),
            TestCase(Comparison.StartsWith, new[] {1, 2, 3, 4, 5}, new[] {1, 2}, true),
            TestCase(Comparison.StartsWith, new[] {1, 2, 3, 4, 5}, new[] {3, 4}, false),
            TestCase(Comparison.StartsWith, new[] {1, 2, 3, 4, 5}, new[] {"1", "2"}, true),
            TestCase(Comparison.StartsWith, new[] {1, 2, 3, 4, 5}, new[] {"3", "4"}, false),
            TestCase(Comparison.StartsWith, new[] {1, 2, 3, 4, 5}, 1, true),
            TestCase(Comparison.StartsWith, new[] {1, 2, 3, 4, 5}, 2, false),
            TestCase(Comparison.StartsWith, new[] {1, 2, 3, 4, 5}, "1", true),
            TestCase(Comparison.StartsWith, new[] {1, 2, 3, 4, 5}, "2", false),
            TestCase(Comparison.StartsWith, "Text", "T", true),
            TestCase(Comparison.StartsWith, "Text", "e", false),
            TestCase(Comparison.StartsWith, "Text", 'T', true),
            TestCase(Comparison.StartsWith, "Text", 'e', false),
            TestCase(Comparison.EndsWith, new[] {1, 2, 3, 4, 5}, new[] {4, 5}, true),
            TestCase(Comparison.EndsWith, new[] {1, 2, 3, 4, 5}, new[] {2, 3}, false),
            TestCase(Comparison.EndsWith, new[] {1, 2, 3, 4, 5}, new[] {"4", "5"}, true),
            TestCase(Comparison.EndsWith, new[] {1, 2, 3, 4, 5}, new[] {"2", "3"}, false),
            TestCase(Comparison.EndsWith, new[] {1, 2, 3, 4, 5}, 5, true),
            TestCase(Comparison.EndsWith, new[] {1, 2, 3, 4, 5}, 4, false),
            TestCase(Comparison.EndsWith, new[] {1, 2, 3, 4, 5}, "5", true),
            TestCase(Comparison.EndsWith, new[] {1, 2, 3, 4, 5}, "4", false),
            TestCase(Comparison.EndsWith, "Text", "t", true),
            TestCase(Comparison.EndsWith, "Text", "x", false),
            TestCase(Comparison.EndsWith, "Text", 't', true),
            TestCase(Comparison.EndsWith, "Text", 'x', false),
            TestCase(Comparison.LessThan, 5.2, 6, true),
            TestCase(Comparison.LessThan, -50, 247.432, true),
            TestCase(Comparison.LessThan, 5, 5.0, false),
            TestCase(Comparison.LessThan, 6, 5.2, false),
            TestCase(Comparison.LessThan, 247.432, -50, false),
            TestCase(Comparison.LessThanOrEqualTo, 5.2, 6, true),
            TestCase(Comparison.LessThanOrEqualTo, -50, 247.432, true),
            TestCase(Comparison.LessThanOrEqualTo, 5, 5.0, true),
            TestCase(Comparison.LessThanOrEqualTo, 6, 5.2, false),
            TestCase(Comparison.LessThanOrEqualTo, 247.432, -50, false),
            TestCase(Comparison.GreaterThan, 5.2, 6, false),
            TestCase(Comparison.GreaterThan, -50, 247.432, false),
            TestCase(Comparison.GreaterThan, 5, 5.0, false),
            TestCase(Comparison.GreaterThan, 6, 5.2, true),
            TestCase(Comparison.GreaterThan, 247.432, -50, true),
            TestCase(Comparison.GreaterThanOrEqualTo, 5.2, 6, false),
            TestCase(Comparison.GreaterThanOrEqualTo, -50, 247.432, false),
            TestCase(Comparison.GreaterThanOrEqualTo, 5, 5.0, true),
            TestCase(Comparison.GreaterThanOrEqualTo, 6, 5.2, true),
            TestCase(Comparison.GreaterThanOrEqualTo, 247.432, -50, true)
        };

        [TestMethod]
        public void ExecuteTestCases()
        {
            foreach (var testCase in TestCases)
            {
                if (testCase.Comparison.IsOperandOnlyComparison())
                {
                    Given<ComparisonCondition>()
                        .With(ANegationOperatorOf, NegationOperator.None)
                        .With(AComparisonOf, testCase.Comparison)
                        .With(AnOperandOf, testCase.Operand)
                        .Then(TheResultShouldBe, testCase.Result);
                    Given<ComparisonCondition>()
                        .With(ANegationOperatorOf, NegationOperator.Not)
                        .With(AComparisonOf, testCase.Comparison)
                        .With(AnOperandOf, testCase.Operand)
                        .Then(TheResultShouldBe, !testCase.Result);
                }
                else
                {
                    Given<ComparisonCondition>()
                        .With(ANegationOperatorOf, NegationOperator.None)
                        .With(AComparisonOf, testCase.Comparison)
                        .With(AnOperandOf, testCase.Operand)
                        .With(AValueOf, testCase.Value)
                        .Then(TheResultShouldBe, testCase.Result);
                    Given<ComparisonCondition>()
                        .With(ANegationOperatorOf, NegationOperator.Not)
                        .With(AComparisonOf, testCase.Comparison)
                        .With(AnOperandOf, testCase.Operand)
                        .With(AValueOf, testCase.Value)
                        .Then(TheResultShouldBe, !testCase.Result);
                }
                if (testCase.Comparison == Comparison.EqualTo)
                {
                    Given<ComparisonCondition>()
                        .With(ANegationOperatorOf, NegationOperator.None)
                        .With(AComparisonOf, testCase.Comparison)
                        .With(AnOperandOf, testCase.Value)
                        .With(AValueOf, testCase.Operand)
                        .Then(TheResultShouldBe, testCase.Result);
                    Given<ComparisonCondition>()
                        .With(ANegationOperatorOf, NegationOperator.Not)
                        .With(AComparisonOf, testCase.Comparison)
                        .With(AnOperandOf, testCase.Value)
                        .With(AValueOf, testCase.Operand)
                        .Then(TheResultShouldBe, !testCase.Result);
                }
            }
        }
    }
}