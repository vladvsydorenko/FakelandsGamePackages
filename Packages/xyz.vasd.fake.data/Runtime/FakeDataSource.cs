using System;
using System.Collections.Generic;
using UnityEngine;

namespace Xyz.Vasd.Fake.Data
{
    [AddComponentMenu("Fake/[Fake] Data Source")]
    public class FakeDataSource : MonoBehaviour, IFakeDataSource
    {
        protected Dictionary<Type, object> SourceDatas = new();

        public void SetSourceData(Type type, object value)
        {
            SourceDatas[type] = value;
        }

        public bool ContainsSourceData(Type type)
        {
            return SourceDatas.ContainsKey(type);
        }

        public bool ContainsSourceData<T>()
        {
            return SourceDatas.ContainsKey(typeof(T));
        }

        public T GetSourceData<T>()
        {
            return (T)GetSourceData(typeof(T));
        }

        public object GetSourceData(Type type)
        {
            if (SourceDatas.ContainsKey(type))
            {
                return SourceDatas[type];
            }

            return default;
        }
    }
}
