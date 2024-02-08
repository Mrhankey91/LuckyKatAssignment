using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public PlatformPart[] platformParts = new PlatformPart[0];

    void Awake()
    {
    }

    public void Reset()
    {
        foreach (PlatformPart part in platformParts)
        {
            part.Reset();
        }
    }

    public void Random()
    {
        int i = UnityEngine.Random.Range(0, platformParts.Length);

        if(i < platformParts.Length - 1)
        {
            platformParts[i].Disable();
            platformParts[i+1].Disable();
        }
        else
        {
            platformParts[i].Disable();
            platformParts[0].Disable();
        }
    }

    public void PassedByBall()
    {
        foreach (PlatformPart part in platformParts)
        {
            part.PlatformPassedByBall();
        }
    }

    public void BreakByBall()
    {
        foreach (PlatformPart part in platformParts)
        {
            part.PlatformPassedByBall();
        }
    }
}
