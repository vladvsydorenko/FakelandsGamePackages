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

        INode GetParent();
        void SetParent(INode node);

        INode[] GetChildren();
        void AddChild(INode child);
        void RemoveChild(INode child);

        bool Contains(Type type);
        object GetContent(Type type);
    }

    public class Node : INode
    {
        public readonly int Id;

        private List<Type> _types = new();
        private Dictionary<Type, object> _content = new(); 

        private List<INode> _children = new();
        private int _childrenVersion;

        private INode[] _childrenArray = new INode[0];
        private int _childrenArrayVersion;

        public Node(int id)
        {
            Id = id;
        }

        int INode.GetId()
        {
            return Id;
        }

        #region parent
        INode INode.GetParent()
        {
            throw new NotImplementedException();
        }

        void INode.SetParent(INode node)
        {
        }
        #endregion

        #region Children
        INode[] INode.GetChildren()
        {
            if (_childrenArrayVersion != _childrenVersion)
            {
                _childrenArray = _children.ToArray();
                _childrenArrayVersion = _childrenVersion;
            }

            return _childrenArray;
        }

        void INode.AddChild(INode child)
        {
            _children.Add(child);
            _childrenVersion++;
        }

        void INode.RemoveChild(INode child)
        {
            _children.Remove(child);
            _childrenVersion++;
        }
        #endregion

        #region Content
        bool INode.Contains(Type type)
        {
            return _types.Contains(type);
        }

        object INode.GetContent(Type type)
        {
            if (_content.ContainsKey(type)) return _content[type];
            return null;
        }
        #endregion
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
