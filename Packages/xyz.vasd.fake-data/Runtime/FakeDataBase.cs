using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Xyz.Vasd.FakeData
{
    [AddComponentMenu("FakeData/" + nameof(FakeDatabase))]

    public class FakeDatabase : MonoBehaviour
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


        public List<T> GetList<T>()
        {
            return GetOrCreateList<T>(typeof(T));
        }

        public void AddToList<T>(T value)
        {
            var list = GetOrCreateList<T>(typeof(T));
            list.Add(value);
        }

        public void AddToList(Type type, object value)
        {
            var list = GetOrCreateList(type);
            list.Add(value);
        }

        private List<T> GetOrCreateList<T>(Type type)
        {
            if (!_lists.ContainsKey(type)) _lists[type] = new List<T>();
            return (List<T>)_lists[type];
        }

        private IList GetOrCreateList(Type type)
        {
            if (!_lists.ContainsKey(type)) 
            {
                Type genericListType = typeof(List<>).MakeGenericType(type);
                _lists[type] = (IList)Activator.CreateInstance(genericListType);
            }
            return (IList)_lists[type];
        }
    }
}
