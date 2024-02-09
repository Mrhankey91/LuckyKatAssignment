using UnityEngine;

public class GameController : MonoBehaviour
{
    public delegate void OnGameOver();
    public OnGameOver onGameOver;

    public void GameOver()
    {
        Debug.Log("GameOver");
        Time.timeScale = 0f;
        onGameOver?.Invoke();
    }
}
