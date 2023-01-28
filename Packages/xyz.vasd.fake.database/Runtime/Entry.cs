namespace Xyz.Vasd.Fake.Database
{
    public struct Entry
    {
        internal int a;
        internal int Id;
        internal int Page;
        internal int Index;

        internal Entry Reset()
        {
            return new Entry
            {
                Id = Id,
                Page = -1,
                Index = -1,
            };
        }

        internal static readonly Entry Null = new Entry();
    }
}
