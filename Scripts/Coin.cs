using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : SpawnObject, ICollectable
{
    private CoinsComponent coinsComponent;

    private Transform modelTransform;
    private float rotateSpeed = 90f;

    protected override void Awake()
    {
        base.Awake();
        coinsComponent = GameObject.Find("GameController").GetComponent<CoinsComponent>();
        modelTransform = transform.Find("CoinModel");
    }

    void Update()
    {
        modelTransform.rotation *= Quaternion.Euler(new Vector3(0f, rotateSpeed * Time.deltaTime, 0f));
    }

    public void Collect()
    {
        coinsComponent.Coins++;
        DisableObject();
    }
}
