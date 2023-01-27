using System;
using System.Collections.Generic;
using UnityEngine;
using Xyz.Vasd.Fake.Database;

namespace Xyz.Vasd.Fake
{
    // stores data
    [AddComponentMenu("Fake/Fake Context")]
    public class FakeContext : MonoBehaviour
    {
        public FakeDatabase DB { get; private set; }

        internal Dictionary<Type, object> Values;

        private void Awake()
        {
            DB = new FakeDatabase();
            Values = new Dictionary<Type, object>();
        }

        public bool Contains<T>()
        {
            return Contains(typeof(T));
        }
        public bool Contains(Type type)
        {
            return Values.ContainsKey(type);
        }

        public T Get<T>()
        {
            return (T)Get(typeof(T));
        }
        public object Get(Type type)
        {
            if (!Values.ContainsKey(type)) return null;
            return Values[type];
        }

        public void Set<T>(T value)
        {
            Set(typeof(T), value);
        }
        public void Set(Type type, object value)
        {
            Values[type] = value;
        }

        public void Remove<T>()
        {
            Remove(typeof(T));
        }
        public void Remove(Type type)
        {
            Values.Remove(type);
        }
    }
}
