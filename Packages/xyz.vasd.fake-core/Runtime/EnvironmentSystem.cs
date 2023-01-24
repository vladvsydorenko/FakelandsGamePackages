using System.Linq;
using Xyz.Vasd.FakeData;
using Xyz.Vasd.FakeSystems;

namespace Xyz.Vasd.FakeCore
{
    public class EnvironmentSystem : DefaultFakeSystem, IUpdateFakeSystem
    {
        public FakeDatabase Database;

        public void OnSystemUpdate()
        {
            if (Database == null) return;

            var list = Database.GetList<EnvironmentData>();
            if (list == null || list.Count < 1) return;

            var item = list.First();
            if (item == null) return;

            item.Camera.gameObject.SetActive(false);
        }
    }
}
