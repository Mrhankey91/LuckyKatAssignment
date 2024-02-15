using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    private LevelController levelController;
    private PlayerProgress playerProgress;

    private RectMask2D mask;
    private TMP_Text currentLevelLabel;
    private TMP_Text nextLevelLabel;

    private float zeroProgressPosition; // use to set mask bottom padding

    void Start()
    {
        levelController = GameObject.Find("GameController").GetComponent<LevelController>();
        playerProgress = GameObject.Find("Ball").GetComponent<PlayerProgress>();
        mask = transform.Find("Mask").GetComponent<RectMask2D>();
        zeroProgressPosition = GetComponent<RectTransform>().rect.height;
        mask.padding = new Vector4(0f, zeroProgressPosition, 0f, 0f);
        currentLevelLabel = transform.Find("CurrentLabel").GetComponent<TMP_Text>();
        nextLevelLabel = transform.Find("NextLabel").GetComponent<TMP_Text>();

        levelController.GetComponent<GameController>().onLevelCompleted += OnLevelCompleted;
        currentLevelLabel.text = levelController.GetCurrentLevelId().ToString();
        nextLevelLabel.text = levelController.GetNextLevelId().ToString();
    }

    void Update()
    {
        UpdateProgressBar(playerProgress.GetProgress());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="percentage">0f to 1f, 1f is level completed</param>
    public void UpdateProgressBar(float percentage)
    {
        //percentage /= 100f;
        mask.padding = new Vector4(0f, 0f, 0f, zeroProgressPosition * (1f - percentage));
    }

    private void OnLevelCompleted(int level)
    {
        currentLevelLabel.text = levelController.GetCurrentLevelId().ToString();
        nextLevelLabel.text = levelController.GetNextLevelId().ToString();
    }
}
