using UnityEngine;
using Xyz.Vasd.Fake.Systems;

namespace Xyz.Vasd.Fake
{
    public class DebugSystem : SystemBehaviour<Context>
    {
        public int Id;

        public override void OnSystemStart()
        {
            Debug.Log($"START: {Id}");
        }

        public override void OnSystemUpdate()
        {
            Debug.Log($"Update: {Id}");
        }

        public override void OnSystemFixedUpdate()
        {
            Debug.Log($"Fixed Update: {Id}");
        }

        public override void OnSystemStop()
        {
            Debug.Log($"STOP: {Id}");
        }
    }

}
