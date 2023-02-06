namespace Xyz.Vasd.Fake.Task
{
    public partial class QuickTask
    {
        public delegate void Void();
        public delegate void VoidVersion(int version);
        public delegate bool Action();
        public delegate bool ActionVersion(int version);

        internal class QuickFunction
        {
            private Void _void = null;
            private VoidVersion _voidVersion = null;
            private Action _action = null;
            private ActionVersion _actionVersion = null;

            public QuickFunction(Void action)
            {
                _void = action;
            }
            public QuickFunction(VoidVersion action)
            {
                _voidVersion = action;
            }
            public QuickFunction(Action action)
            {
                _action = action;
            }
            public QuickFunction(ActionVersion action)
            {
                _actionVersion = action;
            }

            public bool Execute(int version)
            {
                if (_void != null)
                {
                    _void();
                    return true;
                }

                if (_voidVersion != null)
                {
                    _voidVersion(version);
                    return true;
                }

                if (_action != null) return _action();
                return _actionVersion(version);
            }

            public static implicit operator QuickFunction(Void action)
            {
                return new QuickFunction(action);
            }
            public static implicit operator QuickFunction(VoidVersion action)
            {
                return new QuickFunction(action);
            }
            public static implicit operator QuickFunction(Action action)
            {
                return new QuickFunction(action);
            }
            public static implicit operator QuickFunction(ActionVersion action)
            {
                return new QuickFunction(action);
            }
        }
    }
}
