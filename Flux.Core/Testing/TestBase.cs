using System;
using System.Collections.Generic;
using System.Linq;

namespace Flux.Core.Testing
{
    public abstract class TestBase<T>
    {
        protected class StepKey
        {
            public String Name { get; private set; }
            public Type Type { get; private set; }

            public StepKey(String name, Type type)
            {
                Name = name;
                Type = type;
            }
        }

        protected class StepDefinitions
        {
            private readonly Dictionary<String, Func<T>> _givenList;
            private readonly Dictionary<StepKey, Action<T, Object>> _withList;
            private readonly Dictionary<StepKey, Action<T, Object>> _whenList;
            private readonly Dictionary<StepKey, Action<T, Object>> _thenList;

            public StepDefinitions()
            {
                _givenList = new Dictionary<String, Func<T>>();
                _withList = new Dictionary<StepKey, Action<T, Object>>();
                _whenList = new Dictionary<StepKey, Action<T, Object>>();
                _thenList = new Dictionary<StepKey, Action<T, Object>>();
            }

            internal Dictionary<String, Func<T>> Given
            {
                get { return _givenList; }
            }

            internal Dictionary<StepKey, Action<T, Object>> With
            {
                get { return _withList; }
            }

            internal Dictionary<StepKey, Action<T, Object>> When
            {
                get { return _whenList; }
            }

            internal Dictionary<StepKey, Action<T, Object>> Then
            {
                get { return _thenList; }
            }

            public StepDefinitions RegisterGiven(String name, Func<T> func)
            {
                _givenList.Add(name, func);
                return this;
            }

            public StepDefinitions RegisterWith(String name)
            {
                var key = new StepKey(name, null);
                _withList.Add(key, null);
                return this;
            }

            public StepDefinitions RegisterWith(String name, Action<T> action)
            {
                Action<T, Object> actionObject = (a, arg) => action(a);
                var key = new StepKey(name, null);
                _withList.Add(key, actionObject);
                return this;
            }

            public StepDefinitions RegisterWith<TArg>(String name, Action<T, TArg> action)
            {
                Action<T, Object> actionObject = (a, arg) => action(a, (TArg)arg);
                var key = new StepKey(name, typeof(TArg));
                _withList.Add(key, actionObject);
                return this;
            }

            public StepDefinitions RegisterWhen(String name)
            {
                var key = new StepKey(name, null);
                _whenList.Add(key, null);
                return this;
            }

            public StepDefinitions RegisterWhen(String name, Action<T> action)
            {
                Action<T, Object> actionObject = (a, arg) => action(a);
                var key = new StepKey(name, null);
                _whenList.Add(key, actionObject);
                return this;
            }

            public StepDefinitions RegisterWhen<TArg>(String name, Action<T, TArg> action)
            {
                Action<T, Object> actionObject = (a, arg) => action(a, (TArg)arg);
                var key = new StepKey(name, typeof(TArg));
                _whenList.Add(key, actionObject);
                return this;
            }

            public StepDefinitions RegisterThen<TArg>(String name, Action<T, TArg, String> action)
            {
                Action<T, Object> actionObject = (a, arg) =>
                {
                    var whenThenArg = (ThenArg)arg;
                    action(a, (TArg)whenThenArg.Arg, whenThenArg.Message);
                };
                var key = new StepKey(name, typeof(TArg));
                _thenList.Add(key, actionObject);
                return this;
            }
        }

        private class ThenArg
        {
            public String Message { get; private set; }
            public Object Arg { get; private set; }

            public ThenArg(Object arg, String message)
            {
                Arg = arg;
                Message = message;
            }
        }

        protected abstract class WhenThenHandler
        {
            private readonly Dictionary<StepKey, Action<T, Object>> _thenList;
            private readonly Dictionary<StepKey, Action<T, Object>> _whenList;

            private readonly T _context;

            protected T Context
            {
                get { return _context; }
            }

            private readonly List<String> _log = new List<String>();

