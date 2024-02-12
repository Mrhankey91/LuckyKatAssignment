using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform target;
    //private Ball ball;

    private Vector3 cameraPosition;
    private float yOffset;
    private float lowestYPosition;

    private void Awake()
    {
        target = GameObject.Find("Ball").transform;//.GetComponent<Ball>();
        cameraPosition = transform.position;
        yOffset = cameraPosition.y;
        lowestYPosition = yOffset;

        GameObject.Find("GameController").GetComponent<GameController>().onRestarLevel += OnRestarLevel;
    }

    void Update()
    {
        cameraPosition.y = Mathf.Min(lowestYPosition, target.position.y + yOffset);
        //cameraPosition.y = ball.GetLowestYPosition() + yOffset;
        transform.position = cameraPosition;
        lowestYPosition = cameraPosition.y;
    }

    private void OnRestarLevel()
    {
        cameraPosition.y = yOffset;
        transform.position = cameraPosition;
        lowestYPosition = yOffset;
    }
}
