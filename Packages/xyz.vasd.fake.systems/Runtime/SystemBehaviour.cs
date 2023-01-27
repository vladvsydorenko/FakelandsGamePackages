using UnityEngine;

namespace Xyz.Vasd.Fake.Systems
{
    [AddComponentMenu("")]
    public class SystemBehaviour : MonoBehaviour, IFakeSystem
    {
        public virtual bool IsSystemEnabled => isActiveAndEnabled;

        public virtual void OnSystemStart()
        {
        }

        public virtual void OnSystemUpdate()
        {
        }

        public virtual void OnSystemFixedUpdate()
        {
        }

        public virtual void OnSystemStop()
        {
        }
    }
}
