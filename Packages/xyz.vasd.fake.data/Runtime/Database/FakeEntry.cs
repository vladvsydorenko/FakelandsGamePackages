namespace Xyz.Vasd.Fake.Data.Database
{
    public struct FakeEntry
    {
        internal int a;
        internal int Id;
        internal int Page;
        internal int Index;

        internal FakeEntry Reset()
        {
            return new FakeEntry
            {
                Id = Id,
                Page = -1,
                Index = -1,
            };
        }

        internal static readonly FakeEntry Null = new FakeEntry();
    }
}
