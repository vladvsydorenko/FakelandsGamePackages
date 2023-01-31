using System;

namespace Xyz.Vasd.Fakelands
{
    public interface IFakeDataSource
    {
        void SetSourceData(Type type, object value);
        bool ContainsSourceData(Type type);
        bool ContainsSourceData<T>();
        T GetSourceData<T>();
        object GetSourceData(Type type);
    }

    public interface IFakeDataSource<T>
    {
        T GetSourceData();
    }
}