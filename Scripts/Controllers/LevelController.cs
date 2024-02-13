using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    private Transform parent;

    public float distanceBetweenFloors = 2.5f;
    public GameObject floorPrefab;

    private int numberFloors = 20;
    private Floor[] floors = new Floor[0];

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
        floors = new Floor[numberFloors];

        for(int i = 0; i < floors.Length; i++)
        {
            GameObject obj = Instantiate(floorPrefab, new Vector3(0f, i * distanceBetweenFloors, 0f), Quaternion.identity, parent);
            floors[i] = obj.GetComponent<Floor>();

            if (i == floors.Length - 1)
            {
                //floors[i].Finish();
            }
            else
            {
                //floors[i].Random();
            }
        }
    }

    public float GetFinishLinePosition()
    {
        return numberFloors * distanceBetweenFloors;
    }

    private void OnRestarLevel()
    {
        foreach(Floor floor in floors)
        {
            floor.Reset();
        }
    }
}
