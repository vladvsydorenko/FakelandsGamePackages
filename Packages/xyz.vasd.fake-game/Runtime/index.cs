using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Xyz.Vasd.Fake.Database;

namespace Xyz.Vasd.FakeGame
{
    public class index {}

    public class Router : MonoBehaviour
    {
        public string Location { get; private set; }
        public readonly List<string> History;

        public void SetLocation(string location)
        {
            if (!History.Contains(location)) History.Add(Location);
            Location = location;
        }
    }

    public interface IWindowView
    {
        string GetWindowLocation();

        void OpenWindow();
        bool IsWindowOpen();
        bool IsWindowOpening();

        void CloseWindow();
        bool IsWindowClosed();
        bool IsWindowClosing();

        bool IsWindowBusy();
    }

    public class BaseWindowView : MonoBehaviour, IWindowView
    {
        protected virtual string WindowLocation { get; set; }

        public virtual bool IsWindowBusy()
        {
            return false;
        }


        public virtual void OpenWindow()
        {
            gameObject.SetActive(true);
        }

        public virtual bool IsWindowOpen()
        {
            return isActiveAndEnabled;
        }

        public virtual bool IsWindowOpening()
        {
            return false;
        }


        public virtual void CloseWindow()
        {
            gameObject.SetActive(false);
        }

        public virtual bool IsWindowClosed()
        {
            return !isActiveAndEnabled;
        }

        public virtual bool IsWindowClosing()
        {
            return false;
        }

        public string GetWindowLocation()
        {
            return WindowLocation;
        }
    }

    public class WindowSwitch : MonoBehaviour
    {
        public Router Router;

        private string _current;
        private string _previous;
        private List<IWindowView> _windows;

        private void Awake()
        {
            _windows = GetComponentsInChildren<IWindowView>().ToList();
        }

        private void Update()
        {
            var location = Router.Location;

            if (location != _current)
            {
                _previous = _current;
                _current = location;
            }

            var currentWindow = _windows.Find(window => window.GetWindowLocation() == _current);
            if (currentWindow == null)
            {
                // TODO: Log error
                return;
            }

            var previousWindow = _windows.Find(window => window.GetWindowLocation() == _previous);

            currentWindow.OpenWindow();
            if (currentWindow.IsWindowOpen())
            {
                previousWindow.CloseWindow();
            }
        }
    }


}
