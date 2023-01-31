using UnityEngine;

/// <summary>
/// Systems
/// </summary>
namespace Xyz.Vasd.Fakelands
{
    public class FakeSystem : MonoBehaviour, IFakeSystem
    {
        protected virtual void Awake()
        {
            FakeDataInjector.InjectData(this);
        }

        void IFakeSystem.SystemStart()
        {
        }

        void IFakeSystem.SystemUpdate()
        {
        }

        void IFakeSystem.SystemFixedUpdate()
        {
        }

        void IFakeSystem.SystemStop()
        {
        }
    }
}
