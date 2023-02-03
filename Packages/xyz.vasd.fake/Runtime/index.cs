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
    }

    public class Node : INode
    {
        public readonly int Id;

        public Node(int id)
        {
            Id = id;
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

        int INode.GetId()
        {
            return Id;
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
    }

    public class WeakRegistry<TKey, TValue>
        where TKey : class
        where TValue : class
    {
        private ConditionalWeakTable<TKey, TValue> _values = new();
        private HashSet<int> _prints = new();

        public TValue Get(TKey key)
        {
            _values.TryGetValue(key, out var value);
            return value;
        }

        public bool Contains(int print)
        {
            return _prints.Contains(print);
        }

        public virtual bool Contains(TKey key)
        {
            _values.TryGetValue(key, out var value);
            return value != null;
        }

        public virtual void Set(TKey key, TValue value, int print)
        {
            _values.AddOrUpdate(key, value);
            _prints.Add(print);
        }

        public void SetIfEmpty(TKey key, TValue value, int print)
        {
            if (!Contains(key)) Set(key, value, print);
        }
    }

    public class NodeRegistry2 : WeakRegistry<GameObject, Node>
    {
        public void Scan(GameObject root, bool includeRoot = false)
        {
            for (int i = 0; i < root.transform.childCount; i++)
            {
                var child = root.transform.GetChild(i);
                SetIfEmpty(child.gameObject, new Node());
            }
        }

        public override bool Contains(GameObject key)
        {
            return Contains(key.GetInstanceID());
        }

        public void Set(GameObject key, Node value)
        {
            Set(key, value, key.GetInstanceID());
        }
        public void SetIfEmpty(GameObject key, Node value)
        {
            if (!Contains(key)) Set(key, value);
        }
        public static NodeRegistry GlobalRegistry
        {
            get
            {
                if (_instance == null) _instance = new NodeRegistry();
                return _instance;
            }
            private set => _instance = value;
        }
        private static NodeRegistry _instance;
    }
}

namespace Xyz.Vasd.Fake
{
    public class Route : MonoBehaviour
    {
        public string Path { get; private set; }
    
        private void Awake()
        {

        }
    }
}
