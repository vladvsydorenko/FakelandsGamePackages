using UnityEngine;

namespace Xyz.Vasd.Fake.Systems
{

    [AddComponentMenu("Fake/Systems/SystemBehaviour Host")]
    public class SystemBehaviourHost : MonoBehaviour
    {
        private FakeSystemManager Manager;

        private void Awake()
        {
            Manager = new FakeSystemManager();
            CollectSystems(transform);
        }

        private void OnEnable()
        {
            Manager.StartAllSystems();
            Manager.Stage_Start();
        }

        private void OnDisable()
        {
            Manager.StopAllSystem();
            Manager.Stage_Stop();
        }

        private void Update()
        {
            Manager.Stage_Start();
            Manager.Stage_Update();
            Manager.Stage_Stop();
        }

        private void FixedUpdate()
        {
            Manager.Stage_FixedUpdate();
        }

        private void CollectSystems(Transform root, bool clear = true)
        {
            if (clear) Manager.ClearSystems();

            for (int i = 0; i < root.childCount; i++)
            {
                var child = root.GetChild(i).gameObject;

                var breakpoint = child.GetComponent<SystemBehaviourHost>();
                if (breakpoint != null) continue;

                var system = child.GetComponent<IFakeSystem>();
                if (system != null) Manager.AddSystem(system);

                CollectSystems(child.transform, clear: false);
            }
        }
    }
}
