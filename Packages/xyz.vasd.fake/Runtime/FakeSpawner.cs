using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Xyz.Vasd.Fake
{
    [AddComponentMenu("Fake/[Fake] Spawner")]
    public class FakeSpawner : MonoBehaviour
    {
        #region Prefab
        public AssetReferenceGameObject Asset { get; private set; }
        public AssetLabelReference r;

        [SerializeField]
        [Tooltip("Reference to addressable prefab to be loaded and instantiated")]
        private AssetReferenceGameObject _asset;
        #endregion

        #region Parent
        public Transform Parent { get; private set; }

        [SerializeField]
        [Tooltip("If not set will get its own transform")]
        private Transform _parent;
        #endregion

        protected virtual void Awake()
        {
            Prepare();
        }

        protected virtual void OnValidate()
        {
            Prepare();
        }

        public bool Load()
        {
            return false;
        }

        public bool Spawn()
        {
            if (!Load()) return false;
            return false;
        }

        private void Prepare()
        {
            if (_parent == null) _parent = transform;
        }
    }
}