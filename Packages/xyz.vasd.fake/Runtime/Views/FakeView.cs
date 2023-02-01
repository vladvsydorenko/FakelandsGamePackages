using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Xyz.Vasd.Fake
{
    public interface IFakeView
    {
        bool OpenView();
        bool CloseView();
    }

    public class FakeView : MonoBehaviour, IFakeView
    {
        bool IFakeView.CloseView()
        {
            gameObject.SetActive(false);
            return true;
        }

        bool IFakeView.OpenView()
        {
            gameObject.SetActive(true);
            return true;
        }
    }

    public class AnimatedFakeView : FakeView, IFakeView
    {
        

        bool IFakeView.CloseView()
        {
            gameObject.SetActive(false);
            return true;
        }

        bool IFakeView.OpenView()
        {
            gameObject.SetActive(true);
            return true;
        }
    }
}
