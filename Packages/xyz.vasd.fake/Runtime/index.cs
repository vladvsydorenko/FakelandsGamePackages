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


}
