using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    private GameController gameController;
    private Transform parent;

    public GameObject[] spawnObjectPrefabs;
    private Dictionary<string, GameObject> prefabsDict = new Dictionary<string, GameObject>();
    private Dictionary<string, List<GameObject>> spawnedObjectsPool = new Dictionary<string, List<GameObject>>();

    public Transform[] centerColumns = new Transform[3];
    public float distanceBetweenFloors = 2.5f;

    private int numberFloors = 10;
    private float currentFloorY = 0; //increase with distanceBetweenFloors everytime a new floor has been created
    private int currentCenterColumn = 0;

    private int currentLevel = 1;
    private Queue<Level> levels = new Queue<Level>();

    private void Awake()
    {
        gameController = GetComponent<GameController>();
        parent = GameObject.Find("Helix").transform;

        GetComponent<GameController>().onRestarLevel += OnRestarLevel;
        CreateDictionary();
    }

    private void Start()
    {
        //GenerateRandomLevel();
        GenerateLevel(currentLevel, true);
        GenerateLevel(currentLevel+1);
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

    private void GenerateLevel(int levelId, bool start = false)
    {
        TextAsset levelFileJson = Resources.Load<TextAsset>(string.Format("Levels/Level{0}", levelId));
        if (levelFileJson == null)
        {
            Debug.Log(string.Format("Level file(JSON) with id: {0} doesn't exits!", levelId));
            return;
        }

        //Then use JsonUtility.FromJson<T>() to deserialize jsonTextFile into an object
        LevelData levelData = JsonUtility.FromJson<LevelData>(levelFileJson.text);

        GameObject levelObj;
        if (spawnedObjectsPool["level"].Count > 0) //Reuse 
        {
            levelObj = spawnedObjectsPool["level"][0];
            spawnedObjectsPool["level"].RemoveAt(0);
            levelObj.SetActive(true);
        }
        else//New
        {
            levelObj = Instantiate(prefabsDict["level"], parent);
        }

        numberFloors = levelData.floors.Length;
        SetCenterColumn(numberFloors);

        Level level = levelObj.GetComponent<Level>();
        level.SetLevel(levelData);
        level.floors = new Floor[numberFloors];
        levelObj.name = string.Format("Level{0}", level.level);

        for(int i =0; i < numberFloors; i++)
        {
            levelObj.GetComponent<Level>().floors[i] = CreateFloor(levelObj, levelData.floors[i], !start && i == 0);
        }

        levels.Enqueue(levelObj.GetComponent<Level>());
    }

    private void SetCenterColumn(int floorsCount)
    {
        centerColumns[currentCenterColumn].position = new Vector3(0f, currentFloorY + (floorsCount * distanceBetweenFloors /2f), 0f);
        centerColumns[currentCenterColumn].localScale = new Vector3(1f, floorsCount * distanceBetweenFloors/2f, 1f);

        currentCenterColumn++;
        if (currentCenterColumn == centerColumns.Length)
            currentCenterColumn = 0;
    }

    private Floor CreateFloor(GameObject levelObj, FloorData floorData, bool startFloor)
    {
        Floor floor = null;

        if (spawnedObjectsPool["floor"].Count > 0) //Reuse 
        {
            GameObject floorObj = spawnedObjectsPool["floor"][0];
            spawnedObjectsPool["floor"].RemoveAt(0);
            floorObj.SetActive(true);
            floor = floorObj.GetComponent<Floor>();
        }
        else
        {
            floor = Instantiate(prefabsDict["floor"], levelObj.transform).GetComponent<Floor>();
        }

        floor.transform.position = new Vector3(0f, currentFloorY, 0f);
        floor.Init();
        floor.SetAsFinishStartNewLevel(startFloor);

        foreach (PartData partData in floorData.parts)
        {
            AddPartToFloor(partData, floor);
        }

        currentFloorY += distanceBetweenFloors;
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
                partObj.SetActive(true);
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

            if (floor.IsStartFloor())
            {
                if(partObj.TryGetComponent(out FinishFloorPart floorPart))
                {
                    floorPart.Reached(false);
                }
            }

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

    public void FinishedLevel()
    {
        Level finishedLevel = levels.Dequeue();
        finishedLevel.DisableObject();
        currentLevel = finishedLevel.level + 1;
        GenerateLevel(currentLevel + 1);
        gameController.CompletedLevel(currentLevel-1);
    }

    public float GetFloorYPosition(int floorId)
    {
        return levels.Peek().floors[floorId].transform.position.y;
    }

    public float GetStartPosition()
    {
        if(levels.Count == 0)
            return 0f;  
        else
            return levels.Peek().floors[0].transform.position.y;
    }

    public float GetFinishPosition()
    {
        if (levels.Count == 0)
            return 0f;
        else
            return levels.Peek().floors[levels.Peek().floors.Length-1].transform.position.y;
        //return numberFloors * distanceBetweenFloors;
    }

    public Level GetCurrentLevel()
    {
        if(levels.Count == 0) return null;

        return levels.Peek();
    }

    public int GetCurrentLevelId()
    {
        return currentLevel;
    }

    public int GetNextLevelId()
    {
        return currentLevel + 1;
    }

    private void OnRestarLevel()
    {
        levels.Peek().Reset();
    }
}
