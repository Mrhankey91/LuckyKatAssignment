using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Ball ball;
    private LevelController levelController;

    private Vector3 cameraPosition;
    private float yOffset;
    private float targetPositionY;
    private Coroutine moveCoroutine;

    private bool isFalling = false;

    private void Awake()
    {
        ball = GameObject.Find("Ball").GetComponent<Ball>();
        levelController = GameObject.Find("GameController").GetComponent<LevelController>();
        cameraPosition = transform.position;
        yOffset = cameraPosition.y;

        GameObject.Find("GameController").GetComponent<GameController>().onRestarLevel += OnRestarLevel;
        ball.onCurrentFloorChange += OnCurrentFloorChange;
    }

    void Update()
    {
        if (isFalling)
        {
            cameraPosition.y = ball.transform.position.y + yOffset;
            transform.position = cameraPosition;
        }
    }

    private IEnumerator MoveCamera()
    {
        float time = 0f;
        float moveDuration = 1f;
        while (time < moveDuration)
        {
            yield return null;

            time += Time.deltaTime;
            cameraPosition.y = Mathf.Lerp(cameraPosition.y, targetPositionY, time/moveDuration);
            //cameraPosition.y = Mathf.Min(lowestYPosition, target.position.y + yOffset);
            //cameraPosition.y = ball.GetLowestYPosition() + yOffset;
            transform.position = cameraPosition;
        }

        moveCoroutine = null;
    }

    private void OnRestarLevel()
    {
        cameraPosition.y = yOffset;
        transform.position = cameraPosition;
    }

    private void OnCurrentFloorChange(int floor, bool isFalling)
    {
        this.isFalling = isFalling;
        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);

        if (isFalling)
        {

        }
        else
        {
            targetPositionY = floor * levelController.distanceBetweenFloors + yOffset;
            moveCoroutine = StartCoroutine(MoveCamera());
        }
    }
}
