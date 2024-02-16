using UnityEngine;

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