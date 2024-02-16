using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : SpawnObject, ICollectable
{
    private CoinsComponent coinsComponent;
    private Transform chestModelTransform;
    private Transform chestTopTransform;
    private ParticleSystem particleSystem;

    public GameObject[] coins;

    private bool opened = false;
    private int value = 5;
    private WaitForSeconds coinsShowTime = new WaitForSeconds(2f);

    private Vector3 closedRotation = Vector3.zero;//euler angles
    private Vector3 openedRottation= new Vector3(-110f, 0f, 0f);//euler angles

    protected override void Awake()
    {
        base.Awake();
        coinsComponent = GameObject.Find("GameController").GetComponent<CoinsComponent>();
        chestModelTransform = transform.Find("Chest");
        chestTopTransform = transform.Find("Chest/ChestTop");
        particleSystem = transform.Find("ParticleSystem").GetComponent<ParticleSystem>();
    }

    public override void Init()
    {
        base.Init();

        opened = false;
        chestTopTransform.localRotation = Quaternion.Euler(closedRotation);
    }

    public void Collect()
    {
        if (opened) return; //already opnened

        opened = true;
        StartCoroutine(OpenChest());
    }

    private IEnumerator OpenChest()
    {
        float duration = 0.5f;
        float time = 0f;

        particleSystem.Play();

        while (time < duration)
        {
            time += Time.deltaTime;
            yield return null;
            chestTopTransform.localRotation = Quaternion.Euler(Vector3.Lerp(closedRotation, openedRottation, time / duration));

            if(time > duration) { break; }
        }

        coinsComponent.Coins += value;
        StartCoroutine(ShowCoins());
    }

    private IEnumerator ShowCoins()
    {
        foreach (GameObject coin in coins)
        {
            coin.SetActive(true);
            coin.transform.position = chestModelTransform.position;
            coin.transform.rotation = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
            coin.GetComponent<Rigidbody>().AddForce(new Vector3(0f, Random.Range(3f, 4f), 0f), ForceMode.Impulse);
        }

        yield return coinsShowTime;

        foreach (GameObject coin in coins)
        {
            coin.SetActive(false);
        }
    }
}
