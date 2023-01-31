using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Systems
/// </summary>
namespace Xyz.Vasd.Fakelands
{
    internal class FakeSystemRunner
    {
        internal List<IFakeSystem> Systems;

        internal List<IFakeSystem> StartSystems;
        internal List<IFakeSystem> UpdateSystems;
        internal List<IFakeSystem> StopSystems;

        internal FakeSystemRunner()
        {
            Systems = new List<IFakeSystem>();
            StartSystems = new List<IFakeSystem>();
            UpdateSystems = new List<IFakeSystem>();
            StopSystems = new List<IFakeSystem>();
        }

        #region Add/Clear
        internal void AddSystem(IFakeSystem system)
        {
            Systems.Add(system);
            StartSystems.Add(system);
        }

        internal void ClearSystems()
        {
            Systems.Clear();
            StartSystems.Clear();
            UpdateSystems.Clear();
            StopSystems.Clear();
        }
        #endregion

        #region Start/Stop
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
        #endregion

        #region Loop
        internal void Stage_Start()
        {
            foreach (var system in StartSystems)
            {
                try
                {
                    system.SystemStart();
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
                    system.SystemUpdate();
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
                    system.SystemFixedUpdate();
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
                    system.SystemStop();
                }
                catch (System.Exception e)
                {
                    Debug.LogException(e);
                }
            }

            StopSystems.Clear();
        }
        #endregion
    }
}
