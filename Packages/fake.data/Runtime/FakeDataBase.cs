using System;
using System.Collections.Generic;
using UnityEngine;

namespace Xyz.Vasd.FakeData
{
    [AddComponentMenu("FakeData/" + nameof(FakeDataBase))]

    public class FakeDataBase : MonoBehaviour
    {
        private Dictionary<Type, object> _singletons = new();
        private Dictionary<Type, object> _lists = new();

        public T GetSingleton<T>() where T : class, new()
        {
            var type = typeof (T);
            if (_singletons.ContainsKey(type)) return (T)_singletons[type];

            var singleton = new T();
            _singletons[type] = singleton;

            return singleton;
        }

        public void AddToList<T>(T value) where T : class, new()
        {
            var list = GetOrCreateList<T>(typeof(T));
            list.Add(value);            
        }

        public List<T> GetList<T>()
        {
            return GetOrCreateList<T>(typeof(T));
        }

        private List<T> GetOrCreateList<T>(Type type)
        {
            if (!_lists.ContainsKey(type)) _lists[type] = new List<T>();
            return (List<T>)_lists[type];
        }
    }
}
