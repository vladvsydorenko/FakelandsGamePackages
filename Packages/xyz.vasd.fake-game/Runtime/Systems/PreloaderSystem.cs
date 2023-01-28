using UnityEngine;
using Xyz.Vasd.Fake.Database;
using Xyz.Vasd.FakeGame.Core;
using Xyz.Vasd.FakeGame.Data;

namespace Xyz.Vasd.FakeGame.Systems
{
    [AddComponentMenu("Fake Game/Systems/Preloader System")]
    public class PreloaderSystem : DataSystem
    {
        private FakeGroup _newRequests;
        private FakeGroup _activeRequests;

        [ContextMenu("Test()")]
        private void Test()
        {
            Context.DB.CreateEntry(new PreloadRequest());
        }

        public override void OnSystemStart()
        {
            _newRequests = Context.DB
                .CreateGroupDescriptor()
                .Include<PreloadRequest>()
                .Exclude<PreloaderState>()
                .ToGroup();

            _activeRequests = Context.DB
                .CreateGroupDescriptor()
                .Include<PreloadRequest, PreloaderState>()
                .ToGroup();
        }

        public override void OnSystemUpdate()
        {
            foreach (var page in _newRequests.Pages)
            {
                var entries = page.GetEntries();
                var requests = page.GetDataArray<PreloadRequest>();

                for (int i = 0; i < page.Count; i++)
                {
                    var entry = entries[i];
                    var request = requests[i];

                    // handle preload request

                    request.SaySomething();
                    Context.DB.SetData(entry, new PreloaderState());
                }
            }

            foreach (var page in _activeRequests.Pages)
            {
                var entries = page.GetEntries();
                var requests = page.GetDataArray<PreloadRequest>();
                var states = page.GetDataArray<PreloaderState>();

                for (int i = 0; i < page.Count; i++)
                {
                    var entry = entries[i];
                    var request = requests[i];
                    var state = states[i];

                    // handle active preload request, update status
                }
            }
        }
    }

}
