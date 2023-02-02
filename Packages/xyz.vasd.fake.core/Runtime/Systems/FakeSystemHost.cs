using System;
using UnityEngine;
using Xyz.Vasd.Fake.Data;

namespace Xyz.Vasd.Fake.Systems
{
    public static class GameObjectTools
    {
        public static void FindComponentsInTree<T, T_Breakpoint>(Transform root, Action<T, GameObject> callback)
        {
            for (int i = 0; i < root.childCount; i++)
            {
                var child = root.GetChild(i).gameObject;

                var breakpoint = child.GetComponent<T_Breakpoint>();
                if (breakpoint != null) continue;

                var systems = child.GetComponents<T>();
                foreach (var system in systems)
                {
                    if (system != null)
                    {
                        callback.Invoke(system, child);
                    }
                }

                FindComponentsInTree<T, T_Breakpoint>(child.transform, callback);
            }
        }
    }

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
            GameObjectTools.FindComponentsInTree<IFakeSystem, FakeSystemHost>(root, AddSystem);

            // TODO: if line aboive works, delete it
            //for (int i = 0; i < root.childCount; i++)
            //{
            //    var child = root.GetChild(i).gameObject;

            //    var breakpoint = child.GetComponent<FakeSystemHost>();
            //    if (breakpoint != null) continue;

            //    var systems = child.GetComponents<IFakeSystem>();
            //    foreach (var system in systems)
            //    {
            //        if (system != null)
            //        {
            //            AddSystem(system, child);
            //        }
            //    }

            //    CollectSystems(child.transform);
            //}
        }
    }
}
