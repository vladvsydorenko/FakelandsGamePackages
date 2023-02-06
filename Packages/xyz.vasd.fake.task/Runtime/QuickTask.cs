namespace Xyz.Vasd.Fake
{
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
