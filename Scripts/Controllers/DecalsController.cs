using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecalsController : MonoBehaviour
{
    private Transform parent;
    public GameObject decalPrefab;

    private int maxDecals = 10;
    private Queue<GameObject> decalQueue = new Queue<GameObject>();
    private Vector3 offset = new Vector3(0f, .1f, 0f);

    private void Awake()
    {
        parent = GameObject.Find("Decals").transform;
    }

    public void SpawnDecal(Vector3 position)
    {
        position += offset;
        if(decalQueue.Count >= maxDecals)
        {
            GameObject reusedDecal = decalQueue.Dequeue();
            reusedDecal.transform.position = position;
            decalQueue.Enqueue(reusedDecal);
        }
        else
        {
            decalQueue.Enqueue(Instantiate(decalPrefab, position, Quaternion.Euler(90f, Random.Range(0,360f), 0f), parent));
        }
    }
}
