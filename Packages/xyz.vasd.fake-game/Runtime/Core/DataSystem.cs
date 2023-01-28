using UnityEngine;
using Xyz.Vasd.Fake.Systems;

namespace Xyz.Vasd.FakeGame.Core
{
    public class DataSystem<T> : SystemBehaviour where T : Component, IDataContext
    {
        protected T Context;

        public override bool IsSystemEnabled => base.IsSystemEnabled && Context != null;

        public override void OnSystemAwake()
        {
            Context = GetComponentInParent<T>();
        }
    }
}
