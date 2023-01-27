using System;
using UnityEngine;

namespace Xyz.Vasd.Fake.Database
{
    public class Tester : MonoBehaviour
    {
        public class TestClass { public int Id; }
        public class TestClass2 { public int Id2; }

        [ContextMenu(nameof(Test1) + "()")]
        public void Test1()
        {
            Debug.Log("---- TEST ----");

            var db = new FakeDatabase();

            var entry1 = db.CreateEntry(new TestClass { Id = 1 });
            var entry2 = db.CreateEntry(new TestClass { Id = 2 });

            Debug.Log(((TestClass)db.GetData(typeof(TestClass), entry1)).Id);
            Debug.Log(((TestClass)db.GetData(typeof(TestClass), entry2)).Id);

            db.RemoveEntry(entry1);

            Debug.Log((TestClass)db.GetData(typeof(TestClass), entry1));
            Debug.Log(((TestClass)db.GetData(typeof(TestClass), entry2)).Id);

            db.SetData(typeof(TestClass2), entry2, new TestClass2 { Id2 = 22 });
            db.SetData(typeof(TestClass2), entry2, new TestClass2 { Id2 = 22 });
            Debug.Log(((TestClass2)db.GetData(typeof(TestClass2), entry2)).Id2);

            db.RemoveData(typeof(TestClass), entry2);
            Debug.Log((TestClass)db.GetData(typeof(TestClass), entry2) == null);

            Debug.Log("================================================");
            Debug.Log("GROUPS");
            var group = db.CreateGroup(
                new Type[] { typeof(TestClass), typeof(TestClass2) },
                new Type[] { }
            );

            Debug.Log(group.Pages.Count);
            Debug.Log(db.Pages.Count);

            Debug.Log("---- TEST END ----");
        }

        [ContextMenu(nameof(Test2) + "()")]
        public void Test2()
        {
            Debug.Log("---- TEST ----");



            Debug.Log("---- TEST END ----");
        }
    }
}