            internal void Log(String method, String name, Object value)
            {
                if (name.EndsWith(" be", StringComparison.OrdinalIgnoreCase) ||
                    name.EndsWith(" of", StringComparison.OrdinalIgnoreCase) ||
                    name.EndsWith(" to", StringComparison.OrdinalIgnoreCase))
                {
                    if (value == null)
                    {
                        value = "null";
                    }
                    if ((value as String) == String.Empty)
                    {
                        value = "String.Empty";
                    }
                    _log.Add(String.Format("{0} {1} {2}", method, name, value).TrimEnd());
                }
                else
                {
                    _log.Add(String.Format("{0} {1}", method, name).TrimEnd());
                }
            }

            internal void Log(String method, String name)
            {
                Log(method, name, String.Empty);
            }

            protected WhenThenHandler(Dictionary<StepKey, Action<T, Object>> whenList,
                                     Dictionary<StepKey, Action<T, Object>> thenList,
                                     T context)
            {
                _whenList = whenList;
                _thenList = thenList;
                _context = context;
            }

            protected void InvokeAction(String name,
                                       Object value,
                                       String method,
                                       Dictionary<StepKey, Action<T, Object>> actionList)
            {
                var key = FindKey(name, value, actionList);
                if (key == null)
                    throw new ArgumentException(String.Format("name \"{0}\" not found", name));

                Log(method, name, value);
                Action<T, Object> action;
                if (!actionList.TryGetValue(key, out action) || (action == null))
                    return;

                action(Context, method == "Then" ? new ThenArg(value, String.Join(", ", _log)) : value);
            }

            private static StepKey FindKey(String name, Object value, Dictionary<StepKey, Action<T, Object>> actionList)
            {
                var valueType = value == null ? null : value.GetType();
                var keys = actionList.Keys.Where(x => x.Name == name).ToArray();

                var exactKey = keys.FirstOrDefault(x => x.Type == valueType);
                if (exactKey != null)
                {
                    return exactKey;
                }

                foreach (var key in keys)
                {
                    var keyType = key.Type;
                    if (keyType == null)
                        continue;

                    if (value != null)
                    {
                        if (keyType.IsInstanceOfType(value))
                        {
                            return key;
                        }
                    }
                    else
                    {
                        if (!keyType.IsValueType ||
                            (keyType.IsGenericType &&
                             keyType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                        {
                            return key;
                        }
                    }
                }
                return null;
            }

            public WhenThenHandler Then(String name, Object arg)
            {
                InvokeAction(name, arg, "Then", _thenList);
                return this;
            }

            public WhenThenHandler When(String name)
            {
                return When(name, null);
            }

            public WhenThenHandler When(String name, Object arg)
            {
                InvokeAction(name, arg, "When", _whenList);
                return this;
            }
        }

        protected sealed class WithHandler : WhenThenHandler
        {
            private readonly Dictionary<StepKey, Action<T, Object>> _withList;

            internal WithHandler(StepDefinitions stepDefinitions, T context)
                : base(stepDefinitions.When, stepDefinitions.Then, context)
            {
                _withList = stepDefinitions.With;
            }

            public WithHandler With(String name, Object arg = null)
            {
                InvokeAction(name, arg, "With", _withList);
                return this;
            }
        }

        private readonly StepDefinitions _stepDefinitions = new StepDefinitions();

        protected WithHandler Given(String name)
        {
            Func<T> func;
            if (!_stepDefinitions.Given.TryGetValue(name, out func))
                throw new ArgumentException(String.Format("name \"{0}\" not found", name));

            var context = func();
            var with = new WithHandler(_stepDefinitions, context);
            with.Log("Given", name);
            return with;
        }

        protected WithHandler Given<TNew>()
            where TNew : T, new()
        {
            var context = new TNew();
            var with = new WithHandler(_stepDefinitions, context);
            with.Log("Given a new", typeof(TNew).Name);
            return with;
        }

        protected TestBase(Action<StepDefinitions> registerSteps)
        {
            registerSteps(_stepDefinitions);
        }
    }
}