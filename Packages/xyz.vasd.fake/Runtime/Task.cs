namespace Xyz.Vasd.Fake
{
    public interface ITask
    {
        bool IsCompleted { get; }
        bool Execute(int version);
    }

    public class Task : ITask
    {
        public int Version { get; protected set; }
        public bool IsCompleted { get; protected set; }

        public bool Execute(int version)
        {
            if (IsCompleted && Version == version) return true;

            Version = version;
            IsCompleted = OnExecute(version);

            return IsCompleted;
        }

        public virtual bool OnExecute(int version)
        {
            return true;
        }
    }

    public class QuickTask : Task
    {
        public delegate bool Action();
        public delegate void VoidAction();

        private Action _action;
        private VoidAction _voidAction;

        public QuickTask(Action action)
        {
            _action = action;
            _voidAction = null;
        }

        public QuickTask(VoidAction action)
        {
            _action = null;
            _voidAction = action;
        }

        public override bool OnExecute(int version)
        {
            if (_voidAction != null)
            {
                _voidAction();
                return true;
            }

            return _action();
        }
    }
}

namespace Xyz.Vasd.Fake
{
    public class Page
    {
        private ITask _openTask;

        private int CloseVersion;

        public void Setup()
        {
            _openTask = new QuickTask(() => { });
        }

        public bool Open()
        {
            return _openTask.Execute(CloseVersion);
        }

        public bool Close(int version)
        {
            CloseVersion = version;
            return true;
        }
    }
}
