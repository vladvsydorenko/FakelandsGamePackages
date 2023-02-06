namespace Xyz.Vasd.Fake
{
    public interface ITask
    {
        int Version { get; }
        bool IsCompleted { get; }
        bool Execute(int version);
    }

    public class Task : ITask
    {
        public int Version { get; protected set; } = -1;
        public bool IsCompleted { get; protected set; } = false;

        public bool Execute(int version)
        {
            if (IsCompleted && Version == version) return true;

            if (version != Version)
            {
                if (Version >= 0) OnStop();

                Version = version;
                IsCompleted = false;
                OnStart();
            }

            IsCompleted = OnExecute();
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

namespace Xyz.Vasd.Fake
{
}
