using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Xyz.Vasd.Fake
{


    public class Route
    {

    }

    public interface IRouteDB
    {
        Route[] GetRoutes(string location);
    }

    public class Router
    {
        public string Location { get; private set; }

        private Dictionary<string, Route[]> _routes = new();

        public void Update()
        {

        }
    }
}
