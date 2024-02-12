using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    private Transform parent;

    public float distanceBetweenPlatforms = 3f;
    public GameObject platformPrefab;

    private int numberPlatforms = 20;
    private Platform[] platforms = new Platform[0];

    private void Awake()
    {
        parent = GameObject.Find("Helix").transform;

        GetComponent<GameController>().onRestarLevel += OnRestarLevel;
    }

    private void Start()
    {
        GenerateRandomLevel();
    }

    private void GenerateRandomLevel()
    {
        platforms = new Platform[numberPlatforms];

        for(int i = 0; i < platforms.Length; i++)
        {
            GameObject obj = Instantiate(platformPrefab, new Vector3(0f, i * -distanceBetweenPlatforms, 0f), Quaternion.identity, parent);
            platforms[i] = obj.GetComponent<Platform>();

            if (i == platforms.Length - 1)
            {
                platforms[i].Finish();
            }
            else
            {
                platforms[i].Random();
            }
        }
    }

    public void PassedPlatform(int platform)
    {
        if (platform - 1 < platforms.Length -1)
        {
            platforms[platform - 1].PassedByBall();
        }
    }

    public void BreakPlatform(int platform)
    {
        platforms[platform - 1].BreakByBall();
    }

    public float GetFinishLinePosition()
    {
        return numberPlatforms * -distanceBetweenPlatforms;
    }

    private void OnRestarLevel()
    {
        foreach(Platform platform in platforms)
        {
            platform.Reset();
        }
    }
}
