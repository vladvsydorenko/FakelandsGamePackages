using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Xyz.Vasd.Fake
{
    public class index {}

    public class Page
    {
        #region Properties
        public int Count { get; private set; }
        public int Capacity { get; private set; }
        public List<int> Ids { get; private set; }
        #endregion

        #region State
        private Type[] _types;
        private IList[] _layers;
        private Dictionary<Type, IList> _layersMap;
        #endregion

        public Page(Type[] types, int capacity = 0)
        {
            Count = 0;
            Ids = new List<int>(capacity);

            Capacity = capacity;

            _types = types.Distinct().ToArray();
            _layersMap = new Dictionary<Type, IList>();
            _layers = _types
                .Select(type =>
                {
                    Type listType = typeof(List<>).MakeGenericType(type);
                    var list = Activator.CreateInstance(listType, capacity) as IList;
 
                    _layersMap.Add(type, list);

                    return list;
                })
                .ToArray();
        }

        #region Creation
        // Allocate space in layers and return created item's index
        public int Create()
        {
            var index = Count;

            if (index >= Capacity)
            {
                foreach (var layer in _layers)
                {
                    layer.Add(null);
                }
                Capacity++;
            }

            Count++;
            return index;
        }

        public void Set(Type type, int index, object value)
        {
            var layer = _layersMap[type];
            layer[index] = value;
        }

        public object Get(Type type, int index)
        {
            var layer = _layersMap[type];
            return layer[index];
        }

        // returns id of swapped element
        public int Remove(int index)
        {
            var lastIndex = Count;
            MoveItem(lastIndex, index);

            Count--;

            return index;
        }

        private void MoveItem(int from, int to)
        {
            foreach (var layer in _layers)
            {
                layer[to] = layer[from];
                layer[from] = null;
            }
        }
        #endregion

        #region Compares
        public bool Contains(IEnumerable<Type> types)
        {
            return types
                .Distinct()
                .All(t => _types.Contains(t));
        }

        public bool ContainsAny(IEnumerable<Type> types)
        {
            return types
                .Distinct()
                .Any(t => _types.Contains(t));
        }

        public bool ContainsOnly(IEnumerable<Type> types)
        {
            var uniqueTypes = types.Distinct();
            if (uniqueTypes.Count() != types.Count()) return false;

            return uniqueTypes.All(t => _types.Contains(t));
        }
        #endregion
    }

    public class Group
    {
        public List<Page> Pages { get; private set; }
        public Type[] Includes { get; private set; }
        public Type[] Excludes { get; private set; }

        public Group(Type[] inlcudes, Type[] excludes, int capacity = 0)
        {
            Includes = inlcudes.Distinct().ToArray();
            Excludes = excludes.Distinct().ToArray();
            Pages = new List<Page>(capacity);
        }

        public void Add(Page page)
        {
            Pages.Add(page);
        }

        public bool IsEqual(Group group)
        {
            return IsEqual(group.Includes, group.Excludes);
        }

        public bool IsEqual(Type[] includes, Type[] excludes)
        {
            if (Includes.Length != includes.Length || 
                Excludes.Length != excludes.Length) return false;

            var includesMatches = Includes
                .All(t => includes.Contains(t));

            if (!includesMatches) return false;

            var excludeMatches = Excludes
                .All(t => excludes.Contains(t));

            return excludeMatches;
        }
    
        public bool Matches(Page page)
        {
            Debug.Log($"page.Contains(Includes): {page.Contains(Includes)}");
            Debug.Log($"!page.Contains(Excludes): {!page.ContainsAny(Excludes)}");
            return page.Contains(Includes) && !page.ContainsAny(Excludes);
        }
    }

    public class Database
    {
        private struct ItemLink
        {
            public int Id;
            public int Page;
            public int Index;
        }

        #region State
        private List<Page> _pages;
        private List<ItemLink> _links;
        private List<Group> _groups;
        #endregion

        public Database()
        {
            _pages = new List<Page>();
            _links = new List<ItemLink>();
            _groups = new List<Group>();
        }

        #region Items
        public int CreateItem(params object[] values)
        {
            var id = -1;

            var page = CreatePage(values.Select(v => v.GetType()));
            id = page.Create();

            return id;
        }

        public void Set(Type type, int id, object value)
        {
            var link = _links[id];
            var page = _pages[link.Id];

            page.Set(type, link.Index, value);
        }

        public object Get(Type type, int id)
        {
            var link = _links[id];
            var page = _pages[link.Id];

            return page.Get(type, link.Index);
        }

        public void Remove(int id)
        {
            var link = _links[id];
            var page = _pages[link.Page];

            page.Remove(link.Index);
            // now link.Index contains swapped element after remove, so move links
            MoveLink(id, page.Ids[link.Index]);

            _links[id] = link;
        }

        private void MoveLink(int from, int to)
        {
            var toLink = _links[to];
            var fromLink = _links[from];

            toLink.Page = fromLink.Page;
            toLink.Index = fromLink.Index;

            fromLink.Page = -1;
            fromLink.Index = -1;
        }
        #endregion

        #region Pages
        private Page CreatePage(IEnumerable<Type> types)
        {
            var page = _pages.Find(p => p.ContainsOnly(types));

            if (page == null)
            {
                page = new Page(types.ToArray());
                _pages.Add(page);

                _groups.ForEach(g =>
                {
                    if (g.Matches(page)) g.Add(page);
                });
            }

            return page;
        }
        #endregion

        #region Groups
        public Group CreateGroup(params Type[] includes)
        {
            return CreateGroup(includes, new Type[0]);
        }
        public Group CreateGroup(Type[] includes, Type[] excludes)
        {
            var group = _groups.Find(g => g.IsEqual(includes, excludes));
            if (group == null)
            {
                group = new Group(includes, excludes);
                _pages.ForEach(p =>
                {
                    if (group.Matches(p)) group.Add(p);
                });
            }


            return group;
        }
        #endregion
    }
}
