using UnityEngine;
using Xyz.Vasd.Fake.Data;

namespace Xyz.Vasd.Fake.Systems
{
    [AddComponentMenu("Fake/[Fake] System Host")]
    public class FakeSystemHost : MonoBehaviour
    {
        public bool AutoStart;
        private FakeSystemRunner _runner = new();

        private void Awake()
        {
            if (AutoStart) StartSystemHost();
        }

        public void AddSystem(IFakeSystem system, GameObject go)
        {
            _runner.AddSystem(system);
            FakeDataInjector.InjectData(go);
        }

        public void StartSystemHost()
        {
            _runner.ClearSystems();
            CollectSystems(transform);
        }

        private void CollectSystems(Transform root)
        {
            for (int i = 0; i < root.childCount; i++)
            {
                var child = root.GetChild(i).gameObject;

                var breakpoint = child.GetComponent<FakeSystemHost>();
                if (breakpoint != null) continue;

                var systems = child.GetComponents<IFakeSystem>();
                foreach (var system in systems)
                {
                    if (system != null)
                    {
                        AddSystem(system, child);
                    }
                }

                CollectSystems(child.transform);
            }
        }

    }
}
