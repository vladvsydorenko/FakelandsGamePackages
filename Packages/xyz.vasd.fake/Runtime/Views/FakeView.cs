using UnityEngine;

namespace Xyz.Vasd.Fake
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
            if (ViewStatus != FakeViewStatus.Opening) OnOpenView();
            return ViewStatus == FakeViewStatus.Open;
        }

        public virtual void OnOpenView()
        {
            ViewStatus = FakeViewStatus.Open;
        }

        public bool CloseView()
        {
            if (ViewStatus != FakeViewStatus.Closing) OnCloseView();
            return ViewStatus == FakeViewStatus.Open;
        }
        public virtual void OnCloseView()
        {
            ViewStatus = FakeViewStatus.Closed;
        }
    }
}
