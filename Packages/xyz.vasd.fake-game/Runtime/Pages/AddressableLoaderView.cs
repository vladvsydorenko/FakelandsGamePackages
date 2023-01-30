using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Xyz.Vasd.FakeGame.Core;

namespace Xyz.Vasd.FakeGame.Pages
{
    public class AddressableLoaderView : AnimatedPage, ILoaderView
    {
        public AssetLabelReference Label;

        protected AsyncOperationHandle DownloadHandle;

        private void Start()
        {
            DownloadHandle = Addressables.DownloadDependenciesAsync(Label);
        }

        public bool IsLoaded()
        {
            return DownloadHandle.IsDone;
        }
    }
}
