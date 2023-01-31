using System.Reflection;
using UnityEngine;

namespace Xyz.Vasd.Fake.Systems
{

    [AddComponentMenu("Fake/Systems/System Host")]
    public class FakeSystemHost : MonoBehaviour
    {
        private FakeSystemLoop Manager;

        private void Awake()
        {
            Manager = new FakeSystemLoop();
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

                CollectSystems(child.transform, clear: false);
            }
        }

        private void AddSystem(IFakeSystem system, GameObject go)
        {
            Manager.AddSystem(system);

            var type = system.GetType();

            var fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var field in fields)
            {
                object[] attrs = field.GetCustomAttributes(false);
                foreach (object attr in attrs)
                {
                    var contextAttr = attr as SystemContextAttribute;
                    if (contextAttr != null)
                    {
                        var dataType = typeof(ISystemContext<>).MakeGenericType(field.FieldType);
                        var source = go.GetComponentInParent(dataType);
                        var method = dataType.GetMethod(nameof(ISystemContext.GetSystemContextData));
                        field.SetValue(system, method.Invoke(source, new object[0]));
                    }
                }
            }
        }
    }
}
