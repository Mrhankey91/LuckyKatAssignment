using UnityEngine;
using TMPro;
using System;

public class Scorelabel : MonoBehaviour
{
    private TMP_Text label;

    private void Awake()
    {
        label = GetComponent<TMP_Text>();

        GameObject.Find("GameController").GetComponent<ScoreComponent>().onScoreUpdate += OnScoreUpdate;
    }

    private void OnScoreUpdate(int score, int change)
    {
        label.text = score.ToString();
    }
}
