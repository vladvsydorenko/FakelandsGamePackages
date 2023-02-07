namespace Xyz.Vasd.Fake.Task
{
    public interface IFakeTask
    {
        int Version { get; }
        bool IsCompleted { get; }
        bool Execute(int version);
    }
}
