using UnityEngine;
/// <summary>
/// 
/// </summary>
namespace Xyz.Vasd.Fakelands
{   
    public class FakelandsTest : SystemBehaviour
    {
        [FakeData]
        private int _db;
    }

    public class SingletonSource<T> : MonoBehaviour, IDataSource<T> where T : class, new()
    {
        private T _singleton = new();

        public T GetSourceData()
        {
            return _singleton;
        }
    }
}
