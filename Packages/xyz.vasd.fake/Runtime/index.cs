using UnityEngine.AddressableAssets;

//namespace Xyz.Vasd.Fake { public class index {} }

namespace Xyz.Vasd.Fake
{
    public class FakeAssetReference
    {
        public readonly AssetReference Reference;

        public FakeAssetReference(AssetReference reference)
        {
            Reference = reference;
        }
    }

    public static class FakeAddressables
    {

    }
}