using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    private Vector3 cameraPosition;
    private float yOffset;
    private float lowestYPosition;

    private void Awake()
    {
        cameraPosition = transform.position;
        yOffset = cameraPosition.y;
        lowestYPosition = yOffset;
    }

    void Update()
    {
        cameraPosition.y = Mathf.Min(lowestYPosition, target.position.y + yOffset);
        transform.position = cameraPosition;
        lowestYPosition = cameraPosition.y;
    }
}
