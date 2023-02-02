using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Xyz.Vasd.Fakelands
{
    [AddComponentMenu("Fakelands/[Fakelands] Addressable Loader")]
    public class AddressableLoader : MonoBehaviour
    {
        private enum LoadStatus
        {
            Unloaded,
            Loading,
            Loaded
        }

        public AssetLabelReference[] Labels;
        public AssetReference[] References;

        private LoadStatus _status = LoadStatus.Unloaded;
        private bool _isAddressablesLoaded = false;

        private List<AsyncOperationHandle> _handles = new();

        public bool LoadAddressables()
        {
            var loaded = false;

            // already loaded            
            if (_status == LoadStatus.Loaded)
            {
                loaded = true;
            }
            
            // was not loaded yet
            else if (_status == LoadStatus.Unloaded)
            {
                LoadAddressables(Labels);
                LoadAddressables(References);
                
                _status = LoadStatus.Loading;

                loaded = false;
            }

            // loading is in progress
            else if (_status == LoadStatus.Loading)
            {
                if (IsLoaded()) _status = LoadStatus.Loaded;

                loaded = _status == LoadStatus.Loaded;
            }

            return loaded;
        }

        private void LoadAddressables(object[] keys)
        {
            foreach (var key in keys)
            {
                if (key == null) continue;

                var handle = Addressables.DownloadDependenciesAsync(key);
                _handles.Add(handle);
            }
        }

        private bool IsLoaded()
        {
            if (_isAddressablesLoaded) return true;

            _isAddressablesLoaded = _handles.All(h => h.IsDone);
            
            if (_isAddressablesLoaded) 
                _handles
                    .ForEach(h => Addressables.Release(h));

            return _isAddressablesLoaded;
        }
    }
}
