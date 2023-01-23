using System.Collections.Generic;
using UnityEngine;

namespace Xyz.Vasd.FakeSystems
{
    public class FakeSystemGroup : MonoBehaviour
    {
        private FakeSystemManager _manager;

        private List<IFakeSystem> _systems = new List<IFakeSystem>();

        private List<IFakeSystem> _systemsToStart = new List<IFakeSystem>();
        private List<IUpdateFakeSystem> _systemsToUpdate = new List<IUpdateFakeSystem>();
        private List<ILateUpdateFakeSystem> _systemsToLateUpdate = new List<ILateUpdateFakeSystem>();
        private List<IFixedUpdateFakeSystem> _systemsToFixedUpdate = new List<IFixedUpdateFakeSystem>();
        private List<IFakeSystem> _systemsToStop = new List<IFakeSystem>();

        public virtual void InitSystemGroup(FakeSystemManager manager)
        {
            _manager = manager;
        }

        // add all systems to start queue
        public void StartAllSystems()
        {
            _systemsToStart.Clear();
            foreach (var system in _systems)
            {
                _systemsToStart.Add(system);
            }

            _systemsToStop.Clear();
            _systemsToUpdate.Clear();
            _systemsToLateUpdate.Clear();
            _systemsToFixedUpdate.Clear();
        }

        // add all systems to stop queue
        public void StopAllSystems()
        {
            _systemsToStop.Clear();
            foreach (var system in _systems)
            {
                _systemsToStop.Add(system);
            }

            _systemsToStart.Clear();
            _systemsToUpdate.Clear();
            _systemsToLateUpdate.Clear();
            _systemsToFixedUpdate.Clear();
        }

        public void AddSystem<T>(T system) where T : MonoBehaviour, IFakeSystem
        {
            if (_systems.Contains(system)) return;

            // add to all systems
            _systems.Add(system);
        }

        public void Stage_Start()
        {
            // if there is something to start - start it now

            for (int i = 0; i < _systemsToStart.Count; i++)
            {
                var system = _systemsToStart[i];
                if (system == null) continue;

                AddSystemToUpdateLoop(system);

                try
                {
                    system.OnSystemSetup(_manager);
                    system.OnSystemStart();
                }
                catch (System.Exception)
                {
                    Debug.LogError("Start error");
                    // TODO: log something
                    throw;
                }
            }

            _systemsToStart.Clear();
        }

        public void Stage_Update()
        {
            for (int i = 0; i < _systemsToUpdate.Count; i++)
            {
                var system = _systemsToUpdate[i];
                if (system == null) continue;

                try
                {
                    if (system.IsSystemActive()) system.OnSystemUpdate();
                }
                catch (System.Exception)
                {
                    // TODO: log something
                    throw;
                }
            }
        }

        public void Stage_LateUpdate()
        {
            for (int i = 0; i < _systemsToLateUpdate.Count; i++)
            {
                var system = _systemsToLateUpdate[i];
                if (system == null) continue;

                try
                {
                    if (system.IsSystemActive()) system.OnSystemLateUpdate();
                }
                catch (System.Exception)
                {
                    // TODO: log something
                    throw;
                }
            }
        }

        public void Stage_FixedUpdate()
        {
            for (int i = 0; i < _systemsToFixedUpdate.Count; i++)
            {
                var system = _systemsToFixedUpdate[i];
                if (system == null) continue;

                try
                {
                    if (system.IsSystemActive()) system.OnSystemFixedUpdate();
                }
                catch (System.Exception)
                {
                    // TODO: log something
                    throw;
                }
            }
        }

        public void Stage_Stop()
        {
            // if there is something to stop - do it now

            for (int i = 0; i < _systemsToStop.Count; i++)
            {
                var system = _systemsToStop[i];
                if (system == null) continue;

                RemoveSystemFromUpdateLoop(system);

                try
                {
                    system.OnSystemStop();
                }
                catch (System.Exception)
                {
                    Debug.LogError("Stop error");
                    // TODO: log something
                    throw;
                }
            }

            _systemsToStop.Clear();
        }
    
        private void AddSystemToUpdateLoop(IFakeSystem system)
        {
            if (system is IUpdateFakeSystem)
            {
                _systemsToUpdate.Add((IUpdateFakeSystem)system);
            }

            if (system is ILateUpdateFakeSystem)
            {
                _systemsToLateUpdate.Add((ILateUpdateFakeSystem)system);
            }

            if (system is IFixedUpdateFakeSystem)
            {
                _systemsToFixedUpdate.Add((IFixedUpdateFakeSystem)system);
            }
        }

        private void RemoveSystemFromUpdateLoop(IFakeSystem system)
        {
            if (system is IUpdateFakeSystem)
            {
                _systemsToUpdate.Remove((IUpdateFakeSystem)system);
            }

            if (system is ILateUpdateFakeSystem)
            {
                _systemsToLateUpdate.Remove((ILateUpdateFakeSystem)system);
            }

            if (system is IFixedUpdateFakeSystem)
            {
                _systemsToFixedUpdate.Remove((IFixedUpdateFakeSystem)system);
            }
        }
    }
}
