using UnityEngine;
using Xyz.Vasd.FakeGame.Core;

namespace Xyz.Vasd.FakeGame.Pages
{
    public class BaseView : MonoBehaviour, IView
    {
        protected IView.Status ViewStatus;

        public IView.Status GetViewStatus()
        {
            return ViewStatus;
        }

        public virtual void OpenView()
        {
            gameObject.SetActive(true);
            ViewStatus = IView.Status.Open;
        }

        public virtual void CloseView()
        {
            gameObject.SetActive(false);
            ViewStatus = IView.Status.Closed;
        }
    }
}
