using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Flux.Core;

namespace Flux.Conditions
{
    public class ComparisonCondition : Condition, IComparisonCondition
    {
        private Boolean _active;

        public Boolean Active
        {
            get { return _active; }
            set
            {
                if (_active == value)
                    return;

                _active = value;
                ProcessConditions();
            }
        }

        private Object _operand;

        public Object Operand
        {
            get { return _operand; }
            set
            {
                if (_operand == value)
                    return;

                _operand = value;
                ProcessConditions();
            }
        }

        private Comparison _comparison = Comparison.EqualTo;

        public Comparison Comparison
        {
            get { return _comparison; }
            set
            {
                if (_comparison == value)
                    return;

                _comparison = value;
                ProcessConditions();
            }
        }

        private Object _value;

        public Object Value
        {
            get { return _value; }
            set
            {
                if (_value == value)
                    return;

                _value = value;
                ProcessConditions();
            }
        }

        protected override bool GetResult()
        {
            Active = true;
            return base.GetResult();
        }

        private void ProcessConditions()
        {
            if (!_active)
                return;

            var result = default(Boolean?);
            if ((Operand is IEnumerable) &&
                !(Operand is String))
            {
                var operands = (Operand as IEnumerable).Cast<Object>().ToArray();
                if (!operands.Any())
                {
                    result = (Comparison == Comparison.Empty);
                }
                else
                {
                    if ((Value is IEnumerable) &&
                        !(Value is String))
                    {
                        var values = (Value as IEnumerable).Cast<Object>().ToArray();
                        switch (Comparison)
                        {
                            case Comparison.Contains:
                                result = operands.Contains(values,
                                                           (operand, value) =>
                                                           CompareValue(Comparison.EqualTo, operand, value));
                                break;
                            case Comparison.EndsWith:
                                result = operands.EndsWith(values,
                                                           (operand, value) =>
                                                           CompareValue(Comparison.EqualTo, operand, value));
                                break;
                            case Comparison.StartsWith:
                                result = operands.StartsWith(values,
                                                             (operand, value) =>
                                                             CompareValue(Comparison.EqualTo, operand, value));
                                break;
                        }
                    }
                    else
                    {
                        switch (Comparison)
                        {
                            case Comparison.Contains:
                                result = operands.Any(operand => CompareValue(Comparison.EqualTo, operand, Value));
                                break;
                            case Comparison.EndsWith:
                                result = CompareValue(Comparison.EqualTo, operands.Last(), Value);
                                break;
                            case Comparison.StartsWith:
                                result = CompareValue(Comparison.EqualTo, operands.First(), Value);
                                break;
                        }
                    }
                }
            }
            if (!result.HasValue)
            {
                switch (Comparison)
                {
                    case Comparison.Null:
                        result = (Operand == null);
                        break;
                    default:
                        result = CompareValue(Comparison, Operand, Value);
                        break;
                }
            }
            SetResult(result.GetValueOrDefault());
        }

        private bool CompareValue(Comparison comparison, Object operand, Object value)
        {
            if (operand == null)
            {
                return (comparison.ContainedWithin(Comparison.Null, Comparison.NullOrEmpty) ||
                        ((comparison == Comparison.EqualTo) &&
                         (value == null)));
            }
            if (!comparison.IsOperandOnlyComparison())
            {
                if (value == null)
                {
                    return false;
                }
                GetComparisonValues(ref operand, ref value);
            }
            var stringOperand = operand as string;
            if (stringOperand != null)
            {
                var typedValue = Convert.ToString(value);
                return CompareString(comparison, stringOperand, typedValue);
            }
            var comparableOperand = operand as IComparable;
            if (comparableOperand != null)
            {
                var type = comparableOperand.GetType();
                var typedValue = (IComparable)Convert.ChangeType(value, type);
                return CompareComparable(comparison, comparableOperand, typedValue);
            }
            return comparison == Comparison.EqualTo && Equals(operand, value);
        }

