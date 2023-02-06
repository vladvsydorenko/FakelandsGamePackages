using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Xyz.Vasd.Fake.Task;

//namespace Xyz.Vasd.Fake.Router{class index{}}

namespace Xyz.Vasd.Fake.Router
{
    public interface IRoute
    {
        string GetPath();

        bool Open();
        bool Close();
    }

    public class Router
    {
        private List<IRoute> _routes = new();

        public void AddRoute(IRoute route)
        {
            _routes.Add(route);
        }

        public void Process(string location)
        {
            var uri = new Uri(location);
            IRoute foundRoute = null;

            foreach (var route in _routes)
            {
                var parts = route.GetPath().Split('/');

                var len = Mathf.Min(parts.Length, uri.Segments.Length);
                for (int i = 0; i < len; i++)
                {
                    var part = parts[i];
                    var uriPart = uri.Segments[i];
                }
            }
        }
    }
}
