namespace Xyz.Vasd.Fake.Task
{
    public partial class FakeTask : IFakeTask
    {
        public int Version { get; protected set; } = -1;
        public bool IsCompleted { get; protected set; } = false;

        public bool Execute(int version)
        {
            if (IsCompleted && Version == version) return true;

            if (version != Version)
            {
                if (Version >= 0 && !IsCompleted) OnStop();

                Version = version;
                IsCompleted = false;
                OnStart();
            }

            IsCompleted = OnExecute();

            if (IsCompleted) OnStop();

            return IsCompleted;
        }

        protected virtual void OnStart()
        {

        }

        protected virtual bool OnExecute()
        {
            return true;
        }

        protected virtual void OnStop()
        {

        }
    }
}
