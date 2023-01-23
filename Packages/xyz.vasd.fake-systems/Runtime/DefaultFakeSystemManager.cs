using UnityEngine;
using Xyz.Vasd.FakeData;

namespace Xyz.Vasd.FakeSystems
{
    [AddComponentMenu("FakeSystems/" + nameof(DefaultFakeSystemManager))]
    public class DefaultFakeSystemManager : FakeSystemManager
    {
        public FakeDataBase InitialDataBase;
        public FakeSystemGroup[] InitialGroups;

        private void Awake()
        {
            base.DataBase = InitialDataBase;
            base.SystemGroups = InitialGroups;
        }
    }
}
