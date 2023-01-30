using UnityEngine;
using Xyz.Vasd.Fake.Systems;
using Xyz.Vasd.FakeGame.Views;

namespace Xyz.Vasd.FakeGame.Core
{
    public class SwitchView : SystemBehaviour
    {
        public SimplePageView TestView1;
        public SimplePageView TestView2;

        [ContextMenu("ShowTestView1")]
        public void ShowTestView1()
        {
            ShowView(TestView1);
        }

        [ContextMenu("ShowTestView2")]
        public void ShowTestView2()
        {
            ShowView(TestView2);
        }

        public IView View { get; private set; }
        public IView LastView { get; private set; }

        public void ShowView(IView view)
        {
            LastView = View;
            View = view;
        }

        public override void OnSystemUpdate()
        {
            var isOpen = IsViewOpen(View);
            var isOpening = IsViewOpening(View);

            if (!isOpen)
            {
                if (!isOpening && View != null) View.OpenView();
                return;
            }

            var isLastClosed = IsViewClosed(LastView);
            var isLastClosing = IsViewClosing(LastView);

            if (!isLastClosed && !isLastClosing && LastView != null) LastView.CloseView();
        }

        private bool IsViewOpen(IView view)
        {
            return view != null && view.GetViewStatus() == IView.Status.Open;
        }

        private bool IsViewOpening(IView view)
        {
            return view != null && view.GetViewStatus() == IView.Status.Opening;
        }

        private bool IsViewClosed(IView view)
        {
            return view != null && view.GetViewStatus() == IView.Status.Closed;
        }

        private bool IsViewClosing(IView view)
        {
            return view != null && view.GetViewStatus() == IView.Status.Closing;
        }
    }
}