        private static bool CompareComparable(Comparison comparison, IComparable operand, IComparable value)
        {
            switch (comparison)
            {
                case Comparison.EqualTo:
                    return operand.CompareTo(value) == 0;
                case Comparison.LessThan:
                    return operand.CompareTo(value) < 0;
                case Comparison.LessThanOrEqualTo:
                    return operand.CompareTo(value) <= 0;
                case Comparison.GreaterThan:
                    return operand.CompareTo(value) > 0;
                case Comparison.GreaterThanOrEqualTo:
                    return operand.CompareTo(value) >= 0;
            }
            return false;
        }

        private static void GetComparisonValues(ref Object operand, ref Object value)
        {
            var operandType = operand.GetType();
            var valueType = value.GetType();
            if (operandType == valueType)
                return;

            WidenNumeric(ref operand, ref operandType);
            WidenNumeric(ref value, ref valueType);
            if (operandType == valueType)
                return;

            ConvertToNumeric(ref value, ref valueType, operandType);
            ConvertToNumeric(ref operand, ref operandType, valueType);
            if (operandType == valueType)
                return;

            var convertedValue = ConvertToDateTime(ref value, ref valueType, operandType);
            var convertedOperand = ConvertToDateTime(ref operand, ref operandType, valueType);
            if (operandType == valueType)
            {
                if (convertedValue)
                    NarrowDateTime(ref operand, value);
                if (convertedOperand)
                    NarrowDateTime(ref value, operand);

                return;
            }

            if (valueType == typeof(String))
            {
                operand = Convert.ToString(operand);
            }
            else if (operandType == typeof(String))
            {
                value = Convert.ToString(value);
            }
        }

        private static void NarrowDateTime(ref Object value, Object targetPrecision)
        {
            var dateTime = (DateTime)value;
            var dateTimePrecision = (DateTime)targetPrecision;

            var tickDifference = dateTime.Ticks - dateTimePrecision.Ticks;
            if ((tickDifference > 0) && (tickDifference < 10000000))
                value = dateTime.AddTicks(-tickDifference);
        }

        private static Boolean ConvertToDateTime(ref object value, ref Type valueType, Type targetType)
        {
            if ((valueType == typeof(String)) &&
                (targetType == typeof(DateTime)))
            {
                DateTime doubleValue;
                if (DateTime.TryParse(value as String, out doubleValue))
                {
                    value = doubleValue;
                    valueType = targetType;
                    return true;
                }
            }
            return false;
        }

        private static void ConvertToNumeric(ref object value, ref Type valueType, Type targetType)
        {
            if ((valueType == typeof(String)) &&
                (targetType == typeof(Double)))
            {
                Double doubleValue;
                if (Double.TryParse(value as String, out doubleValue))
                {
                    value = doubleValue;
                    valueType = targetType;
                }
            }
        }

        private static void WidenNumeric(ref Object value, ref Type valueType)
        {
            if (!valueType.IsNumericType() ||
                (valueType == typeof(Double)))
                return;

            value = TypeHelper.Convert<Double>(value);
            valueType = typeof(Double);
        }

        private static Boolean CompareString(Comparison comparison, String operand, String value)
        {
            switch (comparison)
            {
                case Comparison.Empty:
                case Comparison.NullOrEmpty:
                    return String.IsNullOrEmpty(operand);
                case Comparison.EqualTo:
                    return String.Compare(operand, value, StringComparison.OrdinalIgnoreCase) == 0;
                case Comparison.Contains:
                    return operand.Contains(value);
                case Comparison.StartsWith:
                    return operand.StartsWith(value);
                case Comparison.EndsWith:
                    return operand.EndsWith(value);
                case Comparison.GreaterThan:
                    return String.Compare(operand, value, StringComparison.OrdinalIgnoreCase) > 0;
                case Comparison.GreaterThanOrEqualTo:
                    return String.Compare(operand, value, StringComparison.OrdinalIgnoreCase) >= 0;
                case Comparison.LessThan:
                    return String.Compare(operand, value, StringComparison.OrdinalIgnoreCase) < 0;
                case Comparison.LessThanOrEqualTo:
                    return String.Compare(operand, value, StringComparison.OrdinalIgnoreCase) <= 0;
            }
            return false;
        }
    }
}