using System;
using Flux.Conditions;
using Flux.Core.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Flux.Conditions
{
    [TestClass]
    public class TestingConditionGroup : TestBase<ConditionGroup>
    {
        public static readonly String ALogicalOperatorOf = "a LogicalOperator of";
        public static readonly String ANegationOperatorOf = "a NegationOperator of";
        public static readonly String NoConditions = "no Conditions";
        public static readonly String AConditionOf = "a Condition of";
        public static readonly String AConditionOfTrue = "a Condition of true";
        public static readonly String AConditionOfFalse = "a Condition of false";
        public static readonly String SetConditionResultToTrue = "set Condition Result to true";
        public static readonly String SetConditionResultToFalse = "set Condition Result to false";
        public static readonly String TheResultShouldBe = "the Result should be";

        public TestingConditionGroup()
            : base(RegisterSteps)
        {
        }

        private static void RegisterSteps(StepDefinitions stepDefinitions)
        {
            stepDefinitions
                .RegisterWith<LogicalOperator>(ALogicalOperatorOf, (c, arg) => c.LogicalOperator = arg)
                .RegisterWith<NegationOperator>(ANegationOperatorOf, (c, arg) => c.NegationOperator = arg)
                .RegisterWith(NoConditions)
                .RegisterWith<ICondition>(AConditionOf, (c, arg) => c.Add(arg))
                .RegisterWith<Boolean>(AConditionOf, (c, arg) => c.Add(new MockCondition(arg)))
                .RegisterWith(AConditionOfTrue, c => c.Add(new MockCondition(true)))
                .RegisterWith(AConditionOfFalse, c => c.Add(new MockCondition(false)))
                .RegisterWhen<MockCondition>(SetConditionResultToTrue, (c, arg) => arg.SetResult(true))
                .RegisterWhen<MockCondition>(SetConditionResultToFalse, (c, arg) => arg.SetResult(false))
                .RegisterThen<Boolean>(TheResultShouldBe, (c, arg, msg) => Assert.AreEqual(arg, c.Result, msg));
        }

        #region LogicalOperator.All Tests

        #region NegationOperator.None Tests

        [TestMethod]
        public void TestAllWithNoConditions()
        {
            Given<ConditionGroup>()
                .With(ALogicalOperatorOf, LogicalOperator.All)
                .With(ANegationOperatorOf, NegationOperator.None)
                .With(NoConditions)
                .Then(TheResultShouldBe, true);
        }

        [TestMethod]
        public void TestAllWithTwoFalseConditions()
        {
            Given<ConditionGroup>()
                .With(ALogicalOperatorOf, LogicalOperator.All)
                .With(ANegationOperatorOf, NegationOperator.None)
                .With(AConditionOfFalse)
                .With(AConditionOfFalse)
                .Then(TheResultShouldBe, false);
        }

        [TestMethod]
        public void TestAllWithTwoConditionsOneTrue()
        {
            Given<ConditionGroup>()
                .With(ALogicalOperatorOf, LogicalOperator.All)
                .With(ANegationOperatorOf, NegationOperator.None)
                .With(AConditionOfFalse)
                .With(AConditionOfTrue)
                .Then(TheResultShouldBe, false);
        }

        [TestMethod]
        public void TestAllWithTwoTrueConditions()
        {
            Given<ConditionGroup>()
                .With(ALogicalOperatorOf, LogicalOperator.All)
                .With(ANegationOperatorOf, NegationOperator.None)
                .With(AConditionOfTrue)
                .With(AConditionOfTrue)
                .Then(TheResultShouldBe, true);
        }

        [TestMethod]
        public void TestAllWithTwoTrueConditionsSettingOneToFalse()
        {
            MockCondition condition;
            Given<ConditionGroup>()
                .With(ALogicalOperatorOf, LogicalOperator.All)
                .With(ANegationOperatorOf, NegationOperator.None)
                .With(AConditionOfTrue)
                .With(AConditionOf, condition = new MockCondition(true))
                .When(SetConditionResultToFalse, condition)
                .Then(TheResultShouldBe, false);
        }

        [TestMethod]
        public void TestAllWithTwoConditionsOneTrueSettingItToFalse()
        {
            MockCondition condition;
            Given<ConditionGroup>()
                .With(ALogicalOperatorOf, LogicalOperator.All)
                .With(ANegationOperatorOf, NegationOperator.None)
                .With(AConditionOfFalse)
                .With(AConditionOf, condition = new MockCondition(true))
                .When(SetConditionResultToFalse, condition)
                .Then(TheResultShouldBe, false);
        }

        [TestMethod]
        public void TestAllWithTwoConditionsOneTrueSettingOtherToTrue()
        {
            MockCondition condition;
            Given<ConditionGroup>()
                .With(ALogicalOperatorOf, LogicalOperator.All)
                .With(ANegationOperatorOf, NegationOperator.None)
                .With(AConditionOfTrue)
                .With(AConditionOf, condition = new MockCondition(false))
                .When(SetConditionResultToTrue, condition)
                .Then(TheResultShouldBe, true);
        }

        #endregion

        #region NegationOperator.Not Tests

        [TestMethod]
        public void TestNotAllWithNoConditions()
        {
            Given<ConditionGroup>()
                .With(ALogicalOperatorOf, LogicalOperator.All)
                .With(ANegationOperatorOf, NegationOperator.Not)
                .With(NoConditions)
                .Then(TheResultShouldBe, false);
        }

        [TestMethod]
        public void TestNotAllWithTwoFalseConditions()
        {
            Given<ConditionGroup>()
                .With(ALogicalOperatorOf, LogicalOperator.All)
                .With(ANegationOperatorOf, NegationOperator.Not)
                .With(AConditionOfFalse)
                .With(AConditionOfFalse)
                .Then(TheResultShouldBe, true);
        }

        [TestMethod]
        public void TestNotAllWithTwoConditionsOneTrue()
        {
            Given<ConditionGroup>()
                .With(ALogicalOperatorOf, LogicalOperator.All)
                .With(ANegationOperatorOf, NegationOperator.Not)
                .With(AConditionOfFalse)
                .With(AConditionOfTrue)
                .Then(TheResultShouldBe, true);
        }

        [TestMethod]
        public void TestNotAllWithTwoTrueConditions()
        {
            Given<ConditionGroup>()
                .With(ALogicalOperatorOf, LogicalOperator.All)
                .With(ANegationOperatorOf, NegationOperator.Not)
                .With(AConditionOfTrue)
                .With(AConditionOfTrue)
                .Then(TheResultShouldBe, false);
        }

        [TestMethod]
        public void TestNotAllWithTwoTrueConditionsSettingOneToFalse()
        {
            MockCondition condition;
            Given<ConditionGroup>()
                .With(ALogicalOperatorOf, LogicalOperator.All)
                .With(ANegationOperatorOf, NegationOperator.Not)
                .With(AConditionOfTrue)
                .With(AConditionOf, condition = new MockCondition(true))
                .When(SetConditionResultToFalse, condition)
                .Then(TheResultShouldBe, true);
        }

        [TestMethod]
        public void TestNotAllWithTwoConditionsOneTrueSettingItToFalse()
        {
            MockCondition condition;
            Given<ConditionGroup>()
                .With(ALogicalOperatorOf, LogicalOperator.All)
                .With(ANegationOperatorOf, NegationOperator.Not)
                .With(AConditionOfFalse)
                .With(AConditionOf, condition = new MockCondition(true))
                .When(SetConditionResultToFalse, condition)
                .Then(TheResultShouldBe, true);
        }

        [TestMethod]
        public void TestNotAllWithTwoConditionsOneTrueSettingOtherToTrue()
        {
            MockCondition condition;
            Given<ConditionGroup>()
                .With(ALogicalOperatorOf, LogicalOperator.All)
                .With(ANegationOperatorOf, NegationOperator.Not)
                .With(AConditionOfTrue)
                .With(AConditionOf, condition = new MockCondition(false))
                .When(SetConditionResultToTrue, condition)
                .Then(TheResultShouldBe, false);
        }

        #endregion

        #endregion

        #region LogicalOperator.Any Tests

        #region NegationOperator.None Tests

        [TestMethod]
        public void TestAnyWithNoConditions()
        {
            Given<ConditionGroup>()
                .With(ALogicalOperatorOf, LogicalOperator.Any)
                .With(ANegationOperatorOf, NegationOperator.None)
                .With(NoConditions)
                .Then(TheResultShouldBe, false);
        }

        [TestMethod]
        public void TestAnyWithTwoFalseConditions()
        {
            Given<ConditionGroup>()
                .With(ALogicalOperatorOf, LogicalOperator.Any)
                .With(ANegationOperatorOf, NegationOperator.None)
                .With(AConditionOfFalse)
                .With(AConditionOfFalse)
                .Then(TheResultShouldBe, false);
        }

        [TestMethod]
        public void TestAnyWithTwoConditionsOneTrue()
        {
            Given<ConditionGroup>()
                .With(ALogicalOperatorOf, LogicalOperator.Any)
                .With(ANegationOperatorOf, NegationOperator.None)
                .With(AConditionOfFalse)
                .With(AConditionOfTrue)
                .Then(TheResultShouldBe, true);
        }

        [TestMethod]
        public void TestAnyWithTwoTrueConditions()
        {
            Given<ConditionGroup>()
                .With(ALogicalOperatorOf, LogicalOperator.Any)
                .With(ANegationOperatorOf, NegationOperator.None)
                .With(AConditionOfTrue)
                .With(AConditionOfTrue)
                .Then(TheResultShouldBe, true);
        }

        [TestMethod]
        public void TestAnyWithTwoTrueConditionsSettingOneToFalse()
        {
            MockCondition condition;
            Given<ConditionGroup>()
                .With(ALogicalOperatorOf, LogicalOperator.Any)
                .With(ANegationOperatorOf, NegationOperator.None)
                .With(AConditionOfTrue)
                .With(AConditionOf, condition = new MockCondition(true))
                .When(SetConditionResultToFalse, condition)
                .Then(TheResultShouldBe, true);
        }

        [TestMethod]
        public void TestAnyWithTwoConditionsOneTrueSettingItToFalse()
        {
            MockCondition condition;
            Given<ConditionGroup>()
                .With(ALogicalOperatorOf, LogicalOperator.Any)
                .With(ANegationOperatorOf, NegationOperator.None)
                .With(AConditionOfFalse)
                .With(AConditionOf, condition = new MockCondition(true))
                .When(SetConditionResultToFalse, condition)
                .Then(TheResultShouldBe, false);
        }

        [TestMethod]
        public void TestAnyWithTwoConditionsOneTrueSettingOtherToTrue()
        {
            MockCondition condition;
            Given<ConditionGroup>()
                .With(ALogicalOperatorOf, LogicalOperator.Any)
                .With(ANegationOperatorOf, NegationOperator.None)
                .With(AConditionOfTrue)
                .With(AConditionOf, condition = new MockCondition(false))
                .When(SetConditionResultToTrue, condition)
                .Then(TheResultShouldBe, true);
        }

        #endregion

        #region NegationOperator.Not Tests

        [TestMethod]
        public void TestNotAnyWithNoConditions()
        {
            Given<ConditionGroup>()
                .With(ALogicalOperatorOf, LogicalOperator.Any)
                .With(ANegationOperatorOf, NegationOperator.Not)
                .With(NoConditions)
                .Then(TheResultShouldBe, true);
        }

        [TestMethod]
        public void TestNotAnyWithTwoFalseConditions()
        {
            Given<ConditionGroup>()
                .With(ALogicalOperatorOf, LogicalOperator.Any)
                .With(ANegationOperatorOf, NegationOperator.Not)
                .With(AConditionOfFalse)
                .With(AConditionOfFalse)
                .Then(TheResultShouldBe, true);
        }

        [TestMethod]
        public void TestNotAnyWithTwoConditionsOneTrue()
        {
            Given<ConditionGroup>()
                .With(ALogicalOperatorOf, LogicalOperator.Any)
                .With(ANegationOperatorOf, NegationOperator.Not)
                .With(AConditionOfFalse)
                .With(AConditionOfTrue)
                .Then(TheResultShouldBe, false);
        }

        [TestMethod]
        public void TestNotAnyWithTwoTrueConditions()
        {
            Given<ConditionGroup>()
                .With(ALogicalOperatorOf, LogicalOperator.Any)
                .With(ANegationOperatorOf, NegationOperator.Not)
                .With(AConditionOfTrue)
                .With(AConditionOfTrue)
                .Then(TheResultShouldBe, false);
        }

        [TestMethod]
        public void TestNotAnyWithTwoTrueConditionsSettingOneToFalse()
        {
            MockCondition condition;
            Given<ConditionGroup>()
                .With(ALogicalOperatorOf, LogicalOperator.Any)
                .With(ANegationOperatorOf, NegationOperator.Not)
                .With(AConditionOfTrue)
                .With(AConditionOf, condition = new MockCondition(true))
                .When(SetConditionResultToFalse, condition)
                .Then(TheResultShouldBe, false);
        }

        [TestMethod]
        public void TestNotAnyWithTwoConditionsOneTrueSettingItToFalse()
        {
            MockCondition condition;
            Given<ConditionGroup>()
                .With(ALogicalOperatorOf, LogicalOperator.Any)
                .With(ANegationOperatorOf, NegationOperator.Not)
                .With(AConditionOfFalse)
                .With(AConditionOf, condition = new MockCondition(true))
                .When(SetConditionResultToFalse, condition)
                .Then(TheResultShouldBe, true);
        }

        [TestMethod]
        public void TestNotAnyWithTwoConditionsOneTrueSettingOtherToTrue()
        {
            MockCondition condition;
            Given<ConditionGroup>()
                .With(ALogicalOperatorOf, LogicalOperator.Any)
                .With(ANegationOperatorOf, NegationOperator.Not)
                .With(AConditionOfTrue)
                .With(AConditionOf, condition = new MockCondition(false))
                .When(SetConditionResultToTrue, condition)
                .Then(TheResultShouldBe, false);
        }

        #endregion

        #endregion

        #region LogicalOperator.None Tests

        #region NegationOperator.None Tests

        [TestMethod]
        public void TestNoneWithNoConditions()
        {
            Given<ConditionGroup>()
                .With(ALogicalOperatorOf, LogicalOperator.None)
                .With(ANegationOperatorOf, NegationOperator.None)
                .With(NoConditions)
                .Then(TheResultShouldBe, true);
        }

        [TestMethod]
        public void TestNoneWithTwoFalseConditions()
        {
            Given<ConditionGroup>()
                .With(ALogicalOperatorOf, LogicalOperator.None)
                .With(ANegationOperatorOf, NegationOperator.None)
                .With(AConditionOfFalse)
                .With(AConditionOfFalse)
                .Then(TheResultShouldBe, true);
        }

        [TestMethod]
        public void TestNoneWithTwoConditionsOneTrue()
        {
            Given<ConditionGroup>()
                .With(ALogicalOperatorOf, LogicalOperator.None)
                .With(ANegationOperatorOf, NegationOperator.None)
                .With(AConditionOfFalse)
                .With(AConditionOfTrue)
                .Then(TheResultShouldBe, false);
        }

        [TestMethod]
        public void TestNoneWithTwoTrueConditions()
        {
            Given<ConditionGroup>()
                .With(ALogicalOperatorOf, LogicalOperator.None)
                .With(ANegationOperatorOf, NegationOperator.None)
                .With(AConditionOfTrue)
                .With(AConditionOfTrue)
                .Then(TheResultShouldBe, false);
        }

        [TestMethod]
        public void TestNoneWithTwoTrueConditionsSettingOneToFalse()
        {
            MockCondition condition;
            Given<ConditionGroup>()
                .With(ALogicalOperatorOf, LogicalOperator.None)
                .With(ANegationOperatorOf, NegationOperator.None)
                .With(AConditionOfTrue)
                .With(AConditionOf, condition = new MockCondition(true))
                .When(SetConditionResultToFalse, condition)
                .Then(TheResultShouldBe, false);
        }

        [TestMethod]
        public void TestNoneWithTwoConditionsOneTrueSettingItToFalse()
        {
            MockCondition condition;
            Given<ConditionGroup>()
                .With(ALogicalOperatorOf, LogicalOperator.None)
                .With(ANegationOperatorOf, NegationOperator.None)
                .With(AConditionOfFalse)
                .With(AConditionOf, condition = new MockCondition(true))
                .When(SetConditionResultToFalse, condition)
                .Then(TheResultShouldBe, true);
        }

        [TestMethod]
        public void TestNoneWithTwoConditionsOneTrueSettingOtherToTrue()
        {
            MockCondition condition;
            Given<ConditionGroup>()
                .With(ALogicalOperatorOf, LogicalOperator.None)
                .With(ANegationOperatorOf, NegationOperator.None)
                .With(AConditionOfTrue)
                .With(AConditionOf, condition = new MockCondition(false))
                .When(SetConditionResultToTrue, condition)
                .Then(TheResultShouldBe, false);
        }

        #endregion

        #region NegationOperator.Not Tests

        [TestMethod]
        public void TestNotNoneWithNoConditions()
        {
            Given<ConditionGroup>()
                .With(ALogicalOperatorOf, LogicalOperator.None)
                .With(ANegationOperatorOf, NegationOperator.Not)
                .With(NoConditions)
                .Then(TheResultShouldBe, false);
        }

        [TestMethod]
        public void TestNotNoneWithTwoFalseConditions()
        {
            Given<ConditionGroup>()
                .With(ALogicalOperatorOf, LogicalOperator.None)
                .With(ANegationOperatorOf, NegationOperator.Not)
                .With(AConditionOfFalse)
                .With(AConditionOfFalse)
                .Then(TheResultShouldBe, false);
        }

        [TestMethod]
        public void TestNotNoneWithTwoConditionsOneTrue()
        {
            Given<ConditionGroup>()
                .With(ALogicalOperatorOf, LogicalOperator.None)
                .With(ANegationOperatorOf, NegationOperator.Not)
                .With(AConditionOfFalse)
                .With(AConditionOfTrue)
                .Then(TheResultShouldBe, true);
        }

        [TestMethod]
        public void TestNotNoneWithTwoTrueConditions()
        {
            Given<ConditionGroup>()
                .With(ALogicalOperatorOf, LogicalOperator.None)
                .With(ANegationOperatorOf, NegationOperator.Not)
                .With(AConditionOfTrue)
                .With(AConditionOfTrue)
                .Then(TheResultShouldBe, true);
        }

        [TestMethod]
        public void TestNotNoneWithTwoTrueConditionsSettingOneToFalse()
        {
            MockCondition condition;
            Given<ConditionGroup>()
                .With(ALogicalOperatorOf, LogicalOperator.None)
                .With(ANegationOperatorOf, NegationOperator.Not)
                .With(AConditionOfTrue)
                .With(AConditionOf, condition = new MockCondition(true))
                .When(SetConditionResultToFalse, condition)
                .Then(TheResultShouldBe, true);
        }

        [TestMethod]
        public void TestNotNoneWithTwoConditionsOneTrueSettingItToFalse()
        {
            MockCondition condition;
            Given<ConditionGroup>()
                .With(ALogicalOperatorOf, LogicalOperator.None)
                .With(ANegationOperatorOf, NegationOperator.Not)
                .With(AConditionOfFalse)
                .With(AConditionOf, condition = new MockCondition(true))
                .When(SetConditionResultToFalse, condition)
                .Then(TheResultShouldBe, false);
        }

        [TestMethod]
        public void TestNotNoneWithTwoConditionsOneTrueSettingOtherToTrue()
        {
            MockCondition condition;
            Given<ConditionGroup>()
                .With(ALogicalOperatorOf, LogicalOperator.None)
                .With(ANegationOperatorOf, NegationOperator.Not)
                .With(AConditionOfTrue)
                .With(AConditionOf, condition = new MockCondition(false))
                .When(SetConditionResultToTrue, condition)
                .Then(TheResultShouldBe, true);
        }

        #endregion

        #endregion

        #region LogicalOperator.One Tests

        #region NegationOperator.None Tests

        [TestMethod]
        public void TestOneWithNoConditions()
        {
            Given<ConditionGroup>()
                .With(ALogicalOperatorOf, LogicalOperator.One)
                .With(ANegationOperatorOf, NegationOperator.None)
                .With(NoConditions)
                .Then(TheResultShouldBe, false);
        }

        [TestMethod]
        public void TestOneWithTwoFalseConditions()
        {
            Given<ConditionGroup>()
                .With(ALogicalOperatorOf, LogicalOperator.One)
                .With(ANegationOperatorOf, NegationOperator.None)
                .With(AConditionOfFalse)
                .With(AConditionOfFalse)
                .Then(TheResultShouldBe, false);
        }

        [TestMethod]
        public void TestOneWithTwoConditionsOneTrue()
        {
            Given<ConditionGroup>()
                .With(ALogicalOperatorOf, LogicalOperator.One)
                .With(ANegationOperatorOf, NegationOperator.None)
                .With(AConditionOfFalse)
                .With(AConditionOfTrue)
                .Then(TheResultShouldBe, true);
        }

        [TestMethod]
        public void TestOneWithTwoTrueConditions()
        {
            Given<ConditionGroup>()
                .With(ALogicalOperatorOf, LogicalOperator.One)
                .With(ANegationOperatorOf, NegationOperator.None)
                .With(AConditionOfTrue)
                .With(AConditionOfTrue)
                .Then(TheResultShouldBe, false);
        }

        [TestMethod]
        public void TestOneWithTwoTrueConditionsSettingOneToFalse()
        {
            MockCondition condition;
            Given<ConditionGroup>()
                .With(ALogicalOperatorOf, LogicalOperator.One)
                .With(ANegationOperatorOf, NegationOperator.None)
                .With(AConditionOfTrue)
                .With(AConditionOf, condition = new MockCondition(true))
                .When(SetConditionResultToFalse, condition)
                .Then(TheResultShouldBe, true);
        }

        [TestMethod]
        public void TestOneWithTwoConditionsOneTrueSettingItToFalse()
        {
            MockCondition condition;
            Given<ConditionGroup>()
                .With(ALogicalOperatorOf, LogicalOperator.One)
                .With(ANegationOperatorOf, NegationOperator.None)
                .With(AConditionOfFalse)
                .With(AConditionOf, condition = new MockCondition(true))
                .When(SetConditionResultToFalse, condition)
                .Then(TheResultShouldBe, false);
        }

        [TestMethod]
        public void TestOneWithTwoConditionsOneTrueSettingOtherToTrue()
        {
            MockCondition condition;
            Given<ConditionGroup>()
                .With(ALogicalOperatorOf, LogicalOperator.One)
                .With(ANegationOperatorOf, NegationOperator.None)
                .With(AConditionOfTrue)
                .With(AConditionOf, condition = new MockCondition(false))
                .When(SetConditionResultToTrue, condition)
                .Then(TheResultShouldBe, false);
        }

        #endregion

        #region NegationOperator.Not Tests

        [TestMethod]
        public void TestNotOneWithNoConditions()
        {
            Given<ConditionGroup>()
                .With(ALogicalOperatorOf, LogicalOperator.One)
                .With(ANegationOperatorOf, NegationOperator.Not)
                .With(NoConditions)
                .Then(TheResultShouldBe, true);
        }

        [TestMethod]
        public void TestNotOneWithTwoFalseConditions()
        {
            Given<ConditionGroup>()
                .With(ALogicalOperatorOf, LogicalOperator.One)
                .With(ANegationOperatorOf, NegationOperator.Not)
                .With(AConditionOfFalse)
                .With(AConditionOfFalse)
                .Then(TheResultShouldBe, true);
        }

        [TestMethod]
        public void TestNotOneWithTwoConditionsOneTrue()
        {
            Given<ConditionGroup>()
                .With(ALogicalOperatorOf, LogicalOperator.One)
                .With(ANegationOperatorOf, NegationOperator.Not)
                .With(AConditionOfFalse)
                .With(AConditionOfTrue)
                .Then(TheResultShouldBe, false);
        }

        [TestMethod]
        public void TestNotOneWithTwoTrueConditions()
        {
            Given<ConditionGroup>()
                .With(ALogicalOperatorOf, LogicalOperator.One)
                .With(ANegationOperatorOf, NegationOperator.Not)
                .With(AConditionOfTrue)
                .With(AConditionOfTrue)
                .Then(TheResultShouldBe, true);
        }

        [TestMethod]
        public void TestNotOneWithTwoTrueConditionsSettingOneToFalse()
        {
            MockCondition condition;
            Given<ConditionGroup>()
                .With(ALogicalOperatorOf, LogicalOperator.One)
                .With(ANegationOperatorOf, NegationOperator.Not)
                .With(AConditionOfTrue)
                .With(AConditionOf, condition = new MockCondition(true))
                .When(SetConditionResultToFalse, condition)
                .Then(TheResultShouldBe, false);
        }

        [TestMethod]
        public void TestNotOneWithTwoConditionsOneTrueSettingItToFalse()
        {
            MockCondition condition;
            Given<ConditionGroup>()
                .With(ALogicalOperatorOf, LogicalOperator.One)
                .With(ANegationOperatorOf, NegationOperator.Not)
                .With(AConditionOfFalse)
                .With(AConditionOf, condition = new MockCondition(true))
                .When(SetConditionResultToFalse, condition)
                .Then(TheResultShouldBe, true);
        }

        [TestMethod]
        public void TestNotOneWithTwoConditionsOneTrueSettingOtherToTrue()
        {
            MockCondition condition;
            Given<ConditionGroup>()
                .With(ALogicalOperatorOf, LogicalOperator.One)
                .With(ANegationOperatorOf, NegationOperator.Not)
                .With(AConditionOfTrue)
                .With(AConditionOf, condition = new MockCondition(false))
                .When(SetConditionResultToTrue, condition)
                .Then(TheResultShouldBe, true);
        }

        #endregion

        #endregion
    }
}