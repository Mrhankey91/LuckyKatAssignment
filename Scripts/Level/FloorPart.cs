using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorPart : SpawnObject, IBouncable
{
    public bool Bounce()
    {
        return true;
    }
}
