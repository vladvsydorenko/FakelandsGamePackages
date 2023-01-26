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

            var page = new Page(0);

            var entry = new Entry(0);

            Debug.Log(entry.Id);
            Debug.Log(entry.Page);
            Debug.Log(entry.Index);

            entry = page.Create(entry);

            Debug.Log("After create:");

            Debug.Log(entry.Id);
            Debug.Log(entry.Page);
            Debug.Log(entry.Index);

            var entry2 = new Entry(1);
            entry2 = page.Create(entry2);

            var moved = page.Remove(entry);

            Debug.Log("After remove:");

            Debug.Log(moved.Id);
            Debug.Log(moved.Page);
            Debug.Log(moved.Index);


            Debug.Log("After remove, last element:");
            moved = page.Entries[1];
            Debug.Log(page.Count);
            Debug.Log(moved.Id);
            Debug.Log(moved.Page);
            Debug.Log(moved.Index);

            Debug.Log("---- TEST END ----");
        }
    }
}
