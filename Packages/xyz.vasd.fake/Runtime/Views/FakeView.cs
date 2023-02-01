using UnityEngine;
using UnityEngine.Playables;

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
            if (ViewStatus != FakeViewStatus.Opening) OnOpenView();
            else OnOpeningView();

            return ViewStatus == FakeViewStatus.Open;
        }

        public virtual void OnOpenView()
        {
            gameObject.SetActive(true);
            ViewStatus = FakeViewStatus.Open;
        }

        public virtual void OnOpeningView()
        {
        }

        public bool CloseView()
        {
            if (ViewStatus != FakeViewStatus.Closing) OnCloseView();
            else OnClosingView();

            return ViewStatus == FakeViewStatus.Open;
        }

        public virtual void OnCloseView()
        {
            gameObject.SetActive(false);
            ViewStatus = FakeViewStatus.Closed;
        }
        public virtual void OnClosingView()
        {
        }
    }

    public class FakeAnimatedView : FakeView
    {
        public PlayableDirector Director;
        public PlayableAsset OpenAnimation;
        public PlayableAsset IdleAnimation;
        public PlayableAsset CloseAnimation;

        public override void OnOpenView()
        {
            PlayAnimation(OpenAnimation);
            ViewStatus = FakeViewStatus.Opening;
        }
        public override void OnOpeningView()
        {
            if (Director.time < Director.duration) return;
            ViewStatus = FakeViewStatus.Open;
        }

        public override void OnCloseView()
        {
            PlayAnimation(CloseAnimation);
            ViewStatus = FakeViewStatus.Closing;
        }
        public override void OnClosingView()
        {
            if (Director.time < Director.duration) return;
            ViewStatus = FakeViewStatus.Closed;
        }

        private void PlayAnimation(PlayableAsset asset)
        {
            Director.playableAsset = asset;
            Director.time = 0.0;
            Director.RebuildGraph();
            Director.Play();
        }
    }
}
