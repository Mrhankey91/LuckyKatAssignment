using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : SpawnObject
{
    public int level = 0;
    public Floor[] floors = new Floor[0];

    public void CompletedLevel()
    {
    }

    public override void Reset()
    {
        base.Reset();
        foreach (Floor floor in floors)
        {
            floor.Reset();
        }
    }

    public override void DisableObject()
    {
        foreach (Floor floor in floors)
        {
            floor.DisableObject();
        }
        base.DisableObject();
    }
}
