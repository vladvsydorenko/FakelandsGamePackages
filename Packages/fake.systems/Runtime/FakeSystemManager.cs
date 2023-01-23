using UnityEngine;
using Xyz.Vasd.FakeData;

namespace Xyz.Vasd.FakeSystems
{
    public class FakeSystemManager : MonoBehaviour
    {
        public FakeDataBase DataBase { get; protected set; }
        public FakeSystemGroup[] SystemGroups { get; protected set; }

        private void OnEnable()
        {
            foreach (var group in SystemGroups)
            {
                group.InitSystemGroup(this);
                group.StartAllSystems();
                group.Stage_Start();
            }
        }

        private void Update()
        {
            foreach (var group in SystemGroups)
            {
                group.Stage_Update();
            }
        }

        private void LateUpdate()
        {
            foreach (var group in SystemGroups)
            {
                group.Stage_LateUpdate();
            }
        }

        private void FixedUpdate()
        {
            foreach (var group in SystemGroups)
            {
                group.Stage_Start();
                group.Stage_FixedUpdate();
                group.Stage_Stop();
            }
        }

        private void OnDisable()
        {
            foreach (var group in SystemGroups)
            {
                group.StopAllSystems();
                group.Stage_Stop();
            }
        }
    }
}
