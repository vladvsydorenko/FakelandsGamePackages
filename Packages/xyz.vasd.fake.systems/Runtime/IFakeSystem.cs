namespace Xyz.Vasd.Fake.Systems
{
    public interface IFakeSystem
    {
        bool IsSystemActive();
        void SystemStart();
        void SystemUpdate();
        void SystemFixedUpdate();
        void SystemStop();
    }
}
