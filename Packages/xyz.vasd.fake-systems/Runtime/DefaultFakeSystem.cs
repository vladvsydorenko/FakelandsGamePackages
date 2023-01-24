using UnityEngine;
using Xyz.Vasd.FakeData;

namespace Xyz.Vasd.FakeSystems
{
    public class DefaultFakeSystem : MonoBehaviour, IFakeSystem
    {
        public FakeSystemManager Manager { get; private set; }
        public FakeDatabase DataBase { get; private set; }

        public virtual bool IsSystemActive()
        {
            return isActiveAndEnabled;
        }

        public virtual void OnSystemSetup(FakeSystemManager manager)
        {
            Manager = manager;
            DataBase = manager.DataBase;
        }

        public virtual void OnSystemStart()
        {

        }

        public virtual void OnSystemStop()
        {
        }
    }
}
