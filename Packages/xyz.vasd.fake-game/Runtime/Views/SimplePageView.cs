using UnityEngine;
using UnityEngine.Playables;
using Xyz.Vasd.Fake.Systems;
using Xyz.Vasd.FakeGame.Core;

namespace Xyz.Vasd.FakeGame.Views
{
    public class SimplePageView : SystemBehaviour, IView
    {
        public PlayableDirector Director;

        private IView.Status _status;

        public IView.Status GetViewStatus()
        {
            return _status;
        }

        public void CloseView()
        {
            gameObject.SetActive(false);
            _status = IView.Status.Closed;
        }

        public void OpenView()
        {
            if (_status == IView.Status.Opening) return;

            gameObject.SetActive(true);
            Director.Play();
            _status = IView.Status.Opening;
        }

        public override void OnSystemUpdate()
        {
            if (_status == IView.Status.Opening)
            {
                if (Director.time >= Director.duration)
                {
                    _status = IView.Status.Open;
                }
            }
        }
    }
}
