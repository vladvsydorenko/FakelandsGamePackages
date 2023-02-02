using UnityEngine;
using Xyz.Vasd.Fake.Views;

namespace Xyz.Vasd.Fakelands
{
    [AddComponentMenu("Fakelands/[Fakelands] Preloader")]
    public class Preloader : MonoBehaviour
    {
        public FakeRouter Router;
        private IFakeView LoaderView;

        private void Awake()
        {
            Refresh();
        }

        private void OnValidate()
        {
            Refresh();
        }

        private void Update()
        {
            if (LoaderView.GetViewStatus() == FakeViewStatus.Open) Router.Location = "/loader";
        }

        private void Refresh()
        {
            LoaderView = GetComponent<IFakeView>();
            if (Router == null) Router = GetComponentInParent<FakeRouter>();
        }
    }
}
