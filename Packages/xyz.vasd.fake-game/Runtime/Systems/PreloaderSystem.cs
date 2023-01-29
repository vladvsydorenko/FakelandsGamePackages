using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Xyz.Vasd.FakeGame.Core;

namespace Xyz.Vasd.FakeGame.Systems
{
    public enum ViewStage
    {
        Closed,
        Opening,
        Open,
        Closing,
    }
    public interface IView
    {
        void OpenView();
        void HideView();

        ViewStage GetViewStage();
        bool IsDone();
    }

    public class PreloaderSystem : DataSystem
    {

        [Tooltip("Main text will be shown even if loading is faster")]
        public string MainText;
        [Tooltip("Speed per character")]
        public float MainTextSpeed;
        public float MainTextDelay;
        public TextMeshProUGUI MainTextElement;

        [Tooltip("Extra text will be repeated endless")]
        public string ExtraText;
        [Tooltip("Speed per character")]
        public float ExtraTextSpeed;
        public TextMeshProUGUI ExtraTextElement;

        private float _time;
        private float _start;
        private float _spent;
        private float _extraSpent;
        private AsyncOperationHandle _downloadHandle;

        private string _mainText;
        private string _extraText;

        public override void OnSystemStart()
        {
            _downloadHandle = Addressables.DownloadDependenciesAsync("preload");

            _start = Time.time;
        }

        public override void OnSystemUpdate()
        {
            _time = Time.time;
            _spent = Mathf.Clamp(_time - MainTextDelay - _start, 0f, float.MaxValue);
            _extraSpent = Mathf.Clamp(_spent - (float)(MainText.Length * MainTextSpeed), 0f, float.MaxValue);

            _mainText = CalculateMainText();

            if (_mainText == MainText && _downloadHandle.IsDone)
            {
                if (_downloadHandle.IsValid()) Addressables.Release(_downloadHandle);
                _mainText = "READY";
                _extraText = "^^^^^";
            }
            else
            {
                _extraText = CalculateExtraText();
            }

            RefreshView();
        }

        private string CalculateMainText()
        {
            var progress = (_spent / MainTextSpeed) / MainText.Length;
            if (progress <= 0f) return "";

            var len = (int)(MainText.Length * progress);

            if (len >= MainText.Length)
            {
                return MainText;
            };

            return MainText[..len];
        }

        private string CalculateExtraText()
        {
            var progress = (_extraSpent / ExtraTextSpeed) / ExtraText.Length;
            if (progress <= 0f) return "";

            var len = (int)(ExtraText.Length * progress);
            var repeats = (int)Mathf.Floor(len / ExtraText.Length);

            var text = string.Concat(Enumerable.Repeat(ExtraText, repeats));

            len -= (repeats * ExtraText.Length);

            if (len <= 0) return text;

            return text + ExtraText[..len];
        }

        private void RefreshView()
        {
            if (MainTextElement != null) MainTextElement.text = _mainText;
            if (ExtraTextElement != null) ExtraTextElement.text = _extraText;
        }
    }

    public class ApplicationSystem : DataSystem
    {
        private enum AppStage
        {
            Start,
            Preloading,
            MenuLoading,
            MenuLoader_Show,
            MenuLoader_Running,
            Menu_Show,
        }

        public GameObject PreloaderRoot;
        public GameObject LoaderRoot;

        private AppStage _stage;

        private IView PreloaderView;
        private IView MenuLoaderView;

        public override void OnSystemUpdate()
        {
            // show preloader
            // hide preloader
            // show loader
            // hide loader

            switch (_stage)
            {
                case AppStage.Start:
                case AppStage.Preloading:
                    switch (PreloaderView.GetViewStage())
                    {
                        case ViewStage.Closed:
                        case ViewStage.Closing:
                            PreloaderView.OpenView();
                            break;
                        case ViewStage.Open:
                            if (!PreloaderView.IsDone()) break;
                            _stage = AppStage.MenuLoading;
                            break;
                        case ViewStage.Opening:
                        default:
                            break;
                    }
                    break;
                case AppStage.MenuLoading:
                    switch (MenuLoaderView.GetViewStage())
                    {
                        case ViewStage.Closed:
                        case ViewStage.Closing:
                            MenuLoaderView.OpenView();
                            break;
                        case ViewStage.Opening:
                            break;
                        case ViewStage.Open:
                            if (PreloaderView.GetViewStage() != ViewStage.Closed)
                            {
                                PreloaderView.HideView();
                            }
                            if (!MenuLoaderView.IsDone()) break;
                            MenuLoaderView.HideView();
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
