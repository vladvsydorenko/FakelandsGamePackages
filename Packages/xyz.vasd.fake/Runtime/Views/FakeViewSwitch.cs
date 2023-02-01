using System.Collections.Generic;
using UnityEngine;

namespace Xyz.Vasd.Fake.Views
{
    public class FakeViewSwitch : MonoBehaviour
    {
        private IFakeView _current;
        private IFakeView _previous;

        public void OpenView(IFakeView view)
        {
            if (_current == view) return;

            _previous = _current;
            _current = view;
        }

        private void Update()
        {
            if (_current != null && !_current.OpenView()) return;
            if (_previous != null) _previous.CloseView();
        }
    }

    public class FakeViewRoute : MonoBehaviour
    {
        public string Location;
    }

    public class FakeViewRouter : MonoBehaviour
    {
        public string Location;

        private IFakeView[] _currentViews;
        private IFakeView[] _previousViews;

        private string _currentLocation;
        private Dictionary<string, IFakeView[]> _routes;

        private void Awake()
        {
            var routes = GetComponentsInChildren<FakeViewRoute>();
            foreach (var route in routes)
            {
                var views = route.gameObject.GetComponents<IFakeView>();
                _routes[route.Location] = views;
            }
        }

        private void Update()
        {
            if (Location == _currentLocation) return;

            var views = _routes[Location];
            foreach (var view in views)
            {
                view.OpenView();
            }
        }

    }
}
