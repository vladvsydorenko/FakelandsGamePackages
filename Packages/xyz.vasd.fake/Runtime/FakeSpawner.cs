using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Xyz.Vasd.Fake
{
    public class Asset
    {
        public AssetReference Reference;
    }    

    public class AssetsMan
    {
        private struct AssetData
        {
            public AssetReference Reference;
            public List<Asset> Assets;
        }

        private Dictionary<AssetReference, AssetData> _assets;

        public Asset CreateAsset(AssetReference reference)
        {
            if (!_assets.ContainsKey(reference))
            {
                _assets[reference] = new AssetData()
                {
                    Reference = reference,
                    Assets = new List<Asset>(),
                };
            }

            var data = _assets[reference];
            var asset = new Asset();
            data.Assets.Add(asset);
            return null;
        }

        public void ReleaseAsset(Asset asset)
        {
            if (!_assets.ContainsKey(asset.Reference) || !asset.Reference.IsDone) return;
            Addressables.Release(asset.Reference);
        }
    }

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