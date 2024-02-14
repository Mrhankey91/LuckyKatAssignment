using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    protected LevelController levelController;

    public string id;

    protected Vector3 startPosition;
    protected Quaternion startRotation;

    protected virtual void Awake()
    {
        levelController = GameObject.Find("GameController").GetComponent<LevelController>();
    }

    public virtual void Init()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    public virtual void Reset()
    {
        transform.position = startPosition;
        transform.rotation = startRotation;
    }

    public virtual void DisableObject()
    {
        levelController.DisableObject(this);
    }
}
