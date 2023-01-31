using UnityEngine;
using Xyz.Vasd.Fake.Database;
using Xyz.Vasd.Fake.Systems;

namespace Xyz.Vasd.FakeGame
{
    public class GameWindowManager : SystemBehaviour
    {
        [SystemContext]
        private FakeDatabase _db;

        public override void OnSystemStart()
        {
            _db.SetSingleton(1);

            if (_db != null)
            {
                Debug.Log("It Works");
            }
            else
            {
                Debug.Log("It DOESN'T Works");
            }
        }

        public override void OnSystemUpdate()
        {
            if (_db.GetSingleton<int>() != 1)
            {
                Debug.Log("FUCK FUCK FUCK!!!");
            }
        }
    }

}
