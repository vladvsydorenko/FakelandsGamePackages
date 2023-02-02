using UnityEngine;

namespace Xyz.Vasd.Fake.Views
{
    public interface IFakeView
    {
        FakeViewStatus GetViewStatus();
        GameObject GetViewGameObject();
        bool OpenView();
        bool CloseView();
    }
}
