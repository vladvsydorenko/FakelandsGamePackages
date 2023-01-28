using UnityEngine;
using Xyz.Vasd.Fake.Database;

namespace Xyz.Vasd.FakeGame.Core
{
    public interface IDataContext
    {
        FakeDatabase DB { get; }
    }

    [AddComponentMenu("Fake Game/Data Context")]
    public class DataContext : MonoBehaviour, IDataContext
    {
        public FakeDatabase DB => _db;
        private FakeDatabase _db = new();
    }
}
