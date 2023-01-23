using UnityEngine;

namespace Xyz.Vasd.FakeSystems
{
    [AddComponentMenu("FakeSystems/" + nameof(DefaultFakeSystemGroup))]
    public class DefaultFakeSystemGroup : FakeSystemGroup
    {
        public DefaultFakeSystem[] Systems;

        public override void InitSystemGroup(FakeSystemManager manager)
        {
            base.InitSystemGroup(manager);

            foreach (var system in Systems)
            {
                AddSystem(system);
            }
        }
    }
}
