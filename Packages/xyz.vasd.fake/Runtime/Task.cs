namespace Xyz.Vasd.Fake
{
    public interface ITask
    {
        bool Execute(int version);
    }

    public class Task : ITask
    {
        public int Version { get; protected set; } = -1;
        public int ExecuteVersion { get; protected set; } = -1;
        public int CompletedVersion { get; protected set; } = -1;

        public bool Execute(int version)
        {
            if (CompletedVersion == version) return true;

            // new version and was executing in previous version
            if (Version > -1 && Version != version && ExecuteVersion == Version)
            {
                OnReset();
            }

            Version = version;

            if (OnExecute())
            {
                // completed
                CompletedVersion = Version;
            }
            else
            {
                // not completed yet, executing...
                ExecuteVersion = Version;
            }


            return true;
        }

        public virtual void OnReset()
        {

        }

        public virtual bool OnExecute()
        {
            return true;
        }
    }

    public class QuickTask : Task
    {
        public delegate bool Action();
        public delegate bool ActionWithVersion(int version);
        public delegate void VoidAction();
        public delegate void VoidActionWithVersion(int version);

        private Action _action;
        private ActionWithVersion _actionWithVersion;
        private VoidAction _voidAction;
        private VoidActionWithVersion _voidActionWithVersion;

        public QuickTask(Action action)
        {
            _action = action;
            _actionWithVersion = null;
            _voidAction = null;
            _voidActionWithVersion = null;
        }

        public QuickTask(ActionWithVersion action)
        {
            _action = null;
            _actionWithVersion = action;
            _voidAction = null;
            _voidActionWithVersion = null;
        }

        public QuickTask(VoidAction action)
        {
            _action = null;
            _actionWithVersion = null;
            _voidAction = action;
            _voidActionWithVersion = null;
        }

        public QuickTask(VoidActionWithVersion action)
        {
            _action = null;
            _actionWithVersion = null;
            _voidAction = null;
            _voidActionWithVersion = action;
        }

        public override bool OnExecute()
        {
            if (_voidAction != null)
            {
                _voidAction();
                return true;
            }

            if (_voidActionWithVersion != null)
            {
                _voidActionWithVersion(Version);
                return true;
            }

            if (_action != null) return _action();
            return _actionWithVersion(Version);
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
