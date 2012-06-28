using System;

namespace Flux.Conditions
{
    public abstract class Condition : ICondition
    {
        protected Condition() : this(NegationOperator.None) { }

        protected Condition(NegationOperator negationOperator)
        {
            _negationOperator = negationOperator;
            SetResult(false);
        }

        private NegationOperator _negationOperator;
        public NegationOperator NegationOperator
        {
            get { return _negationOperator; }
            set
            {
                if (_negationOperator != value)
                {
                    _negationOperator = value;
                    SetResult(_result);
                }
            }
        }

        private Boolean _result;
        protected void SetResult(Boolean result)
        {
            switch (_negationOperator)
            {
                case NegationOperator.None:
                    InternalSetResult(result);
                    break;
                case NegationOperator.Not:
                    InternalSetResult(!result);
                    break;
            }
        }

        private void InternalSetResult(Boolean result)
        {
            if (_result == result)
                return;
            
            _result = result;
            var onResultChanged = OnResultChanged;
            if (onResultChanged != null)
            {
                onResultChanged(this, EventArgs.Empty);
            }
        }

        protected virtual Boolean GetResult()
        {
            return _result;
        }

        public bool Result
        {
            get { return GetResult(); }
        }

        public event EventHandler OnResultChanged;
    }
}
