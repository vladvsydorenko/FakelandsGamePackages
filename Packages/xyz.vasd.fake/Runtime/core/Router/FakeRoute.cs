using UnityEngine;

namespace Xyz.Vasd.Fake.Router
{
    public interface IFakeRoute
    {
        bool MatchRoute(string path);
        bool OpenRoute(int version);
        bool CloseRoute(int version);
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

        public bool OpenRoute(int version = 0)
        {
            return OnOpenRoute(version);
        }

        public bool CloseRoute(int version = 0)
        {
            return OnCloseRoute(version);
        }

        public virtual bool OnOpenRoute(int version)
        {
            gameObject.SetActive(true);
            return true;
        }

        public virtual bool OnCloseRoute(int version)
        {
            gameObject.SetActive(false);
            return true;
        }
    }
}
