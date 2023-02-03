//namespace Xyz.Vasd.Fake { public class index {} }

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Xyz.Vasd.Fake
{

    

}

// Components
namespace Xyz.Vasd.Fake
{
    public interface INode
    {
        void SetParent(INode node);
        void AddChild(INode child);
        void RemoveChild(INode child);
    }

    public class Node : INode
    {
        void INode.SetParent(INode node)
        {
        }

        void INode.AddChild(INode child)
        {
        }

        void INode.RemoveChild(INode child)
        {
        }
    }

    public class Route : MonoBehaviour
    {
        public string Path { get; private set; }
    
        public Route ParentRoute { get; private set; }
        public List<Route> ChildRoutes { get; private set; }

        private void Awake()
        {
            var parentRoute = transform.parent.GetComponentInParent<Route>();

            if (parentRoute == null)
            {
                // CASE: no parent route
                return;
            }

            parentRoute.AddChildRoute(this);
        }

        public void SetParentRoute(Route parent)
        {
            ParentRoute = parent;
        }

        public void AddChildRoute(Route route)
        {
            ChildRoutes?.Add(route);
        }

        public void RemoveChildRoute(Route route)
        {
            ChildRoutes?.Remove(route);
        }
    }
}
