using System;
using System.Collections.Generic;
using UnityEngine;

namespace Xyz.Vasd.Fake
{
    // stores data
    public class Context : MonoBehaviour
    {
        private Dictionary<Type, object> _values;

        public bool Contains<T>()
        {
            return Contains(typeof(T));
        }
        public bool Contains(Type type)
        {
            return _values.ContainsKey(type);
        }

        public T Get<T>()
        {
            return (T)Get(typeof(T));
        }
        public object Get(Type type)
        {
            if (!_values.ContainsKey(type)) return null;
            return _values[type];
        }

        public void Set<T>(T value)
        {
            Set(typeof(T), value);
        }
        public void Set(Type type, object value)
        {
            _values[type] = value;
        }

        public void Remove<T>()
        {
            Remove(typeof(T));
        }
        public void Remove(Type type)
        {
            _values.Remove(type);
        }
    }
}
