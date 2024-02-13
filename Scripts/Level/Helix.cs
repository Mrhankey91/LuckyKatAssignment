using System;
using UnityEngine;

namespace Assets.Scripts.Level
{
    public class Helix : MonoBehaviour
    {
        void Awake()
        {
            GameObject.Find("GameController").GetComponent<GameController>().onRestarLevel += OnRestartLevel;
        }

        private void OnRestartLevel()
        {
            transform.rotation = Quaternion.identity;
        }
    }
}