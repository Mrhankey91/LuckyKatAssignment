using System.Collections.Generic;
using UnityEngine;

public class Floor : SpawnObject
{
    public bool start = false;

    private List<SpawnObject> objects = new List<SpawnObject>();

    public override void Reset()
    {
        base.Reset();
        foreach (SpawnObject so in objects)
        {
            so.Reset();
        }
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
}
