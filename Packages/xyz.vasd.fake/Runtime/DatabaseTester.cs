using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace Xyz.Vasd.Fake
{
    public class DatabaseTester : MonoBehaviour
    {
        class Test
        {
            public int Id;
            public Test(int id)
            {
                Id = id;
            }
        }

        [ContextMenu(nameof(TestDataPage1) + "()")]
        public void TestDataPage1()
        {
            Debug.Log("---- TEST ----");

            var database = new Database();
            var entry = database.CreateEntry(new object[] { new Test(0) });
            var entry2 = database.CreateEntry(new object[] { new Test(1) });

            database.RemoveEntry(entry);
            database.SetData(typeof(Test), entry2, new Test(3));

            var test = (Test)database.GetData(typeof(Test), entry);
            var test2 = (Test)database.GetData(typeof(Test), entry2);
            Debug.Log(test == null);
            Debug.Log(test2.Id);


            Debug.Log("---------------------------------");

            database.SetData(transform.GetType(), entry2, transform);
            var tes = (Test)database.GetData(typeof(Test), entry2);
            var tr = (Transform)database.GetData(typeof(Transform), entry2);
            Debug.Log(tr != null);
            Debug.Log(tr.position);
            Debug.Log(tes.Id);
            
            Debug.Log("---- TEST END ----");
        }
    }
}
