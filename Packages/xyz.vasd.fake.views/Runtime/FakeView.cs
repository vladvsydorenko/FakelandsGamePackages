using UnityEngine;

namespace Xyz.Vasd.Fake.Views
{
    [AddComponentMenu("Fake/[Deprecated]/[Fake] View")]
    public class FakeView : MonoBehaviour, IFakeView
    {
        [SerializeField]
        protected FakeViewStatus ViewStatus;
    
        public FakeViewStatus GetViewStatus()
        {
            return ViewStatus;
        }

        public GameObject GetViewGameObject()
        {
            return gameObject;
        }

        #region Open
        public bool OpenView()
        {
            // already open
            if (ViewStatus == FakeViewStatus.Open) return true;

            // first open
            if (ViewStatus != FakeViewStatus.Opening)
            {
                ViewStatus = FakeViewStatus.Opening;
                OnViewOpen();
            }

            // view was open immediately, no need to call refresh
            if (ViewStatus == FakeViewStatus.Open) return true;

            // refresh
            if (ViewStatus == FakeViewStatus.Opening)
            {
                OnViewOpenRefresh();
            }

            return ViewStatus == FakeViewStatus.Open;
        }

        public virtual void OnViewOpen()
        {
            gameObject.SetActive(true);
        }

        public virtual void OnViewOpenRefresh()
        {
            ViewStatus = FakeViewStatus.Open;
        }
        #endregion

        #region Close
        public bool CloseView()
        {
            // already closed
            if (ViewStatus == FakeViewStatus.Closed) return true;

            if (ViewStatus != FakeViewStatus.Closing)
            {
                // mark status
                ViewStatus = FakeViewStatus.Closing;
                OnViewClose();
            }

            // view was closed immediately, no need to call refresh
            if (ViewStatus == FakeViewStatus.Closed) return true;

            if (ViewStatus == FakeViewStatus.Closing)
            {
                OnViewCloseRefresh();
            }

            return ViewStatus == FakeViewStatus.Closed;
        }

        public virtual void OnViewClose()
        {
        }

        public virtual void OnViewCloseRefresh()
        {
            gameObject.SetActive(false);
            ViewStatus = FakeViewStatus.Closed;
        }
        #endregion
    }
}
