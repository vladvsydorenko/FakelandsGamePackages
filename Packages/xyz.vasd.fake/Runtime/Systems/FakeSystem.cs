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

        bool IFakeSystem.IsSystemActive()
        {
            return isActiveAndEnabled;
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
