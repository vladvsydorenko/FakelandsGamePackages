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

        public override bool IsSystemEnabled => base.IsSystemEnabled && Context != null;

        public override void OnSystemAwake()
        {
            Debug.Log("awake?");
            Context = GetComponentInParent<T>();
        }
    }
}
