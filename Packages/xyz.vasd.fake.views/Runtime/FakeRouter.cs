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
                var view = route.View;
                if (view == null) continue;

                _routes[route.Location] = view;
                _views.Add(view);
                var viewGo = view.GetViewGameObject();
                if (viewGo != null) viewGo.SetActive(false);
            }
        }

        private void Update()
        {
            RefreshRouter();
            UpdateRouter();
        }

        public void RefreshRouter()
        {
            Debug.Log($"Location: {Location} {_routes.ContainsKey(Location)}");
            if (Location == _activeLocation || !_routes.ContainsKey(Location)) return;

            IFakeView current = null;

            if (_routes.ContainsKey(Location))
            {
                current = _routes[Location];
            }

            _currentView = current;
            _activeLocation = Location;
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
