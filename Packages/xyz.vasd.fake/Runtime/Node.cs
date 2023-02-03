using System;
using System.Collections.Generic;

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
    }

    public class Node : INode
    {
        public readonly int Id;

        public Node Parent { get; private set; }

        private List<INode> _children = new();
        private int _childrenVersion;

        private INode[] _childrenArray = new INode[0];
        private int _childrenArrayVersion;

        public Node(int id, Node parent = null)
        {
            Id = id;
            Parent = parent;
        }

        public int GetId()
        {
            return Id;
        }

        #region parent
        public INode GetParent()
        {
            throw new NotImplementedException();
        }

        public void SetParent(INode node)
        {
            
        }
        #endregion

        #region Children
        public INode[] GetChildren()
        {
            if (_childrenArrayVersion != _childrenVersion)
            {
                _childrenArray = _children.ToArray();
                _childrenArrayVersion = _childrenVersion;
            }

            return _childrenArray;
        }

        public void AddChild(INode child)
        {
            _children.Add(child);
            _childrenVersion++;
        }

        public void RemoveChild(INode child)
        {
            _children.Remove(child);
            _childrenVersion++;
        }
        #endregion
    }
}
