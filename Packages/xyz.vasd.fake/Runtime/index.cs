using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Xyz.Vasd.Fake
{
    public struct Entry
    {
        public readonly int Id;

        internal int Page;
        internal int Index;

        internal Entry(int id, int page = -1, int index = -1)
        {
            Id = id;
            Page = page;
            Index = index;
        }

        internal Entry Reset()
        {
            return new Entry(Id, -1, -1);
        }

        internal bool Exists()
        {
            return Id >= 0 && Page >= 0 && Index >= 0;
        }

        internal static readonly Entry Null = new(-1, -1, -1);
    }

    public class Page
    {
        internal int Id;
        internal int Count;
        internal int Capacity;

        internal readonly Type[] Types;
        internal readonly IList[] Layers;
        internal readonly Dictionary<Type, IList> LayersMap;

        internal readonly List<Entry> Entries;        

        public Page(int id, Type[] types)
        {
            Id = id;
            Count = 0;
            Capacity = 0;
            Entries = new List<Entry>();

            Types = types
                .Distinct()
                .ToArray();

            LayersMap = new Dictionary<Type, IList>(Types.Length);
            Layers = Types
                .Select(type =>
                {
                    var list = (IList)TypeTools.CreateGeneric(typeof(List<>), type);
                    LayersMap[type] = list;
                    return list;
                })
                .ToArray();
        }

        internal Entry Add(Entry entry)
        {
            if (Count >= Capacity)
            {
                var size = 4;
                Grow(size);
                Capacity += size;
            }

            var index = Count;
            
            entry.Page = Id;
            entry.Index = index;

            Entries[index] = entry;

            Count++;

            return entry;
        }

        internal Entry Remove(Entry entry)
        {
            var index = entry.Index;
            var lastIndex = Count - 1;

            var lastEntry = Entries[lastIndex];

            lastEntry.Index = index;
            Entries[index] = lastEntry;
            Entries[lastIndex] = Entry.Null;

            foreach (var layer in Layers)
            {
                layer[lastIndex] = null;
            }

            Count--;

            return lastEntry;
        }

        internal object GetData(Type type, Entry entry)
        {
            var list = LayersMap[type];
            return list[entry.Index];
        }

        internal void SetData(Type type, Entry entry, object value)
        {
            var list = LayersMap[type];
            list[entry.Index] = value;
        }

        internal bool IsEqual(Type[] types)
        {
            if (types.Length != Types.Length) return false;
            return types.All(t => Types.Contains(t));
        }

        private void Grow(int size)
        {
            // grow entries
            for (int i = 0; i < size; i++)
            {
                Entries.Add(Entry.Null);
            }

            // grow layers
            foreach (var layer in Layers)
            {
                for (int i = 0; i < size; i++)
                {
                    layer.Add(null);
                }
            }
        }
    }

    public class Database
    {
        internal List<Page> Pages;
        internal List<Entry> Entries;
        internal List<int> RemovedEntries;

        public Database()
        {
            Pages = new List<Page>();
            Entries = new List<Entry>();
            RemovedEntries = new List<int>();
        }

        public Entry CreateEntry(params object[] values)
        {
            var types = values
                .Select(x => x.GetType())
                .Distinct()
                .ToArray();

            var page = FindOrCreatePage(types);

            Entry entry;
            if (RemovedEntries.Count > 0)
            {
                var last = RemovedEntries.Count - 1;
                entry = Entries[RemovedEntries[last]];
                RemovedEntries.RemoveAt(last);
            }
            else
            {
                entry = new Entry(Entries.Count);
                Entries.Add(entry);
            }

            entry = page.Add(entry);
            Entries[entry.Index] = entry;

            foreach (var value in values)
            {
                page.SetData(value.GetType(), entry, value);
            }

            return entry;
        }

        public void RemoveEntry(Entry entry)
        {
            // refresh entry
            entry = Entries[entry.Id];

            var page = Pages[entry.Page];
            var moved = page.Remove(entry);

            Entries[moved.Id] = moved;
            Entries[entry.Id] = entry.Reset();

            RemovedEntries.Add(entry.Id);
        }

        public bool Exists(Entry entry)
        {
            entry = Entries[entry.Id];
            return entry.Exists();
        }

        public object GetData(Type type, Entry entry)
        {
            // refresh entry as page and index could be changed
            entry = Entries[entry.Id];

            if (!entry.Exists()) return null;

            var page = Pages[entry.Page];
            return page.GetData(type, entry);
        }

        public void SetData(Type type, Entry entry, object value)
        {
            entry = Entries[entry.Id];

            if (!entry.Exists()) return;

            var page = Pages[entry.Page];
            page.SetData(type, entry, value);
        }

        internal Page FindPage(Type[] types)
        {
            return Pages.Find(page => page.IsEqual(types));
        }

        internal Page FindOrCreatePage(Type[] types)
        {
            var page = FindPage(types);
            if (page != null) return page;

            var id = Pages.Count;
            page = new Page(id, types);

            Pages.Add(page);

            return page;
        }
    }
}
