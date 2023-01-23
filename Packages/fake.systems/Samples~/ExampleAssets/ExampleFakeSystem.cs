using UnityEngine;
using Xyz.Vasd.FakeSystems;

public class ExampleFakeSystem : DefaultFakeSystem, IUpdateFakeSystem, ILateUpdateFakeSystem, IFixedUpdateFakeSystem
{
    public override void OnSystemStart()
    {
        Debug.Log("Start");
    }

    public override void OnSystemStop()
    {
        Debug.Log("Stop");
    }

    public void OnSystemUpdate()
    {
        Debug.Log("Update");
    }

    public void OnSystemLateUpdate()
    {
        Debug.Log("Late update");
    }

    public void OnSystemFixedUpdate()
    {
        Debug.Log("Fixed update");
    }
}
