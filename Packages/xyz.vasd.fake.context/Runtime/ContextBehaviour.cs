using System;
using System.Collections.Generic;
using UnityEngine;

namespace Xyz.Vasd.Fake.Context
{
    // stores data
    [AddComponentMenu("")]
    public abstract class ContextBehaviour<T> : MonoBehaviour where T : new()
    {
        public bool IsProxy;

        public T Context => IsProxy && _parent != null 
            ? _parent.Context 
            : _context;

        private T _context;
        private ContextBehaviour<T> _parent;

        private void Awake()
        {
            var parent = transform.parent;

            if (IsProxy)
            {
                if (parent != null) _parent = parent.GetComponentInParent<ContextBehaviour<T>>();
                else _context = new T();
            }
        }
    }
}
