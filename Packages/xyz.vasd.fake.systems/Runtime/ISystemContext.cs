namespace Xyz.Vasd.Fake.Systems
{
    public interface ISystemContext
    {
        object GetSystemContextData();
    }

    public interface ISystemContext<T> : ISystemContext
    {
        new T GetSystemContextData();
    }
}
