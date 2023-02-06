namespace Xyz.Vasd.Fake.Task
{
    public partial class QuickTask : FakeTask
    {
        private QuickFunction _function;

        public QuickTask(Action action)
        {
            _function = action;
        }
        public QuickTask(ActionVersion action)
        {
            _function = action;
        }
        public QuickTask(Void action)
        {
            _function = action;
        }
        public QuickTask(VoidVersion action)
        {
            _function = action;
        }

        protected override bool OnExecute()
        {
            return _function.Execute(Version);
        }

        public QuickTask Then(IFakeTask task)
        {
            return new QuickTask((version) => Execute(version) && task.Execute(version));
        }
        public QuickTask Then(Action action)
        {
            return Then(new QuickTask(action));
        }
        public QuickTask Then(ActionVersion action)
        {
            return Then(new QuickTask(action));
        }
        public QuickTask Then(Void action)
        {
            return Then(new QuickTask(action));
        }
        public QuickTask Then(VoidVersion action)
        {
            return Then(new QuickTask(action));
        }

        public static QuickTask Create(Action action)
        {
            return new QuickTask(action);
        }
        public static QuickTask Create(ActionVersion action)
        {
            return new QuickTask(action);
        }
        public static QuickTask Create(Void action)
        {
            return new QuickTask(action);
        }
        public static QuickTask Create(VoidVersion action)
        {
            return new QuickTask(action);
        }
    }
}
