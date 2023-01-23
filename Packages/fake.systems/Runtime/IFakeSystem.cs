using Xyz.Vasd.FakeData;

namespace Xyz.Vasd.FakeSystems
{
    public interface IFakeSystem
    {
        bool IsSystemActive();
        void OnSystemSetup(FakeSystemManager manager);
        void OnSystemStart();
        void OnSystemStop();
    }
}
