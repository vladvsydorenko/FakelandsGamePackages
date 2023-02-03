using UnityEngine;
using UnityEngine.AddressableAssets;

//namespace Xyz.Vasd.Fake { public class index {} }

namespace Xyz.Vasd.Fake
{
    public enum FakeActionStage
    {
        None,
        Start,
        Work,
        Done,
    }

    public class FakeAsset
    {
        public FakeActionStage LoadingStage { get; private set; }

        public bool Load()
        {
            return LoadingStage == FakeActionStage.Done;
        }
    }
}