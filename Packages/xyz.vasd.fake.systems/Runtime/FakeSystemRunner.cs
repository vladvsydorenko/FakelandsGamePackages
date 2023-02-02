using System.Collections.Generic;
using UnityEngine;

namespace Xyz.Vasd.Fake.Systems
{
    public class FakeSystemRunner
    {
        internal List<IFakeSystem> Systems;

        internal List<IFakeSystem> StartSystems;
        internal List<IFakeSystem> UpdateSystems;
        internal List<IFakeSystem> StopSystems;

        private List<IFakeSystem> _temp;
        private List<IFakeSystem> _startedSystems;

        public FakeSystemRunner()
        {
            Systems = new List<IFakeSystem>();
            StartSystems = new List<IFakeSystem>();
            UpdateSystems = new List<IFakeSystem>();
            StopSystems = new List<IFakeSystem>();
        }

        #region Add/Clear
        public void AddSystem(IFakeSystem system)
        {
            Systems.Add(system);
            StartSystems.Add(system);
        }

        public void ClearSystems()
        {
            Systems.Clear();
            StartSystems.Clear();
            UpdateSystems.Clear();
            StopSystems.Clear();
        }
        #endregion

        #region Start/Stop
        public void StartAllSystems()
        {
            StartSystems.Clear();
            foreach (var system in Systems)
            {
                StartSystems.Add(system);
            }

            StopSystems.Clear();
            UpdateSystems.Clear();
        }

        public void StopAllSystem()
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
        public void Stage_Start()
        {
            _temp.Clear();

            foreach (var system in StartSystems)
            {
                try
                {
                    if (system.IsSystemActive())
                    {
                        system.SystemStart();
                        UpdateSystems.Add(system);
                        _temp.Add(system);
                        _startedSystems.Add(system);
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogException(e);
                }
            }

            RemoveFromList(_temp, StartSystems);
        }

        public void Stage_Update()
        {
            foreach (var system in UpdateSystems)
            {
                try
                {
                    if (system.IsSystemActive()) system.SystemUpdate();
                }
                catch (System.Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }

        public void Stage_FixedUpdate()
        {
            foreach (var system in UpdateSystems)
            {
                try
                {
                    if (system.IsSystemActive()) system.SystemFixedUpdate();
                }
                catch (System.Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }

        public void Stage_Stop()
        {
            foreach (var system in StopSystems)
            {
                try
                {
                    if (_startedSystems.Contains(system)) system.SystemStop();
                }
                catch (System.Exception e)
                {
                    Debug.LogException(e);
                }
            }

            StopSystems.Clear();
        }
        #endregion

        #region
        private void RemoveFromList(List<IFakeSystem> valuesToRemove, List<IFakeSystem> list)
        {
            foreach (var system in valuesToRemove)
            {
                list.Remove(system);
            }
        }
        #endregion
    }
}
