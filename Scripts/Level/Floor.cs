using System.Collections.Generic;
using UnityEngine;

public class Floor : SpawnObject
{
    private bool start = false;
    private List<SpawnObject> objects = new List<SpawnObject>();

    public override void Reset()
    {
        base.Reset();
        foreach (SpawnObject so in objects)
        {
            so.Reset();
        }
    }

    public void SetAsFinishStartNewLevel(bool b)
    {
        start = b;
    }

    public override void DisableObject()
    {
        foreach (SpawnObject so in objects)
        {
            so.DisableObject();
        }

        objects.Clear();
        base.DisableObject();
    }

    public void AddObject(SpawnObject obj)
    {
        objects.Add(obj);
    }

    public bool IsStartFloor()
    {
        return start;
    }
}
