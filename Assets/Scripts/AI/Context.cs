using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace AI
{
    public abstract class ContextBase
    {
        public string Key { get; protected set; }
        public abstract void RemoveOnChanged(Action<string, object> action);
    }

    public class Context<T> : ContextBase
    {
        public event Action<string, T> OnChanged;

        private T _value; // Backing field
        public T Value
        {
            get { return _value; }
            private set { SetValue(value); }
        }


        public Context(string key, T initialValue) : base()
        {
            Key = key;
            _value = initialValue; // Set the backing field directly
        }

        public void SetValue(T newValue)
        {
            if (EqualityComparer<T>.Default.Equals(_value, newValue)) return;

            _value = newValue; // Update the backing field
            OnChanged?.Invoke(Key, _value);

            UnityEngine.Debug.Log("A Value was set : " + _value);

            ContextManager.CreateOrUpdateContext(Key, _value);
        }

        public override void RemoveOnChanged(Action<string, object> action)
        {
            // Convert the action to the correct type and unsubscribe
            Action<string, T> typedAction = (k, v) => action(k, v);
            OnChanged -= typedAction;
        }
    }
}
