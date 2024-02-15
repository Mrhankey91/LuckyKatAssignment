using System.Collections;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private HealthComponent healthComponent;
    private GameOverPanel gameOverPanel;
    private SoundPlayComponent soundPlayComponent;

    public delegate void OnGameOver();
    public OnGameOver onGameOver;

    public delegate void OnRestarLevel();
    public OnRestarLevel onRestarLevel;

    public delegate void OnLevelCompleted(int level);
    public OnLevelCompleted onLevelCompleted;

    private WaitForSeconds shieldTime = new WaitForSeconds(2f);//seconds of shield so player cant take damage after continue

    private void Awake()
    {
        healthComponent = GetComponent<HealthComponent>();
        gameOverPanel = GameObject.Find("GameOverPanel").GetComponent<GameOverPanel>();
        soundPlayComponent = GameObject.Find("Sounds").GetComponent<SoundPlayComponent>();

        healthComponent.onOutOfHealth += OnOutOfHealth;
    }

    public void Restart()
    {
        gameOverPanel.ShowGameOverPanel(false);
        onRestarLevel?.Invoke();
        Time.timeScale = 1f;
        healthComponent.SetFullHealth();
    }

    public void Continue()
    {
        gameOverPanel.ShowGameOverPanel(false);
        Time.timeScale = 1f;
        StartCoroutine(TemporaryShield());
        healthComponent.Health += 1;
    }

    public void GameOver()
    {
        soundPlayComponent.PlayAudioClip("GameOver");
        Time.timeScale = 0f;
        onGameOver?.Invoke();
        gameOverPanel.ShowGameOverPanel(true);
    }

    public void CompletedLevel(int level)
    {
        soundPlayComponent.PlayAudioClip("LevelCompleted");
        onLevelCompleted?.Invoke(level);
    }

    private IEnumerator TemporaryShield()
    {
        healthComponent.canTakeDamage = false;
        yield return shieldTime;
        healthComponent.canTakeDamage = true;
    }

    private void OnOutOfHealth()
    {
        GameOver();
    }
}
