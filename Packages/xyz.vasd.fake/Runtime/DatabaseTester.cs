using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace Xyz.Vasd.Fake
{
    public class DatabaseTester : MonoBehaviour
    {
        [ContextMenu(nameof(TestDataPage1) + "()")]
        public void TestDataPage1()
        {
            Debug.Log("---- TEST ----");

            var page = new Page(0, new Type[] { typeof(Transform) });
            var entry = new Entry(0);

            entry = page.Add(entry);
            page.SetData(typeof(Transform), entry, transform);
            var tr = (Transform)page.GetData(typeof(Transform), entry);

            Debug.Log(tr.position);

            var entry2 = new Entry(1);
            entry2 = page.Add(entry2);
            page.SetData(typeof(Transform), entry2, transform.parent);
            var tr2 = (Transform)page.GetData(typeof(Transform), entry2);

            Debug.Log(tr2.position);

            var moved = page.Remove(entry2);
            Debug.Log(moved.Id);
            Debug.Log(moved.Index);
            Debug.Log(page.Count);

            var tr3 = (Transform)page.GetData(typeof(Transform), moved);
            Debug.Log(tr3.position);

            Debug.Log("---- TEST END ----");
        }
    }
}
