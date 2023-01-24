using UnityEngine;
using UnityEngine.Rendering;
using Xyz.Vasd.FakeData;

namespace Xyz.Vasd.FakeCore
{
    public class EnvironmentData : MonoBehaviour, IFakeData
    {
        public Light Sun;
        public Camera Camera;
        public Volume Volume;
    }
}
