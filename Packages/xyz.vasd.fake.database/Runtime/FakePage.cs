using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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

    public abstract class FakePageBase
    {
        internal readonly int Id;

        internal int Count;
        internal int Capacity;

        internal readonly Type[] Types;

        internal List<Entry> Entries;

        internal readonly IList[] Layers;
        internal readonly Dictionary<Type, IList> LayersMap;

        internal FakePageBase(int id, Type[] types)
        {
            Id = id;

            Count = 0;
            Capacity = 0;

            Types = types.Distinct().ToArray();
            Entries = new List<Entry>();

            LayersMap = new Dictionary<Type, IList>();
            Layers = Types
                .Select(type =>
                {
                    var list = (IList)TypeTools.CreateGeneric(typeof(List<>), type);
                    LayersMap[type] = list;
                    return list;
                })
                .ToArray();
        }

        internal void Grow(int size = 4)
        {
            for (int i = 0; i < size; i++)
            {
                Entries.Add(Entry.Null);
            }

            for (int i = 0; i < Layers.Length; i++)
            {
                var layer = Layers[i];

                for (int x = 0; x < size; x++)
                {
                    layer.Add(null);
                }
            }

            Capacity += size;
        }

        internal bool IsEqual(Type[] types)
        {
            if (types.Length != Types.Length) return false;

            return types.All(type => Types.Contains(type));
        }

        internal bool Contains(Type[] types)
        {
            return types.All(type => Types.Contains(type));
        }

        internal bool ContainsAny(Type[] types)
        {
            return types.Any(type => Types.Contains(type));
        }
    }

    public class FakePage : FakePageBase
    {
        internal FakePage(int id, Type[] types) : base(id, types)
        {
        }

        internal Entry Add(Entry entry)
        {
            if (Count >= Capacity) Grow();
            var index = Count;

            var result = entry;
            result.Page = Id;
            result.Index = index;

            Entries[index] = result;

            Count++;

            return result;
        }

        internal Entry Remove(Entry entry)
        {
            var index = entry.Index;
            var lastIndex = Count - 1;
            var lastEntry = Entries[lastIndex];

            foreach (var layer in Layers)
            {
                layer[index] = layer[lastIndex];
                layer[lastIndex] = null;
            }

            lastEntry.Index = index;
            Entries[index] = lastEntry;

            Entries[lastIndex] = Entry.Null;

            Count--;

            return lastEntry;
        }

        internal void Copy(Entry entry, FakePage target, Entry targetEntry)
        {
            for (int i = 0; i < Layers.Length; i++)
            {
                var type = Types[i];

                if (!target.HasData(type)) continue;

                var layer = Layers[i];
                target.SetData(type, targetEntry, layer[i]);
            }
        }

        internal bool HasData(Type type)
        {
            return LayersMap.ContainsKey(type);
        }

        internal object GetData(Type type, Entry entry)
        {
            if (!LayersMap.ContainsKey(type)) return null;

            var layer = LayersMap[type];
            return layer[entry.Index];
        }

        internal void SetData(Type type, Entry entry, object value)
        {
            var layer = LayersMap[type];
            layer[entry.Index] = value;
        }
    }
}
