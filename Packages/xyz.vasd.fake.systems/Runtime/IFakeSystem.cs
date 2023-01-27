namespace Xyz.Vasd.Fake.Systems
{
    public interface IFakeSystem
    {
        bool IsSystemEnabled { get; }
        void OnSystemStart();
        void OnSystemUpdate();
        void OnSystemFixedUpdate();
        void OnSystemStop();
    }
}
