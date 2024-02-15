using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsInstructions : MonoBehaviour
{
    private LevelController levelController;
    private Transform ballTransform;

    private void Start()
    {
        levelController = GameObject.Find("GameController").GetComponent<LevelController>();
        if (levelController.GetCurrentLevelId() > 1)
        {
            Destroy(gameObject);
        }

        ballTransform = GameObject.Find("Ball").transform;
    }

    public void CompletedInstructions()
    {
        if (ballTransform.position.y >= levelController.distanceBetweenFloors)
        {
            Destroy(gameObject);
        }
    }
}
