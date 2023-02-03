//namespace Xyz.Vasd.Fake { public class index {} }

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Xyz.Vasd.Fake
{

    

}

// Components
namespace Xyz.Vasd.Fake
{
    public interface INode
    {
        int GetId();

        void SetParent(INode node);

        void AddChild(INode child);
        void RemoveChild(INode child);

        bool Contains(Type type);
    }

    public class Node : INode
    {
        public readonly int Id;

        protected readonly List<Type> Types;

        public Node(int id)
        {
            Id = id;
        }

        int INode.GetId()
        {
            return Id;
        }

        void INode.SetParent(INode node)
        {
        }

        void INode.AddChild(INode child)
        {
        }

        void INode.RemoveChild(INode child)
        {
        }

        bool INode.Contains(Type type)
        {
            return Types.Contains(type);
        }
    }

    public class NodeRegistry<T> where T : class, INode
    {
        private HashSet<int> _prints = new();
        private ConditionalWeakTable<GameObject, T> _nodes = new();

        public bool Contains(GameObject go)
        {
            return Contains(go.GetInstanceID());
        }

        public bool Contains(int id)
        {
            return _prints.Contains(id);
        }

        public T Get(GameObject go)
        {
            _nodes.TryGetValue(go, out T node);
            return node;
        }

        public void Set(GameObject go, T value)
        {
            _nodes.AddOrUpdate(go, value);
            _prints.Add(value.GetId());
        }
    }

    public class NodeRegistry : NodeRegistry<Node>
    {
        public static NodeRegistry Global
        {
            get
            {
                if (_global == null) _global = new NodeRegistry();
                return _global;
            }
        }
        private static NodeRegistry _global;

    }

    public static class GameObjectExtensions
    {
        private static NodeRegistry Registry = NodeRegistry.Global;

        public static INode GetFakeNode(this GameObject instance)
        {
            var id = instance.GetInstanceID();

            if (!Registry.Contains(id)) Registry.Set(instance, new Node(id));

            return Registry.Get(instance);
        }

        public static void SetFakeNode(this GameObject instance, Node node)
        {
            Registry.Set(instance, node);
        }
    }
}

namespace Xyz.Vasd.Fake
{
    public class Route : MonoBehaviour
    {
        public string Path { get; private set; }
    
        private void Awake()
        {
            var node = gameObject.GetFakeNode();
        }
    }
}
