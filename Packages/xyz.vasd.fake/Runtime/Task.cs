using UnityEngine;

namespace Xyz.Vasd.Fake
{
    public interface ITask
    {
        bool Execute(int version);
    }

    public class Task : ITask
    {
        public int Version { get; protected set; } = -1;
        public bool IsCompleted;

        public bool Execute(int version)
        {
            if (IsCompleted && Version == version) return true;

            if (Version < 0) return Start(version);
            if (Version != version) return Restart(version);

            IsCompleted = OnExecute();

            return IsCompleted;
        }

        private bool Start(int version)
        {
            Version = version;
            OnStart();
            return OnExecute();
        }

        private bool Restart(int version)
        {
            OnReset();
            return Start(version);
        }

        protected virtual void OnStart()
        {

        }

        protected virtual bool OnExecute()
        {
            return true;
        }

        protected virtual void OnReset()
        {

        }
    }

    public class TaskFunction
    {
        public delegate bool Action();
        public delegate bool ActionWithVersion(int version);
        public delegate void VoidAction();
        public delegate void VoidActionWithVersion(int version);

        private Action _action = null;
        private ActionWithVersion _actionWithVersion = null;
        private VoidAction _voidAction = null;
        private VoidActionWithVersion _voidActionWithVersion = null;

        public TaskFunction(Action action)
        {
            _action = action;
        }

        public TaskFunction(ActionWithVersion action)
        {
            _actionWithVersion = action;
        }

        public TaskFunction(VoidAction action)
        {
            _voidAction = action;
        }

        public TaskFunction(VoidActionWithVersion action)
        {
            _voidActionWithVersion = action;
        }

        public bool Execute(int version)
        {
            if (_voidAction != null)
            {
                _voidAction();
                return true;
            }

            if (_voidActionWithVersion != null)
            {
                _voidActionWithVersion(version);
                return true;
            }

            if (_action != null) return _action();
            return _actionWithVersion(version);
        }

        public static implicit operator TaskFunction(Action action)
        {
            return new TaskFunction(action);
        }

        public static implicit operator TaskFunction(ActionWithVersion action)
        {
            return new TaskFunction(action);
        }

        public static implicit operator TaskFunction(VoidAction action)
        {
            return new TaskFunction(action);
        }

        public static implicit operator TaskFunction(VoidActionWithVersion action)
        {
            return new TaskFunction(action);
        }
    }

    public class QuickTask : Task
    {
        private TaskFunction _action;

        public QuickTask(TaskFunction.Action action)
        {
            _action = action;
        }

        public QuickTask(TaskFunction.ActionWithVersion action)
        {
            _action = action;
        }

        public QuickTask(TaskFunction.VoidAction action)
        {
            _action = action;
        }

        public QuickTask(TaskFunction.VoidActionWithVersion action)
        {
            _action = action;
        }

        protected override bool OnExecute()
        {
            return _action.Execute(Version);
        }

        public QuickTask Then(ITask task)
        {
            return new QuickTask((int version) => Execute(version) && task.Execute(version));
        }

        public QuickTask Then(TaskFunction.Action action)
        {
            return Then(new QuickTask(action));
        }
        public QuickTask Then(TaskFunction.ActionWithVersion action)
        {
            return Then(new QuickTask(action));
        }
        public QuickTask Then(TaskFunction.VoidAction action)
        {
            return Then(new QuickTask(action));
        }
        public QuickTask Then(TaskFunction.VoidActionWithVersion action)
        {
            return Then(new QuickTask(action));
        }

        public static QuickTask Create(TaskFunction.Action action)
        {
            return new QuickTask(action);
        }
        public static QuickTask Create(TaskFunction.ActionWithVersion action)
        {
            return new QuickTask(action);
        }
        public static QuickTask Create(TaskFunction.VoidAction action)
        {
            return new QuickTask(action);
        }
        public static QuickTask Create(TaskFunction.VoidActionWithVersion action)
        {
            return new QuickTask(action);
        }
    }
}

namespace Xyz.Vasd.Fake
{
    public class Page
    {
        private ITask _openTask;
        private ITask _closeTask;

        private int CloseVersion;

        private Animator Animator;

        public void Setup()
        {
            _openTask = QuickTask
                .Create(() => 
                {
                    Animator.Play(0);
                })
                .Then(() => 
                {
                    return Animator.GetBool("open");
                });

            _closeTask = QuickTask
                .Create(() => 
                {
                    Animator.SetBool("close", true);
                })
                .Then(() => 
                {
                    return Animator.GetBool("closed");
                });
        }

        public bool Open()
        {
            return _openTask.Execute(version: 0);
        }

        public bool Close()
        {
            return _closeTask.Execute(version: 0);
        }
    }
}
