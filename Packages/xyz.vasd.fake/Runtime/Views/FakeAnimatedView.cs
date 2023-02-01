using UnityEngine;

namespace Xyz.Vasd.Fake.Views
{

    [AddComponentMenu("Fake/[Fake] Animated View")]
    public class FakeAnimatedView : FakeView
    {
        public Animator Animator;

        public string CloseTrigger = "trigger:close";
        public string OpenCallback = "callback:open";
        public string ClosedCallback = "callback:closed";

        protected virtual void Awake()
        {
            if (Animator == null) Animator = GetComponent<Animator>();
            if (Animator != null) Animator.StopPlayback();
        }

        public override void OnViewOpen()
        {
            gameObject.SetActive(true);
            Animator.Play(0);
        }

        public override void OnViewOpenRefresh()
        {
            if (!Animator.GetBool(OpenCallback)) return;

            ViewStatus = FakeViewStatus.Open;
        }

        public override void OnViewClose()
        {
            base.OnViewClose();
            Animator.SetBool(CloseTrigger, true);
        }

        public override void OnViewCloseRefresh()
        {
            if (!Animator.GetBool(ClosedCallback)) return;

            ViewStatus = FakeViewStatus.Closed;
            gameObject.SetActive(false);
        }
    }
}
