using UnityEngine;

namespace Xyz.Vasd.Fake.Views
{
    public class FakeView : MonoBehaviour, IFakeView
    {
        protected FakeViewStatus ViewStatus;
    
        public FakeViewStatus GetViewStatus()
        {
            return ViewStatus;
        }

        public bool OpenView()
        {
            if (ViewStatus != FakeViewStatus.Opening) OnViewOpen();
            else OnViewOpenRefresh();

            return ViewStatus == FakeViewStatus.Open;
        }

        public virtual void OnViewOpen()
        {
            gameObject.SetActive(true);
            ViewStatus = FakeViewStatus.Open;
        }

        public virtual void OnViewOpenRefresh()
        {
        }

        public bool CloseView()
        {
            if (ViewStatus != FakeViewStatus.Closing) OnViewClose();
            else OnViewCloseRefresh();

            return ViewStatus == FakeViewStatus.Open;
        }

        public virtual void OnViewClose()
        {
            gameObject.SetActive(false);
            ViewStatus = FakeViewStatus.Closed;
        }
        public virtual void OnViewCloseRefresh()
        {
        }
    }
}
