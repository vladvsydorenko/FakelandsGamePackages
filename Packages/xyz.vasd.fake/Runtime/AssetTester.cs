using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Xyz.Vasd.Fake
{
    public class AssetTester : MonoBehaviour
    {
        private void Start()
        {
            var refer = new AssetReferenceT<GameObject>("7fabbba6cc332974caee4aff89b518a8");
            refer.LoadAssetAsync();
            refer.OperationHandle.Completed += OnCompleted;
        }

        private void OnCompleted(AsyncOperationHandle obj)
        {
            Instantiate(obj.Result as GameObject);
        }
    }
}