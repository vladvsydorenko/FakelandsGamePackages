using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Xyz.Vasd.Fake.Database;
using Xyz.Vasd.FakeGame.Core;
using Xyz.Vasd.FakeGame.Data;

namespace Xyz.Vasd.FakeGame.Systems
{
    public class PreloaderSystem : DataSystem
    {

        [Tooltip("Main text will be shown even if loading is faster")]
        public string MainText;
        [Tooltip("Speed per character")]
        public float MainTextSpeed;
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
            //_state.SizeHandle = Addressables.GetDownloadSizeAsync(Settings.Labels);
            //_state.DownloadHandle = Addressables.DownloadDependenciesAsync(Settings.Labels);

            _start = Time.time;
        }

        public override void OnSystemUpdate()
        {
            _time = Time.time;
            _spent = _time - _start;
            _extraSpent = Mathf.Clamp(_spent - (float)(MainText.Length * MainTextSpeed), 0f, float.MaxValue);

            _mainText = CalculateMainText();
            _extraText = CalculateExtraText();

            RefreshView();
        }

        private string CalculateMainText()
        {
            var progress = _spent / MainTextSpeed;
            if (progress <= 0f) return "";

            var len = (int)(MainText.Length * progress);

            if (len >= MainText.Length)
            {
                Debug.Log("YEAH!");
                return MainText;
            };

            return MainText[..len];
        }

        private string CalculateExtraText()
        {
            var progress = _extraSpent / ExtraTextSpeed;
            Debug.Log($"_spent {(float)(_spent)}");
            Debug.Log($"_textDuration {(float)(MainText.Length * MainTextSpeed)}");
            Debug.Log($"_extraSpent {_extraSpent}");
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

}
