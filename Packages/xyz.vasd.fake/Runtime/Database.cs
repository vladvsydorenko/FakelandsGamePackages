using System;
using System.Collections.Generic;
using System.Linq;

namespace Xyz.Vasd.Fake
{
    public abstract class DatabaseBase
    {
        internal List<Page> Pages;
        internal List<Group> Groups;
        internal List<Entry> Entries;
        internal List<Entry> RemovedEntries;

        public DatabaseBase()
        {
            Pages = new List<Page>();
            Groups = new List<Group>();
            Entries = new List<Entry>();
            RemovedEntries = new List<Entry>();
        }

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

        internal Page CreatePage(Type[] types)
        {
            var page = new Page(Pages.Count, types);

            foreach (var group in Groups)
            {
                if (group.Matches(page)) group.Add(page);
            }

            Pages.Add(page);

            return page;
        }

        internal Page GetPage(Entry entry)
        {
            if (entry.Page < 0 || entry.Page >= Pages.Count) return null;
            return Pages[entry.Page];
        }
        
        internal Page FindPage(Type[] types)
        {
            return Pages.Find(page => page.IsEqual(types));
        }

        internal Page FindOrCreatePage(Type[] types)
        {
            var page = FindPage(types);

            if (page == null) page = CreatePage(types);

            return page;
        }

        internal Page FindOrCreatePage(object[] values)
        {
            var types = values
                .Select(value => value.GetType())
                .Distinct()
                .ToArray();

            return FindOrCreatePage(types);
        }
    
        internal Group CreateGroup(Type[] includes, Type[] excludes)
        {
            var group = new Group(includes, excludes);

            foreach (var page in Pages)
            {
                if (group.Matches(page)) group.Add(page);
            }

            Groups.Add(group);

            return group;
        }
        
        internal Group FindGroup(Type[] includes, Type[] excludes)
        {
            return Groups.Find(group => group.IsEqual(includes, excludes));
        }
    }

    public class Database : DatabaseBase
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

        public void ClearData(Type type, Entry entry)
        {
            entry = GetEntry(entry);
            var page = GetPage(entry);
            var types = page.Types
                .Where(t => t != type)
                .ToArray();

            Move(entry, types);
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

        public Group FindOrCreateGroup(Type[] includes, Type[] excludes)
        {
            var group = FindGroup(includes, excludes);
            if (group == null) group = CreateGroup(includes, excludes);

            return group;
        }

        private Page Move(Entry entry, Type[] types)
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
}
