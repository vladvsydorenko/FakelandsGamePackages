using UnityEngine;

namespace Xyz.Vasd.FakeData
{
    // just a tag for finding components
    public interface IFakeData
    {

    }

    [AddComponentMenu("FakeData/" + nameof(FakeDataInjector))]
    public class FakeDataInjector : MonoBehaviour
    {
        public FakeDatabase Database;

        private void Awake()
        {
            Inject();
        }

        public void Inject()
        {
            if (Database == null) Database = FindDatabase();

            var datas = GetComponents<IFakeData>();
            foreach (var data in datas)
            {
                Debug.Log($"add to list {data.GetType()}");
                Database.AddToList(data.GetType(), data);
            }
        }

        private FakeDatabase FindDatabase()
        {
            var dataBase = GetComponentInParent<FakeDatabase>();
            if (dataBase != null) return dataBase;

            dataBase = FindObjectOfType<FakeDatabase>();
            return dataBase;
        }
    }
}
