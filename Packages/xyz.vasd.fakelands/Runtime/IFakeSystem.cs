/// <summary>
/// Systems
/// </summary>
namespace Xyz.Vasd.Fakelands
{
    public interface IFakeSystem
    {
        void SystemStart();
        void SystemUpdate();
        void SystemFixedUpdate();
        void SystemStop();
    }
}
