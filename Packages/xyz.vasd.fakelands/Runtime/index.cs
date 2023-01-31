using System;
using System.Reflection;
using UnityEngine;

/// <summary>
/// 
/// </summary>
namespace Xyz.Vasd.Fakelands
{
}

/// <summary>
/// Core
/// </summary>
namespace Xyz.Vasd.Fakelands
{
}

/// <summary>
/// Data
/// </summary>
namespace Xyz.Vasd.Fakelands
{
    public interface IDataSource<T>
    {
        T GetSourceData();
    }

    /// <summary>
    /// Injects data into fields of marked components
    /// </summary>
    public class FakeDataInjector : MonoBehaviour
    {
        public bool InjectOnAwake;

        private void Awake()
        {
            if (InjectOnAwake) InjectData();
        }

        [ContextMenu(nameof(InjectData) + "()")]
        public void InjectData()
        {
            InjectData(gameObject);
        }

        public static void InjectData(GameObject go)
        {
            var components = go.GetComponents<Component>();

            foreach (var component in components)
            {
                var type = component.GetType();
                var typeAttrs = type.GetCustomAttributes(typeof(FakeDataAttribute), true);
                if (typeAttrs.Length < 1) continue;

                InjectData(component);
            }
        }
        
        public static void InjectData(Component component)
        {
            var type = component.GetType();
            var fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (var field in fields)
            {
                object[] attrs = field.GetCustomAttributes(false);
                foreach (object attr in attrs)
                {
                    var contextAttr = attr as FakeDataAttribute;
                    if (contextAttr != null)
                    {
                        var dataType = typeof(IDataSource<>).MakeGenericType(field.FieldType);
                        var source = component.gameObject.GetComponentInParent(dataType);
                        var method = dataType.GetMethod(nameof(IDataSource<int>.GetSourceData));
                        field.SetValue(component, method.Invoke(source, new object[0]));
                    }
                }
            }
        }
    }

    /// <summary>
    /// Tags field to be injected from context
    /// </summary>
    public class FakeDataAttribute : Attribute
    {

    }
}

/// <summary>
/// Systems
/// </summary>
namespace Xyz.Vasd.Fakelands
{
    public interface ISystem
    {
        void SystemStart();
        void SystemUpdate();
        void SystemFixedUpdate();
        void SystemStop();
    }

    public class SystemBehaviour : MonoBehaviour, ISystem
    {
        protected virtual void Awake()
        {
            FakeDataInjector.InjectData(this);
        }

        void ISystem.SystemStart()
        {
        }

        void ISystem.SystemUpdate()
        {
        }

        void ISystem.SystemFixedUpdate()
        {
        }

        void ISystem.SystemStop()
        {
        }
    }
}
