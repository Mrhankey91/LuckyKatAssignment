using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    private LevelController levelController;
    private Ball ball;

    private RectMask2D mask;

    private float zeroProgressPosition; // use to set mask bottom padding

    void Start()
    {
        levelController = GameObject.Find("GameController").GetComponent<LevelController>();
        ball = GameObject.Find("Ball").GetComponent<Ball>();
        mask = transform.Find("Mask").GetComponent<RectMask2D>();
        zeroProgressPosition = GetComponent<RectTransform>().rect.height;
        mask.padding = new Vector4(0f, zeroProgressPosition, 0f, 0f);
    }

    void Update()
    {
        //UpdateProgressBar(ball.GetHighestYPosition() / levelController.GetFinishLinePosition());
        UpdateProgressBar(ball.transform.position.y / levelController.GetFinishLinePosition());
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
}
