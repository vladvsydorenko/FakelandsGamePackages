using System;
using UnityEngine;

namespace Xyz.Vasd.Fake
{
    public class index {}

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
}

namespace Xyz.Vasd.Fake
{
    public struct FakeResult<T>
    {
        public readonly T Value;
        public readonly Exception Error;

        public FakeResult(T value, Exception error = null)
        {
            Value = value;
            Error = error;
        }

        public static implicit operator T(FakeResult<T> other)
        {
            return other.Value;
        }

        public static implicit operator FakeResult<T>(T value)
        {
            return new FakeResult<T>(value);
        }
    }
}
