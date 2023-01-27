using UnityEngine;
using Xyz.Vasd.Fake.Database;

namespace Xyz.Vasd.Fake
{
    public class DefaultContext : MonoBehaviour
    {
        public FakeDatabase DB = new();
    }
}
