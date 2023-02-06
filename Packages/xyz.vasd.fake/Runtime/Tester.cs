using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Xyz.Vasd.Fake.Task;

namespace Xyz.Vasd.Fake
{
    public class Tester : MonoBehaviour
    {
        private void Start()
        {
            //Test_Tasks.Test();
            Test_QucikTasks.Test();
        }
    }

    public class Test_Addressables
    {
        public static void Test()
        {
            var refer = new AssetReferenceT<GameObject>("7fabbba6cc332974caee4aff89b518a8");
            refer.LoadAssetAsync();
            refer.OperationHandle.Completed += OnCompleted;
        }

        private static void OnCompleted(AsyncOperationHandle obj)
        {
            Object.Instantiate(obj.Result as GameObject);
        }
    }
    public class Test_Tasks
    {
        private class TestTask : FakeTask
        {
            private int _test = 0;

            protected override void OnStart()
            {
                _test = 0;
                Debug.Log($"My task started: {Version}");
            }
            protected override bool OnExecute()
            {
                if (_test > 1) return true;
                _test++;
                Debug.Log($"My task executing: {Version}");
                return false;
            }
            protected override void OnStop()
            {
                Debug.Log($"My task stopped: {Version}");
            }
        }

        public static void Test()
        {
            var task = new TestTask();

            task.Execute(0);
            task.Execute(0);
            var isDone = task.Execute(0);

            Debug.Log($"task is done: {isDone}");

            var isDone1 = task.Execute(1);
            Debug.Log($"task is done 1: {isDone1}");
        }
    }
    public class Test_QucikTasks
    {
        public static void Test()
        {
            var value = 0f;

            var task2 = new FakeTask() as IFakeTask;
            task2.Then(v => { });

            var task = FakeTask.Function((v) =>
                {
                    Debug.Log("Create me");
                    value = 0f;
                })
                .Then((v) =>
                {
                    value++;
                    Debug.Log($"Update me {value}");
                    return value > 2f;
                })
                .Then((v) =>
                {
                    Debug.Log("Stop me");
                });


            var isDone = task.Execute(0);

            Debug.Log($"isDone should be false: {isDone == false}");

            isDone = task.Execute(0);

            Debug.Log($"isDone should be false: {isDone == false}");

            isDone = task.Execute(0);

            Debug.Log($"isDone should be true: {isDone == true}");

            task.Execute(0);
            task.Execute(1);
            task.Execute(1);
        }
    }
}