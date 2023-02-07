using UnityEngine;
using Xyz.Vasd.Fake.Task;

namespace Xyz.Vasd.Fake.Router
{
    public class PreloaderRoute : FakeRoute
    {
        public Animator Animator;

        private IFakeTask _openTask;
        private IFakeTask _loadingTask;

        protected override void Awake()
        {
            base.Awake();

            _openTask = FakeTask
                .Function((v) => Animator.Play(0))
                .Then(v => Animator.GetBool("open"));

            _loadingTask = FakeTask
                .Function((v) => true);
        }

        public override bool OnOpenRoute(int version)
        {
            gameObject.SetActive(true);
            return _openTask.Execute(version) && _loadingTask.Execute(version);
        }

        public override bool OnCloseRoute(int version)
        {
            gameObject.SetActive(false);
            return true;
        }
    }
}
