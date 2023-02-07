using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Xyz.Vasd.Fake.Router
{
    public class FakeRouter : MonoBehaviour
    {
        public string Path { get; private set; }
        public string PreviousPath { get; private set; }
        public int PathVersion = 0;

        public List<IFakeRoute> Routes { get; private set; }

        private List<IFakeRoute> _openRoutes;
        private List<IFakeRoute> _closeRoutes;

        private bool _isOpen;
        private bool _isBusy;

        [Header("Debug")]
        [SerializeField]
        private string _debugPath;

        private void Awake()
        {
            Routes = GetComponentsInChildren<IFakeRoute>(includeInactive: true).ToList();
        }

        public void SetPath(string path)
        {
            if (_isBusy || Path == path) return;

            var routes = Routes.FindAll(r => r.MatchRoute(path));
            if (routes.Count < 1) return;

            PreviousPath = Path;
            Path = path;

            _isBusy = true;
            _isOpen = false;

            _closeRoutes = _openRoutes;
            _openRoutes = routes;

            PathVersion++;
        }

        public void Update()
        {
            if (_debugPath.Length > 0) SetPath(_debugPath);
                
            if (!_isBusy) return;

            if (!_isOpen) _isOpen = _openRoutes.All(route => route.OpenRoute(PathVersion));
            if (!_isOpen) return;

            _isBusy = _closeRoutes != null && !_closeRoutes.All(route => route.CloseRoute(PathVersion));
        }
    }
}
