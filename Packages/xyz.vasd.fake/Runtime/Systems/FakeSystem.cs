using UnityEngine;
using Xyz.Vasd.Fake.Data;

namespace Xyz.Vasd.Fake.Systems
{
    [AddComponentMenu("")]
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
