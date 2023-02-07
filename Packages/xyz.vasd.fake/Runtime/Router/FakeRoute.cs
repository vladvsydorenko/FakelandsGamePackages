using UnityEngine;
using Xyz.Vasd.Fake.Task;

namespace Xyz.Vasd.Fake.Router
{
    public interface IFakeRoute
    {
        bool MatchRoute(string path);
        bool OpenRoute();
        bool CloseRoute();
    }

    public class FakeRoute : MonoBehaviour, IFakeRoute
    {
        public string Path;

        protected virtual void Awake()
        {
            gameObject.SetActive(false);
        }

        public virtual bool MatchRoute(string path)
        {
            return path == Path;
        }

        public bool OpenRoute()
        {
            return OnOpenRoute();
        }

        public bool CloseRoute()
        {
            return OnCloseRoute();
        }

        public virtual bool OnOpenRoute()
        {
            gameObject.SetActive(true);
            return true;
        }

        public virtual bool OnCloseRoute()
        {
            gameObject.SetActive(false);
            return true;
        }
    }
}
