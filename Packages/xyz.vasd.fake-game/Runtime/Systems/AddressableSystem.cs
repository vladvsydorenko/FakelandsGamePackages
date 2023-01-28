using Xyz.Vasd.Fake.Database;
using Xyz.Vasd.FakeGame.Core;
using Xyz.Vasd.FakeGame.Data;

namespace Xyz.Vasd.FakeGame.Systems
{
    public class AddressableSystem : DataSystem
    {
        private FakeGroup _newPrefabs;
        private FakeGroup _loadingPrefabs;

        public override void OnSystemStart()
        {
            _newPrefabs = Context.DB
                .CreateGroupDescriptor()
                .Include<PrefabData>()
                .Exclude<PrefabLoadingTag>()
                .ToGroup();

            _loadingPrefabs = Context.DB
                .CreateGroupDescriptor()
                .Include<PrefabData, PrefabLoadingTag>()
                .ToGroup();
        }

        public override void OnSystemUpdate()
        {
            foreach (var page in _newPrefabs.Pages)
            {
                var entries = page.GetEntries();
                var prefabs = page.GetDataArray<PrefabData>();

                for (int i = 0; i < page.Count; i++)
                {
                    var entry = entries[i];
                    var prefab = prefabs[i];

                    prefab.Reference.LoadAssetAsync();
                    Context.DB.SetData(entry, new PrefabLoadingTag());
                }
            }

            foreach (var page in _loadingPrefabs.Pages)
            {
                var entries = page.GetEntries();
                var prefabs = page.GetDataArray<PrefabData>();

                for (int i = 0; i < page.Count; i++)
                {
                    var entry = entries[i];
                    var prefab = prefabs[i];

                    if (prefab.Reference.IsDone)
                    {
                        Context.DB.SetData(entry, new PrefabReadyTag());
                    }
                }
            }
        }
    }
}
