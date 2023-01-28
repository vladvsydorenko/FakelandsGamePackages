using System.Linq;
using TMPro;
using UnityEngine;
using Xyz.Vasd.Fake.Database;
using Xyz.Vasd.FakeGame.Core;
using Xyz.Vasd.FakeGame.Data;

namespace Xyz.Vasd.FakeGame.Systems
{
    [AddComponentMenu("Fake Game/Systems/Preloader System")]
    public class PreloaderSystem : DataSystem
    {
        [Header("Text")]
        public string Text;
        public float TextSpeed;

        [Header("Extra")]
        public string Extra;
        public float ExtraSpeed;

        [Header("Refs")]
        public TextMeshProUGUI TextElement;

        private float _startTime;

        public override void OnSystemStart()
        {
            _startTime = Time.time;
        }

        public override void OnSystemUpdate()
        {
            var timeSpent = Time.time - _startTime;
            var textTime = TextSpeed * Text.Length;

            var extraTime = Mathf.Clamp(timeSpent - textTime, 0.0f, 1.0f);

            var textProgress = Mathf.Clamp(timeSpent / textTime, 0.0f, 1.0f);

            var textLength = (int)(Text.Length * textProgress);
            var extraLength = (int)(extraTime / ExtraSpeed);

            var text = "";
            if (textLength > 0) text = Text.Substring(0, textLength);

            var extra = string.Concat(Enumerable.Repeat(Extra, extraLength));

            TextElement.text = text + extra;

            //foreach (var page in _newRequests.Pages)
            //{
            //    var entries = page.GetEntries();
            //    var requests = page.GetDataArray<PreloadRequest>();

                //    for (int i = 0; i < page.Count; i++)
                //    {
                //        var entry = entries[i];
                //        var request = requests[i];

                //        // handle preload request

                //        request.SaySomething();
                //        Context.DB.SetData(entry, new PreloaderState());
                //    }
                //}

                //foreach (var page in _activeRequests.Pages)
                //{
                //    var entries = page.GetEntries();
                //    var requests = page.GetDataArray<PreloadRequest>();
                //    var states = page.GetDataArray<PreloaderState>();

                //    for (int i = 0; i < page.Count; i++)
                //    {
                //        var entry = entries[i];
                //        var request = requests[i];
                //        var state = states[i];

                //        // handle active preload request, update status
                //    }
                //}
        }
    }

}
