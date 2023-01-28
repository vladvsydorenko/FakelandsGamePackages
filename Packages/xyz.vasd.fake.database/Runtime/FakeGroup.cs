using System;
using System.Collections.Generic;
using System.Linq;

namespace Xyz.Vasd.Fake.Database
{
    public class FakeGroup
    {
        internal Type[] Includes;
        internal Type[] Excludes;

        public List<FakePage> Pages { get; private set; }

        internal FakeGroup(Type[] includes, Type[] excludes)
        {
            Includes = includes.Distinct().ToArray();
            Excludes = excludes.Distinct().ToArray();
            Pages = new List<FakePage>();
        }

        internal void Add(FakePage page)
        {
            Pages.Add(page);
        }

        internal bool IsEqual(Type[] includes, Type[] excludes)
        {
            if (includes.Length != Includes.Length || excludes.Length != Excludes.Length) return false;

            if (!includes.All(type => Includes.Contains(type))) return false;
            if (!excludes.All(type => Excludes.Contains(type))) return false;

            return true;
        }

        internal bool Matches(FakePage page)
        {
            return page.Contains(Includes) && !page.ContainsAny(Excludes);
        }
    }
}
