using System;
using System.Collections.Generic;

namespace Xyz.Vasd.Fake.Database
{
    public class FakeGroupDescriptor
    {
        internal List<Type> Includes;
        internal List<Type> Excludes;
        internal FakeDatabase Database;

        public FakeGroupDescriptor(FakeDatabase database)
        {
            Includes = new List<Type>();
            Excludes = new List<Type>();
            Database = database;
        }

        public FakeGroup ToGroup()
        {
            return Database.FindOrCreateGroup(this);
        }

        public FakeGroupDescriptor Include(params Type[] types)
        {
            foreach (var type in types)
            {
                if (Includes.Contains(type)) continue; 
                Includes.Add(type);
            }

            return this;
        }
        public FakeGroupDescriptor Include<T>()
        {
            return Include(typeof(T));
        }
        public FakeGroupDescriptor Include<T, T2>()
        {
            return Include(typeof(T), typeof(T2));
        }
        public FakeGroupDescriptor Include<T, T2, T3>()
        {
            return Include(typeof(T), typeof(T2), typeof(T3));
        }

        public FakeGroupDescriptor Exclude(params Type[] types)
        {
            foreach (var type in types)
            {
                if (Excludes.Contains(type)) continue;
                Excludes.Add(type);
            }

            return this;
        }

        public FakeGroupDescriptor Exclude<T>()
        {
            return Exclude(typeof(T));
        }
        public FakeGroupDescriptor Exclude<T, T2>()
        {
            return Exclude(typeof(T), typeof(T2));
        }
        public FakeGroupDescriptor Exclude<T, T2, T3>()
        {
            return Exclude(typeof(T), typeof(T2), typeof(T3));
        }
    }
}
