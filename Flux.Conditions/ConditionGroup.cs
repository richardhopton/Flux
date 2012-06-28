using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Flux.Core;

namespace Flux.Conditions
{
    public class ConditionGroup : Condition, IConditionGroup
    {
        public ConditionGroup()
        {
            _conditions = new List<ICondition>();
            SetResult(_trueCount);
        }

        private readonly List<ICondition> _conditions;
        private Int32 _trueCount;
        private LogicalOperator _logicalOperator = LogicalOperator.All;

        public LogicalOperator LogicalOperator
        {
            get { return _logicalOperator; }
            set
            {
                if (_logicalOperator == value)
                    return;
                
                _logicalOperator = value;
                SetResult(_trueCount);
            }
        }

        private void SetResult(Int32 trueCount)
        {
            switch (_logicalOperator)
            {
                case LogicalOperator.None:
                    SetResult(trueCount == 0);
                    break;
                case LogicalOperator.One:
                    SetResult(trueCount == 1);
                    break;
                case LogicalOperator.Any:
                    SetResult(trueCount >= 1);
                    break;
                case LogicalOperator.All:
                    SetResult(trueCount == _conditions.Count);
                    break;

            }
        }

        private void Subscribe_OnResultChanged(ICondition condition)
        {
            condition.OnResultChanged += condition_OnResultChanged;
            var trueCount = _trueCount;
            if (condition.Result)
            {
                trueCount = Interlocked.Increment(ref _trueCount);
            }
            SetResult(trueCount);
        }

        private void Unsubscribe_OnResultChanged(ICondition condition)
        {
            condition.OnResultChanged -= condition_OnResultChanged;
            var trueCount = _trueCount;
            if (condition.Result)
            {
                trueCount = Interlocked.Decrement(ref _trueCount);
            }
            SetResult(trueCount);
        }

        private void condition_OnResultChanged(object sender, EventArgs e)
        {
            var condition = sender as ICondition;
            if (condition == null)
                return;

            var trueCount = condition.Result
                                ? Interlocked.Increment(ref _trueCount)
                                : Interlocked.Decrement(ref _trueCount);
            SetResult(trueCount);
        }

        public int IndexOf(ICondition item)
        {
            Requires.NotNull(item, "item");
            return _conditions.IndexOf(item);
        }

        public void Insert(int index, ICondition item)
        {
            Requires.NotNull(item, "item");
            _conditions.Insert(index, item);
            Subscribe_OnResultChanged(item);
        }

        public void RemoveAt(int index)
        {
            var item = _conditions[index];
            Remove(item);
        }

        public ICondition this[int index]
        {
            get { return _conditions[index]; }
            set
            {
                Requires.NotNull(value, "value");
                var item = _conditions[index];
                Unsubscribe_OnResultChanged(item);
                Subscribe_OnResultChanged(value);
                _conditions[index] = value;
            }
        }

        public void Add(ICondition item)
        {
            Requires.NotNull(item, "item");
            _conditions.Add(item);
            Subscribe_OnResultChanged(item);
        }

        public void Clear()
        {
            foreach (var condition in _conditions)
            {
                Subscribe_OnResultChanged(condition);
            }
            _conditions.Clear();
            SetResult(0);
        }

        public bool Contains(ICondition item)
        {
            if (item == null)
            {
                return false;
            }
            return _conditions.Contains(item);
        }

        public void CopyTo(ICondition[] array, int arrayIndex)
        {
            _conditions.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return _conditions.Count; }
        }

        bool ICollection<ICondition>.IsReadOnly
        {
            get { return ((ICollection<ICondition>)_conditions).IsReadOnly; }
        }

        public bool Remove(ICondition item)
        {
            if (_conditions.Remove(item))
            {
                Unsubscribe_OnResultChanged(item);
                return true;
            }
            return false;
        }

        public IEnumerator<ICondition> GetEnumerator()
        {
            return _conditions.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _conditions.GetEnumerator();
        }
    }
}