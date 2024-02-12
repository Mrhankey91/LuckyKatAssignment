using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public Mesh platformPartMesh;
    public List<PlatformPart> platformParts = new List<PlatformPart>();

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
            platformParts[i].SetPartAsTrigger();
            platformParts[i+1].SetPartAsTrigger();
        }
        else
        {
            platformParts[i].SetPartAsTrigger();
            platformParts[0].SetPartAsTrigger();
        }

        int b = UnityEngine.Random.Range(0, platformParts.Length);
        if (b == i || b == i + 1)
        {
            b -= 1;
            if (b < 0)
            {
                b = platformParts.Length - 1;
            }
        }

        platformParts[b].SetPartAsBad();
    }

    public void Finish()
    {
        foreach (PlatformPart part in platformParts)
        {
            part.SetPartAsFinish();
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
