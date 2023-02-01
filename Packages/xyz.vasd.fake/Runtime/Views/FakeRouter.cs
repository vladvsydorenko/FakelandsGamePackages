using System.Collections.Generic;
using UnityEngine;

namespace Xyz.Vasd.Fake.Views
{
    [AddComponentMenu("Fake/[Fake] Router")]
    public class FakeRouter : MonoBehaviour
    {
        public string Location;

        private string _activeLocation;

        private IFakeView _currentView;
        private List<IFakeView> _views = new();
        private Dictionary<string, IFakeView> _routes = new();

        private void Awake()
        {
            var routes = GetComponentsInChildren<FakeRoute>(includeInactive: true);
            foreach (var route in routes)
            {
                var view = route.gameObject.GetComponent<IFakeView>();
                _routes[route.Location] = view;
                _views.Add(view);
            }
        }

        private void Update()
        {
            RefreshRouter();
            UpdateRouter();
        }

        public void RefreshRouter()
        {
            if (Location != _activeLocation)
            {
                IFakeView current = null;

                if (_routes.ContainsKey(Location))
                {
                    current = _routes[Location];
                }

                _currentView = current;
                _activeLocation = Location;
            }
        }

        public void UpdateRouter()
        {
            // wait for current to open
            if (_currentView != null && !_currentView.OpenView()) return;

            // close all views except current
            foreach (var view in _views)
            {
                if (view == _currentView) continue;

                view.CloseView();
            }
        }
    }
}
