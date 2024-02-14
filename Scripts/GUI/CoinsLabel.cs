using UnityEngine;
using TMPro;
using System;

public class Coinslabel : MonoBehaviour
{
    private TMP_Text label;

    private void Awake()
    {
        label = GetComponent<TMP_Text>();

        GameObject.Find("GameController").GetComponent<CoinsComponent>().onCoinsUpdate += OnCoinsUpdate;
    }

    private void OnCoinsUpdate(int coins, int change)
    {
        label.text = coins.ToString();
    }
}
