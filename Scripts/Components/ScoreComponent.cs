using UnityEngine;

public class ScoreComponent : MonoBehaviour
{
    private int score = 0;
    public int Score
    {
        get { return score; }
        set { 
            int change = value - score; 
            score = value; 
            onScoreUpdate?.Invoke(score, change); 
        }
    }

    public delegate void OnScoreUpdate(int score, int change);
    public OnScoreUpdate onScoreUpdate;
}
