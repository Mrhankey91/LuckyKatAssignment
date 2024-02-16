using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataContainer : MonoBehaviour
{
    public static DataContainer instance;

    public int levelId = 1; //store the levelId so it can be loaded when game starts

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetLevelId(int levelId)
    {
        this.levelId = levelId;
    }
}
