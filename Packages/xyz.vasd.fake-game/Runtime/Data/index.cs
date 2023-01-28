using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Xyz.Vasd.FakeGame.Data
{
    public class index { }

    [Serializable]
    public class PreloadRequest
    {
        public AssetReferenceGameObject[] Prefabs;

        public void SaySomething()
        {
            Debug.Log("Something");
        }
    }

    public class PreloaderState { }

    public class PrefabData
    {
        public GameObject Value;
        public AssetReferenceGameObject Reference;
    }
    public class PrefabLoadingTag { }
    public class PrefabReadyTag { }
}
