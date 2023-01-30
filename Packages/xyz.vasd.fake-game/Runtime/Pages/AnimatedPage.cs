using UnityEngine;
using UnityEngine.Playables;
using Xyz.Vasd.FakeGame.Core;

namespace Xyz.Vasd.FakeGame.Pages
{

    public class AnimatedPage : BaseView
    {
        public PlayableDirector Director;
        public PlayableAsset InAnimation;
        public PlayableAsset OutAnimation;

        public override void OpenView()
        {
            if (ViewStatus == IView.Status.Open || ViewStatus == IView.Status.Opening) return;

            ViewStatus = IView.Status.Opening;
            gameObject.SetActive(true);
            if (Director != null)
            {
                Director.playableAsset = InAnimation;
                Director.RebuildGraph();
                Director.Play();
            }
        }

        public override void CloseView()
        {
            if (ViewStatus == IView.Status.Closed || ViewStatus == IView.Status.Closing) return;

            ViewStatus = IView.Status.Closing;
            if (Director != null)
            {
                Director.playableAsset = OutAnimation;
                Director.time = 0.0;
                Director.RebuildGraph();
                Director.Play();
            }
        }

        private void Update()
        {
            if (Director == null)
            {
                if (ViewStatus == IView.Status.Opening) ViewStatus = IView.Status.Open;
                if (ViewStatus == IView.Status.Closing)
                {
                    ViewStatus = IView.Status.Closed;
                    gameObject.SetActive(false);
                }
                return;
            };

            if (ViewStatus == IView.Status.Opening && Director.time >= Director.duration)
            {
                ViewStatus = IView.Status.Open;
            }

            if (ViewStatus == IView.Status.Closing && Director.time >= Director.duration)
            {
                ViewStatus = IView.Status.Closed;
                gameObject.SetActive(false);
            }
        }
    }
}
