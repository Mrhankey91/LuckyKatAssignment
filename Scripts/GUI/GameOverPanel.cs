using UnityEngine;

public class GameOverPanel : MonoBehaviour
{
    private Animator animator;

    private bool show = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void ShowGameOverPanel(bool show)
    {
        this.show = show;
        animator.SetBool("show", this.show);
    }
}
