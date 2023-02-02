using UnityEngine;
using Xyz.Vasd.Fake.Views;

namespace Xyz.Vasd.Fakelands
{
    [AddComponentMenu("Fakelands/[Fakelands] Preloader Page")]
    public class LoaderPage : FakeAnimatedView
    {
        [Header("Refs")]
        public FakeRouter Router;
        public AddressableLoader Loader;

        [Header("Settings")]
        [Tooltip("Redirect router after loading. empty = disabled")]
        public string Redirect;

        protected override void Awake()
        {
            base.Awake();
            Refresh();
        }

        protected virtual void Start()
        {
            if (Loader != null) Loader.LoadAddressables();
        }

        protected virtual void Update()
        {
            if (Loader != null && !Loader.LoadAddressables()) return;

            if (Router != null &&
                ViewStatus == FakeViewStatus.Open &&
                Redirect.Length > 0)
            {
                Router.Location = Redirect;
            }
        }

        protected override void OnValidate()
        {
            Refresh();
        }

        private void Refresh()
        {
            if (Router == null) Router = GetComponentInParent<FakeRouter>();
            if (Loader == null) Loader = GetComponentInChildren<AddressableLoader>();
        }
    }
}
