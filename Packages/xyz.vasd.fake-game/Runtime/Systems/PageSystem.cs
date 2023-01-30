using UnityEngine;
using Xyz.Vasd.Fake.Systems;
using Xyz.Vasd.FakeGame.Core;
using Xyz.Vasd.FakeGame.Pages;

namespace Xyz.Vasd.FakeGame.Systems
{
    public class PageSystem : SystemBehaviour
    {
        public SwitchView Switch;

        public AddressableLoaderView Preloader;
        public AddressableLoaderView Loader;
        public AnimatedPage Game;

        public override void OnSystemStart()
        {
            Switch.OpenView(Preloader);
        }

        public override void OnSystemUpdate()
        {
            if (Preloader.GetViewStatus() == IView.Status.Open)
            {
                if (Preloader.IsLoaded())
                {
                    Switch.OpenView(Loader);
                }
                return;
            }

            if (Loader.GetViewStatus() == IView.Status.Open)
            {
                if (Loader.IsLoaded())
                {
                    Switch.OpenView(Game);
                }
            }
        }
    }
}
