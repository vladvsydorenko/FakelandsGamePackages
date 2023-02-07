namespace Xyz.Vasd.Fake.Task
{
    public static class IFakeTaskExtensions
    {
        public static FakeTask.TaskFunction Then(this IFakeTask instance, IFakeTask nextTask)
        {
            return new FakeTask.TaskFunction(v => instance.Execute(v) && nextTask.Execute(v));
        }
 
        public static FakeTask.TaskFunction Then(this IFakeTask instance, FakeTask.TaskFunction.Void action)
        {
            return instance.Then(new FakeTask.TaskFunction(action));
        }
        
        public static FakeTask.TaskFunction Then(this IFakeTask instance, FakeTask.TaskFunction.Action action)
        {
            return instance.Then(new FakeTask.TaskFunction(action));
        }
    }
}
