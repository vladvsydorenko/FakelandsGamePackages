using UnityEngine;

namespace Xyz.Vasd.Fake
{
    public class DatabaseTester : MonoBehaviour
    {
        [ContextMenu(nameof(Test) + "()")]
        public void Test()
        {
            var db = new Database();
            db.CreateItem(transform);
            var group = db.CreateGroup(typeof(Transform));

            var tra = group.Pages[0].Get(typeof(Transform), 0) as Transform;

            Debug.Log($"count: {group.Pages.Count}");
            Debug.Log($"Item: {tra.position}");
        }
    }
}
