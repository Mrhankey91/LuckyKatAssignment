using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyColor : MonoBehaviour
{
    private PlayerProgress playerProgress;
    private LevelController levelController;

    void Awake()
    {
        playerProgress = GameObject.Find("Ball").GetComponent<PlayerProgress>();
        levelController = GameObject.Find("GameController").GetComponent<LevelController>();
    }

    void Update()
    {
        Level level = levelController.GetCurrentLevel();
        if (level != null)
            Camera.main.backgroundColor = Color.Lerp(level.background[0], level.background[1], playerProgress.GetProgress());
    }
}
