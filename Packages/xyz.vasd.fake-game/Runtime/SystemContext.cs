using UnityEngine;
using Xyz.Vasd.Fake.Database;
using Xyz.Vasd.Fake.Systems;

namespace Xyz.Vasd.FakeGame
{
    public class SystemContext : MonoBehaviour,
        ISystemContext<FakeDatabase>
    {
        public readonly FakeDatabase DB = new();

        FakeDatabase ISystemContext<FakeDatabase>.GetSystemContextData()
        {
            return DB;
        }

        object ISystemContext.GetSystemContextData()
        {
            return null;
        }
    }

}
