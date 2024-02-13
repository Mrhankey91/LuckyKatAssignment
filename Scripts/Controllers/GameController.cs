using UnityEngine;

public class GameController : MonoBehaviour
{
    private GameOverPanel gameOverPanel;

    public delegate void OnGameOver();
    public OnGameOver onGameOver;

    public delegate void OnRestarLevel();
    public OnRestarLevel onRestarLevel;

    private void Awake()
    {
        gameOverPanel = GameObject.Find("GameOverPanel").GetComponent<GameOverPanel>();
    }

    public void Restart()
    {
        gameOverPanel.ShowGameOverPanel(false);
        onRestarLevel?.Invoke();
        Time.timeScale = 1f;
    }

    public void Continue()
    {
        gameOverPanel.ShowGameOverPanel(false);
        Time.timeScale = 1f;
    }

    public void GameOver()
    {
        Time.timeScale = 0f;
        onGameOver?.Invoke();
        gameOverPanel.ShowGameOverPanel(true);
    }
}
