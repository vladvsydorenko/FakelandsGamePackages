namespace Xyz.Vasd.Fake.Task
{
    public partial class FakeTask
    {
        public class TaskFunction : FakeTask
        {
            public delegate void Void(int version);
            public delegate bool Action(int version);

            private readonly Void _void = null;
            private readonly Action _action = null;

            public TaskFunction(Void action)
            {
                _void = action;
            }
            public TaskFunction(Action action)
            {
                _action = action;
            }

            protected override bool OnExecute()
            {
                if (_void != null)
                {
                    _void(Version);
                    return true;
                }

                if (_action != null) return _action(Version);

                return true;
            }

            public static TaskFunction Create(Void action)
            {
                return new TaskFunction(action);
            }
            public static TaskFunction Create(Action action)
            {
                return new TaskFunction(action);
            }

            public static implicit operator TaskFunction(Void action)
            {
                return new TaskFunction(action);
            }
            public static implicit operator TaskFunction(Action action)
            {
                return new TaskFunction(action);
            }
        }

        public static TaskFunction Function(TaskFunction.Void action)
        {
            return new TaskFunction(action);
        }

        public static TaskFunction Function(TaskFunction.Action action)
        {
            return new TaskFunction(action);
        }
    }
}
