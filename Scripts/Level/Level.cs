using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : SpawnObject
{
    public int level = 0;
    public Color[] background = new Color[0];
    public Floor[] floors = new Floor[0];

    public void SetLevel(LevelData levelData)
    {
        level = levelData.level;
        background = new Color[levelData.background.Length];
        for(int i = 0; i < background.Length; i++)
        {
            if (ColorUtility.TryParseHtmlString(levelData.background[i], out Color myColor))
                background[i] = myColor;
            else
                background[i] = new Color(0.9740638f, 0.6556604f, 1f);
        }
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
