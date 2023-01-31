using UnityEngine;

namespace Xyz.Vasd.Fake.Systems
{
    [AddComponentMenu("")]
    public class FakeSystem : MonoBehaviour, IFakeSystem
    {
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
