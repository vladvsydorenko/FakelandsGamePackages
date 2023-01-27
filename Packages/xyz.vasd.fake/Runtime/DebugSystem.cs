using UnityEngine;
using Xyz.Vasd.Fake.Systems;

namespace Xyz.Vasd.Fake
{
    public class DebugSystem : SystemBehaviour<DefaultContext>
    {
        public class TestClass1
        {
            public int Id;

            public TestClass1(int id)
            {
                Id = id;
            }
        }
        public class TestClass2 : TestClass1
        {
            public TestClass2(int id) : base(id)
            {
            }
        }

        public int Id;

        public override void OnSystemStart()
        {
            var group1 = Context.DB
                .CreateGroupDescriptor()
                .Include<TestClass1>()
                .ToGroup();

            var group2 = Context.DB
                .CreateGroupDescriptor()
                .Include<TestClass2>()
                .ToGroup();

            var group3 = Context.DB
                .CreateGroupDescriptor()
                .Include<TestClass1>()
                .Exclude<TestClass2>()
                .ToGroup();

            var group4 = Context.DB
                .CreateGroupDescriptor()
                .Include<TestClass2>()
                .Exclude<TestClass1>()
                .ToGroup();

            var group5 = Context.DB
                .CreateGroupDescriptor()
                .Include<TestClass2, TestClass1>()
                .ToGroup();

            var entry1 = Context.DB.CreateEntry(new TestClass1(0));
            var entry2 = Context.DB.CreateEntry(new TestClass2(0));
            var entry3 = Context.DB.CreateEntry(new TestClass1(1), new TestClass2(1));



            Debug.Log($"START: {Id}");
        }

        public override void OnSystemUpdate()
        {
            Debug.Log($"Update: {Id}");
        }

        public override void OnSystemFixedUpdate()
        {
            Debug.Log($"Fixed Update: {Id}");
        }

        public override void OnSystemStop()
        {
            Debug.Log($"STOP: {Id}");
        }
    }
}
