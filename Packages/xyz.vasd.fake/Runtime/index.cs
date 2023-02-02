using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//namespace Xyz.Vasd.Fake { public class index {} }

namespace Xyz.Vasd.Fake
{
    public interface IElement
    {
        IElement GetParent();
        List<IElement> GetChildren();

        void AddChild(IElement child);
        void RemoveChild(IElement child);
        void SetParent(IElement parent);
    }

    public class Element : IElement
    {
        protected IElement Parent;
        protected List<IElement> Children;


        public IElement GetParent()
        {
            return Parent;
        }

        public List<IElement> GetChildren()
        {
            return Children;
        }


        public void SetParent(IElement parent)
        {
            var oldParent = Parent;
            oldParent.RemoveChild(this);
            parent.AddChild(this);

            OnSetParent(parent, oldParent);
        }
        protected virtual void OnSetParent(IElement parent, IElement oldParent)
        {

        }

        public void AddChild(IElement child)
        {
            if (Children.Contains(child)) return;
            Children.Add(child);
            child.SetParent(this);
            OnAddChild(child);
        }
        protected virtual void OnAddChild(IElement child)
        {

        }

        public void RemoveChild(IElement child)
        {
            Children.Remove(child);
            child.SetParent(null);

            OnRemoveChild(child);
        }
        protected virtual void OnRemoveChild(IElement child)
        {

        }
    }

}