using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProgress : MonoBehaviour
{
    private LevelController levelController;

    private float progress;

    void Awake()
    {
        levelController = GameObject.Find("GameController").GetComponent<LevelController>();
    }

    public float GetProgress()
    {
        float startPosition = levelController.GetStartPosition();
        return (transform.position.y - startPosition) / (levelController.GetFinishPosition() - startPosition);
    }
}
