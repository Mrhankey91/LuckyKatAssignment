using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    private Transform parent;

    public GameObject[] spawnObjectPrefabs;
    private Dictionary<string, GameObject> prefabsDict = new Dictionary<string, GameObject>();
    private Dictionary<string, List<GameObject>> spawnedObjectsPool = new Dictionary<string, List<GameObject>>();

    public float distanceBetweenFloors = 2.5f;

    private int numberFloors = 10;
    private float currentFloorHeight = 0;

    private int level = 1;
    private Queue<Level> levels = new Queue<Level>();

    private void Awake()
    {
        parent = GameObject.Find("Helix").transform;

        GetComponent<GameController>().onRestarLevel += OnRestarLevel;
        CreateDictionary();
    }

    private void Start()
    {
        //GenerateRandomLevel();
        GenerateLevel(1);
        GenerateLevel(2);
    }

    private void CreateDictionary()
    {
        foreach(GameObject prefab in spawnObjectPrefabs)
        {
            if(prefab.TryGetComponent(out SpawnObject spawnObject))
            {
                prefabsDict.Add(spawnObject.id, prefab);
                spawnedObjectsPool.Add(spawnObject.id, new List<GameObject>());
            }
        }
    }

    private void GenerateLevel(int level)
    {
        TextAsset levelFileJson = Resources.Load<TextAsset>(string.Format("Levels/Level{0}", level));
        //Then use JsonUtility.FromJson<T>() to deserialize jsonTextFile into an object
        LevelData levelData = JsonUtility.FromJson<LevelData>(levelFileJson.text);

        GameObject levelObj;
        if (spawnedObjectsPool["level"].Count > 0) //Reuse 
        {
            levelObj = spawnedObjectsPool["level"][0];
            spawnedObjectsPool["level"].RemoveAt(0);
        }
        else//New
        {
            levelObj = Instantiate(prefabsDict["level"], parent);
        }

        numberFloors = levelData.floors.Length;
        levelObj.GetComponent<Level>().floors = new Floor[numberFloors];
        levelObj.name = string.Format("Level{0}", level);

        for(int i =0; i < numberFloors; i++)
        {
            levelObj.GetComponent<Level>().floors[i] = CreateFloor(levelObj, levelData.floors[i]);
        }

        levels.Enqueue(levelObj.GetComponent<Level>());
    }

    private Floor CreateFloor(GameObject levelObj, FloorData floorData)
    {
        Floor floor = null;

        floor = Instantiate(prefabsDict["floor"], new Vector3(0f, currentFloorHeight), Quaternion.identity, levelObj.transform).GetComponent<Floor>();
        foreach(PartData partData in floorData.parts)
        {
            AddPartToFloor(partData, floor);
        }

        floor.Init();
        currentFloorHeight += distanceBetweenFloors;
        return floor;
    }

    private void AddPartToFloor(PartData partData, Floor floor)
    {
        if(prefabsDict.ContainsKey(partData.key)) 
        {
            GameObject partObj;
            if (spawnedObjectsPool[partData.key].Count > 0) //Reuse 
            {
                partObj = spawnedObjectsPool[partData.key][0];
                spawnedObjectsPool[partData.key].RemoveAt(0);
            }
            else//New
            {
                partObj = Instantiate(prefabsDict[partData.key]);
            }

            partObj.transform.parent = floor.transform;
            partObj.transform.localPosition = Vector3.zero;
            partObj.transform.rotation = Quaternion.Euler(0f, partData.rotation, partData.mirrored ? 180f : 0f);
            SpawnObject so = partObj.GetComponent<SpawnObject>();
            so.Init();
            floor.AddObject(so);
        }
        else
        {
            Debug.Log(string.Format("Missing prefab with id: {0}", partData.key));
        }
    }

    public void DisableObject(SpawnObject spawnObject)
    {
        spawnedObjectsPool[spawnObject.id].Add(spawnObject.gameObject);
        spawnObject.gameObject.SetActive(false);
    }

    public float GetFinishLinePosition()
    {
        return numberFloors * distanceBetweenFloors;
    }

    private void OnRestarLevel()
    {
        levels.Peek().Reset();
    }
}
