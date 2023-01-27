using UnityEngine;

namespace Xyz.Vasd.Fake.Systems
{
    [AddComponentMenu("")]
    public class SystemBehaviour : MonoBehaviour, IFakeSystem
    {
        public virtual bool IsSystemEnabled => isActiveAndEnabled;

        public virtual void OnSystemAwake()
        {
        }

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

    [AddComponentMenu("")]
    public class SystemBehaviour<T> : SystemBehaviour where T : Component
    {
        protected T Context;

        public override void OnSystemAwake()
        {
            Context = GetComponentInParent<T>();
        }
    }
}
