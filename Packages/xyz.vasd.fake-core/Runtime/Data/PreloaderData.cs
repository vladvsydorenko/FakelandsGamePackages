using UnityEngine;
using Xyz.Vasd.FakeData;

namespace Xyz.Vasd.FakeCore.Data
{
    public class PreloaderData : MonoBehaviour, IFakeData
    {
        public enum PreloaderState
        {
            None,
            Loading,
            Finishing,
            Done
        }

        public PreloaderState State;
    }
}
