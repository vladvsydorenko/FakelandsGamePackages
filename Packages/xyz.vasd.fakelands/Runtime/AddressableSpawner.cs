using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Xyz.Vasd.Fakelands
{
    [AddComponentMenu("Fakelands/[Fakelands] Addressable Spawner")]
    public class AddressableSpawner : MonoBehaviour
    {
        public AssetReferenceGameObject Prefab;
        public bool AutoLoad;

        protected GameObject Instance;
        private AsyncOperationHandle _handle;

        public GameObject Spawn(Transform parent = null)
        {
            if (Prefab.IsDone) return SpawnPrefab(parent);
            if (AutoLoad) Addressables.DownloadDependenciesAsync(Prefab, true);
            return null;
        }

        private GameObject SpawnPrefab(Transform parent = null)
        {
            return Instantiate((GameObject)Prefab.Asset, parent);
        }
    }
}
