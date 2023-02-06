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
        public bool IsStarted { get; protected set; }
        public bool IsCompleted { get; protected set; }

        public bool Execute(int version)
        {
            if (IsCompleted && Version == version) return true;

            if (Version < 0) Start(version);
            else if (Version != version) Restart(version);

            IsCompleted = OnExecute();

            return IsCompleted;
        }

        private void Start(int version)
        {
            Version = version;
            OnStart();
            IsStarted = true;
        }

        private void Restart(int version)
        {
            OnReset();
            IsStarted = false;
            Start(version);
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

    public class QuickTaskFunction
    {
        public delegate bool Action();
        public delegate bool ActionWithVersion(int version);
        public delegate void VoidAction();
        public delegate void VoidActionWithVersion(int version);

        private Action _action = null;
        private ActionWithVersion _actionWithVersion = null;
        private VoidAction _voidAction = null;
        private VoidActionWithVersion _voidActionWithVersion = null;

        public QuickTaskFunction(Action action)
        {
            _action = action;
        }

        public QuickTaskFunction(ActionWithVersion action)
        {
            _actionWithVersion = action;
        }

        public QuickTaskFunction(VoidAction action)
        {
            _voidAction = action;
        }

        public QuickTaskFunction(VoidActionWithVersion action)
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

        public static implicit operator QuickTaskFunction(Action action)
        {
            return new QuickTaskFunction(action);
        }

        public static implicit operator QuickTaskFunction(ActionWithVersion action)
        {
            return new QuickTaskFunction(action);
        }

        public static implicit operator QuickTaskFunction(VoidAction action)
        {
            return new QuickTaskFunction(action);
        }

        public static implicit operator QuickTaskFunction(VoidActionWithVersion action)
        {
            return new QuickTaskFunction(action);
        }
    }

    public class QuickTask : Task
    {
        private QuickTaskFunction _action;

        public QuickTask(QuickTaskFunction.Action action)
        {
            _action = action;
        }

        public QuickTask(QuickTaskFunction.ActionWithVersion action)
        {
            _action = action;
        }

        public QuickTask(QuickTaskFunction.VoidAction action)
        {
            _action = action;
        }

        public QuickTask(QuickTaskFunction.VoidActionWithVersion action)
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

        public QuickTask Then(QuickTaskFunction.Action action)
        {
            return Then(new QuickTask(action));
        }
        public QuickTask Then(QuickTaskFunction.ActionWithVersion action)
        {
            return Then(new QuickTask(action));
        }
        public QuickTask Then(QuickTaskFunction.VoidAction action)
        {
            return Then(new QuickTask(action));
        }
        public QuickTask Then(QuickTaskFunction.VoidActionWithVersion action)
        {
            return Then(new QuickTask(action));
        }

        public static QuickTask Create(QuickTaskFunction.Action action)
        {
            return new QuickTask(action);
        }
        public static QuickTask Create(QuickTaskFunction.ActionWithVersion action)
        {
            return new QuickTask(action);
        }
        public static QuickTask Create(QuickTaskFunction.VoidAction action)
        {
            return new QuickTask(action);
        }
        public static QuickTask Create(QuickTaskFunction.VoidActionWithVersion action)
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
