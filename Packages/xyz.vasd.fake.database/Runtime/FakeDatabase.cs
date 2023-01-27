using System;
using System.Collections.Generic;
using System.Linq;

namespace Xyz.Vasd.Fake.Database
{
    #region Singletons
    public partial class FakeDatabase
    {
        internal Dictionary<Type, object> Singletons = new();

        public bool ContainsSingleton<T>()
        {
            return ContainsSingleton(typeof(T));
        }
        public bool ContainsSingleton(Type type)
        {
            return Singletons.ContainsKey(type);
        }

        public T GetSingleton<T>(T fallback = default)
        {
            return (T)GetSingleton(typeof(T), fallback);
        }
        public object GetSingleton(Type type, object fallback = null)
        {
            if (!Singletons.ContainsKey(type))
            {
                if (fallback == null) return null;
                Singletons[type] = fallback;
            };
            return Singletons[type];
        }

        public void SetSingleton<T>(T value)
        {
            SetSingleton(typeof(T), value);
        }
        public void SetSingleton(Type type, object value)
        {
            Singletons[type] = value;
        }

        public void RemoveSingleton<T>()
        {
            RemoveSingleton(typeof(T));
        }
        public void RemoveSingleton(Type type)
        {
            Singletons.Remove(type);
        }
    }
    #endregion

    #region Pages
    public partial class FakeDatabase
    {
        internal List<FakePage> Pages = new();
        internal List<FakeGroup> Groups = new();
        internal List<Entry> Entries = new();
        internal List<Entry> RemovedEntries = new();

        internal Entry GetEntry(Entry entry)
        {
            return Entries[entry.Id];
        }

        internal void SetEntry(Entry entry)
        {
            Entries[entry.Id] = entry;
        }

        internal Entry PullEntry()
        {
            Entry entry;

            if (RemovedEntries.Count > 0)
            {
                var last = RemovedEntries.Count - 1;
                entry = RemovedEntries[last];
                RemovedEntries.RemoveAt(last);
            }
            else
            {
                entry = new Entry
                {
                    Id = Entries.Count,
                    Page = -1,
                    Index = -1,
                };
                Entries.Add(entry);
            }

            return entry;
        }

        internal FakePage CreatePage(Type[] types)
        {
            var page = new FakePage(Pages.Count, types);

            foreach (var group in Groups)
            {
                if (group.Matches(page)) group.Add(page);
            }

            Pages.Add(page);

            return page;
        }

        internal FakePage GetPage(Entry entry)
        {
            if (entry.Page < 0 || entry.Page >= Pages.Count) return null;
            return Pages[entry.Page];
        }

        internal FakePage FindPage(Type[] types)
        {
            return Pages.Find(page => page.IsEqual(types));
        }

        internal FakePage FindOrCreatePage(Type[] types)
        {
            var page = FindPage(types);

            if (page == null) page = CreatePage(types);

            return page;
        }

        internal FakePage FindOrCreatePage(object[] values)
        {
            var types = values
                .Select(value => value.GetType())
                .Distinct()
                .ToArray();

            return FindOrCreatePage(types);
        }

        internal FakeGroup CreateGroup(Type[] includes, Type[] excludes)
        {
            var group = new FakeGroup(includes, excludes);

            foreach (var page in Pages)
            {
                if (group.Matches(page)) group.Add(page);
            }

            Groups.Add(group);

            return group;
        }

        internal FakeGroup FindGroup(Type[] includes, Type[] excludes)
        {
            return Groups.Find(group => group.IsEqual(includes, excludes));
        }
    }
    #endregion

    #region Data
    public partial class FakeDatabase
    {
        public Entry CreateEntry(params object[] values)
        {
            var page = FindOrCreatePage(values);
            var entry = PullEntry();

            entry = page.Add(entry);

            foreach (var value in values)
            {
                page.SetData(value.GetType(), entry, value);
            }

            SetEntry(entry);

            return entry;
        }

        public void RemoveEntry(Entry entry)
        {
            entry = GetEntry(entry);
            var page = GetPage(entry);

            var moved = page.Remove(entry);
            SetEntry(moved);

            entry = entry.Reset();
            SetEntry(entry);

            RemovedEntries.Add(entry);
        }

        public object GetData(Type type, Entry entry)
        {
            entry = GetEntry(entry);
            var page = GetPage(entry);

            if (page == null) return null;

            return page.GetData(type, entry);
        }

        public void SetData(Type type, Entry entry, object value)
        {
            entry = GetEntry(entry);
            var page = GetPage(entry);

            if (!page.HasData(type))
            {
                var types = page.Types
                    .Concat(new Type[] { type })
                    .ToArray();

                page = Move(entry, types);
            }

            entry = GetEntry(entry);
            page.SetData(type, entry, value);
        }

        public void RemoveData(Type type, Entry entry)
        {
            entry = GetEntry(entry);
            var page = GetPage(entry);
            var types = page.Types
                .Where(t => t != type)
                .ToArray();

            Move(entry, types);
        }

        public FakeGroup FindOrCreateGroup(Type[] includes, Type[] excludes)
        {
            var group = FindGroup(includes, excludes);
            if (group == null) group = CreateGroup(includes, excludes);

            return group;
        }

        public FakeGroup FindOrCreateGroup(FakeGroupDescriptor descriptor)
        {
            var includes = descriptor.Includes.ToArray();
            var excludes = descriptor.Excludes.ToArray();

            return FindOrCreateGroup(includes, excludes);
        }

        public FakeGroupDescriptor CreateGroupDescriptor()
        {
            return new FakeGroupDescriptor(this);
        }

        private FakePage Move(Entry entry, Type[] types)
        {
            var page = GetPage(entry);

            var targetPage = FindOrCreatePage(types);
            var targetEntry = targetPage.Add(entry);

            page.Copy(entry, targetPage, targetEntry);

            var moved = page.Remove(entry);
            SetEntry(moved);
            SetEntry(targetEntry);

            return targetPage;
        }
    }
    #endregion
}
