using UnityEngine;
using UnityEngine.Playables;

namespace Xyz.Vasd.Fake.Views
{
    [AddComponentMenu("Fake/[Fake] Animated View")]
    public class FakeAnimatedView : FakeView
    {
        public PlayableDirector Director;
        public PlayableAsset OpenAnimation;
        public PlayableAsset IdleAnimation;
        public PlayableAsset CloseAnimation;

        #region Open
        public override void OnViewOpen()
        {
            PlayAnimation(OpenAnimation);
            ViewStatus = FakeViewStatus.Opening;
        }

        public override void OnViewOpenRefresh()
        {
            if (Director.time < Director.duration) return;
            ViewStatus = FakeViewStatus.Open;

            if (Director.playableAsset != IdleAnimation) PlayAnimation(IdleAnimation);
        }
        #endregion

        #region Close
        public override void OnViewClose()
        {
            PlayAnimation(CloseAnimation);
            ViewStatus = FakeViewStatus.Closing;
        }

        public override void OnViewCloseRefresh()
        {
            if (Director.time < Director.duration) return;
            ViewStatus = FakeViewStatus.Closed;
        }
        #endregion

        private void PlayAnimation(PlayableAsset asset)
        {
            Director.playableAsset = asset;
            Director.time = 0.0;
            Director.RebuildGraph();
            Director.Play();
        }
    }
}
