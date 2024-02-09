using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    private RectMask2D mask;

    private float zeroProgressPosition; // use to set mask bottom padding

    void Start()
    {
        mask = transform.Find("Mask").GetComponent<RectMask2D>();
        zeroProgressPosition = GetComponent<RectTransform>().rect.height;
        mask.padding = new Vector4(0f, zeroProgressPosition, 0f, 0f);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="percentage">0f to 100f, 100f is level completed</param>
    public void UpdateProgressBar(float percentage)
    {
        percentage /= 100f;
        mask.padding = new Vector4(0f, zeroProgressPosition * (1f - percentage), 0f, 0f);
    }
}
