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

        internal void Set(int page, int index)
        {
            Page = page;
            Index = index;
        }
        internal Entry Reset()
        {
            return new Entry(Id, -1, -1);
        }
    }

    public class Page
    {
        public int Id { get; private set; }
        public int Count { get; private set; }
        public int Capacity { get; private set; }
        public List<Entry> Entries;

        public Page(int id)
        {
            Id = id;
            Count = 0;
            Capacity = 0;
            Entries = new List<Entry>(Capacity);
        }

        internal Entry Create(Entry entry)
        {
            entry.Page = Id;
            entry.Index = Count;

            if (Count >= Capacity)
            {
                Entries.Add(entry);
                Capacity++;
            }
            else
            {
                Entries[entry.Index] = entry;
            }

            Count++;

            return entry;
        }

        internal Entry Remove(Entry target)
        {
            var lastIndex = Entries.Count - 1;
            var targetIndex = target.Index;

            var last = Entries[lastIndex];
            last.Index = target.Index;
            Entries[targetIndex] = last;

            Entries[lastIndex] = target.Reset();

            Count--;

            return last;
        }
    }

    public class Database
    {
        internal List<Page> Pages;
        internal List<Entry> Entries;

        public Database()
        {
            Pages = new List<Page>();
            Entries = new List<Entry>();
        }

        public void Remove(Entry entry)
        {
            var page = Pages[entry.Page];
            var swapped = page.Remove(entry);

            Entries[swapped.Id] = swapped;
            Entries[entry.Id] = entry.Reset();
        }
    }
}

namespace Xyz.Vasd.Fake.rNd2
{
    public static class TypeTools
    {
        public static object CreateGeneric(Type baseType, Type valueType, params object[] args)
        {
            Type type = baseType.MakeGenericType(valueType);

            return Activator.CreateInstance(type, args);
        }
    }

    public class Entry
    {
        public readonly int Id;

        #region Internal
        internal DataPage Page;
        internal int Index;

        internal Entry(int id, DataPage page = null, int index = -1)
        {
            Id = id;
            Page = page;
            Index = index;
        }

        internal void Set(DataPage page, int index)
        {
            Page = page;
            Index = index;
        }
        #endregion
    }

    public class DataPage
    {
        public readonly int Id;
        public readonly Type[] Types;

        public int Count { get; private set; }
        public int Capacity { get; private set; }
        public List<Entry> Entries { get; private set; }

        #region Internal
        internal IList[] Layers;
        internal Dictionary<Type, IList> LayersMap;

        internal DataPage(int id, Type[] types, int capacity = 0)
        {
            Id = id;
            Types = types.Distinct().ToArray();
            Entries = new List<Entry>(capacity);
            LayersMap = new Dictionary<Type, IList>(Types.Length);

            Layers = Types
                .Select(type =>
                {
                    var list = (IList)TypeTools.CreateGeneric(typeof(List<>), type, capacity);

                    LayersMap[type] = list;

                    return list;
                })
                .ToArray();

            Count = 0;
            Capacity = capacity;
        }

        internal void Create(Entry entry)
        {
            var index = Count;

            if (index >= Capacity) Grow(4);

            entry.Set(this, index);
            Entries[index] = entry;

            Count++;
        }
        internal void Remove(Entry entry)
        {
            Move(Count - 1, entry.Index);
            Count--;
        }
        internal void Move(Entry entry, Entry targetEntry, DataPage target)
        {
            for (int i = 0; i < Layers.Length; i++)
            {
                var type = Types[i];

                if (target.Has(type))
                {
                    var layer = Layers[i];
                    target.Set(type, targetEntry, layer[entry.Index]);
                }
            }
        }
        internal void Set(Type type, Entry entry, object value)
        {

        }

        internal bool Has(Type type)
        {
            return LayersMap.ContainsKey(type);
        }
        internal bool IsEqual(Type[] types)
        {
            var comparingTypes = types.Distinct().ToArray();

            if (Types.Length != comparingTypes.Length) return false;

            return comparingTypes.All(type => Types.Contains(type));
        }
        internal bool Contains(Type[] types)
        {
            var comparingTypes = types.Distinct().ToArray();

            if (Types.Length < comparingTypes.Length) return false;

            return comparingTypes.All(type => Types.Contains(type));
        }
        #endregion

        #region Private
        private void Grow(int count)
        {
            foreach (var layer in Layers)
            {
                for (int i = 0; i < count; i++)
                {
                    layer.Add(null);
                }
            }

            for (int i = 0; i < count; i++)
            {
                Entries.Add(null);
            }

            Capacity += count;
        }

        private void Move(int from, int to)
        {
            foreach (var layer in Layers)
            {
                layer[to] = layer[from];
                layer[from] = null;
            }
            
            Entries[to].Set(null, -1);
            Entries[to] = Entries[from];
            Entries[from] = null;
        }
        #endregion
    }
}
