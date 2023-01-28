using System.Collections.Generic;
using UnityEngine;

namespace Xyz.Vasd.Fake.Systems
{
    public class FakeSystemLoop
    {
        internal List<IFakeSystem> Systems;

        internal List<IFakeSystem> StartSystems;
        internal List<IFakeSystem> UpdateSystems;
        internal List<IFakeSystem> StopSystems;

        internal FakeSystemLoop()
        {
            Systems = new List<IFakeSystem>();
            StartSystems = new List<IFakeSystem>();
            UpdateSystems = new List<IFakeSystem>();
            StopSystems = new List<IFakeSystem>();
        }

        internal void AddSystem(IFakeSystem system)
        {
            Systems.Add(system);
            StartSystems.Add(system);
        }

        internal void StartAllSystems()
        {
            StartSystems.Clear();
            foreach (var system in Systems)
            {
                StartSystems.Add(system);
            }

            StopSystems.Clear();
            UpdateSystems.Clear();
        }

        internal void StopAllSystem()
        {
            StopSystems.Clear();
            foreach (var system in Systems)
            {
                try
                {
                    StopSystems.Add(system);
                }
                catch (System.Exception)
                {
                }
            }

            StartSystems.Clear();
            UpdateSystems.Clear();
        }

        internal void ClearSystems()
        {
            Systems.Clear();
            StartSystems.Clear();
            UpdateSystems.Clear();
            StopSystems.Clear();
        }

        internal void Stage_Start()
        {
            foreach (var system in StartSystems)
            {
                try
                {
                    system.OnSystemAwake();
                    system.OnSystemStart();
                    UpdateSystems.Add(system);
                }
                catch (System.Exception e)
                {
                    Debug.LogException(e);
                }
            }

            StartSystems.Clear();
        }

        internal void Stage_Update()
        {
            foreach (var system in UpdateSystems)
            {
                try
                {
                    if (system.IsSystemEnabled) system.OnSystemUpdate();
                }
                catch (System.Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }

        internal void Stage_FixedUpdate()
        {
            foreach (var system in UpdateSystems)
            {
                try
                {
                    if (system.IsSystemEnabled) system.OnSystemFixedUpdate();
                }
                catch (System.Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }

        internal void Stage_Stop()
        {
            foreach (var system in StopSystems)
            {
                try
                {
                    system.OnSystemStop();
                }
                catch (System.Exception e)
                {
                    Debug.LogException(e);
                }
            }

            StopSystems.Clear();
        }
    }
}
