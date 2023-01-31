using System.Reflection;
using UnityEngine;

/// <summary>
/// Data
/// </summary>
namespace Xyz.Vasd.Fake.Data
{
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
                        var dataType = typeof(IFakeDataSource<>).MakeGenericType(field.FieldType);
                        var source = component.gameObject.GetComponentInParent(dataType);
                        object value = null;

                        if (source != null)
                        {
                            var method = dataType.GetMethod(nameof(IFakeDataSource<int>.GetSourceData));
                            value = method.Invoke(source, new object[0]);
                        }
                        else
                        {
                            var fallbackSource = component.gameObject.GetComponentInParent<IFakeDataSource>();
                            if (fallbackSource != null || fallbackSource.ContainsSourceData(field.FieldType))
                            {
                                value = fallbackSource.GetSourceData(field.FieldType);
                            }
                        }

                        field.SetValue(component, value);
                    }
                }
            }
        }
    }
}