namespace Xyz.Vasd.Fake.Systems
{
    public interface IFakeSystem
    {
        void SystemStart();
        void SystemUpdate();
        void SystemFixedUpdate();
        void SystemStop();
    }
}
