using UnityEngine;
using UnityEngine.AddressableAssets;
using Xyz.Vasd.FakeCore.Data;
using Xyz.Vasd.FakeCore.UI;
using Xyz.Vasd.FakeSystems;

namespace Xyz.Vasd.FakeCore.Systems
{

    public class PreloaderSystem : DefaultFakeSystem, IUpdateFakeSystem
    {
        public TextLogo Logo;
        public AssetReferenceGameObject MainMenuPrefabRef;
        public GameObject PreloaderCanvasRoot;

        private PreloaderData _data;

        public void OnSystemUpdate()
        {
            if (DataBase == null) return;
            if (!DataBase.ContainsSingleton<PreloaderData>()) return;

            if (_data == null) _data = DataBase.GetSingleton<PreloaderData>();

            switch (_data.State)
            {
                case PreloaderData.PreloaderState.None:
                    Logo.Play();
                    MainMenuPrefabRef.LoadAssetAsync();
                    _data.State = PreloaderData.PreloaderState.Loading;
                    if (PreloaderCanvasRoot != null) PreloaderCanvasRoot.SetActive(true);
                    break;
                case PreloaderData.PreloaderState.Loading:
                    if (IsItTimeToFinishing())
                    {
                        _data.State = PreloaderData.PreloaderState.Finishing;
                    }
                    break;
                case PreloaderData.PreloaderState.Finishing:
                    Logo.gameObject.SetActive(false);
                    _data.State = PreloaderData.PreloaderState.Done;
                    if (PreloaderCanvasRoot != null) PreloaderCanvasRoot.SetActive(false);
                    Instantiate(MainMenuPrefabRef.Asset);
                    break;
                case PreloaderData.PreloaderState.Done:
                default:
                    break;
            }
        }

        private bool IsItTimeToFinishing()
        {
            return Logo.IsTextComplete && MainMenuPrefabRef.IsDone;
        }
    }
}
