using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    private Ball ball;
    private Transform helixTransform;

    private Vector2 rotateInput;
#if UNITY_ANDROID || UNITY_IOS
    private float rotateSpeed = 5f;
#else
    private float rotateSpeed = 10f;
#endif
    public bool click = false;//if mouse/touch is down
    private Vector2 swipeDirection;

    public LayerMask rayCastLayers;
    private float rayCastRadius = 0.48f;
    private float rayCastDistance = 0.12f;

    void Awake()
    {
        ball = GameObject.Find("Ball").GetComponent<Ball>();
        helixTransform = GameObject.Find("Helix").transform;
    }

    void Update()
    {
        if(CanMoveDirection(rotateInput.x))
            helixTransform.eulerAngles += new Vector3(0f, rotateSpeed * rotateInput.x * Time.deltaTime, 0f);
    }

    //Check if any platform in way of ball, helps reduce times ball gets in a platform
    private bool CanMoveDirection(float direction)
    {
        if(direction == 0f) return false;

        RaycastHit hit;
        Vector3 dir;
        if(PlayerPrefs.GetInt("InvertMovement", 0) == 0)
            dir = ball.transform.TransformDirection(direction > 0 ? Vector3.right : Vector3.left);
        else
            dir = ball.transform.TransformDirection(direction > 0 ? Vector3.left : Vector3.right);

        if (Physics.SphereCast(ball.transform.position - (dir * 0.1f), rayCastRadius, dir, out hit, rayCastDistance, rayCastLayers))
        {
            return false;
        }

        return true;
    }

    private void MouseTouchRelease()
    {
        //ball.jump = true;
        rotateInput = Vector2.zero;

        if (Mathf.Abs(swipeDirection.y) > 2f && Mathf.Abs(swipeDirection.x) < Mathf.Abs(swipeDirection.y))
            ball.jump = true;
    }

    private void OnRotateMouseTouch(InputValue value)
    {
        if (click)
        {
            rotateInput = (PlayerPrefs.GetInt("InvertMovement", 0) == 0 ? -1 : 1) * value.Get<Vector2>() * PlayerPrefs.GetFloat("Sensitivity", 1f);
            swipeDirection += value.Get<Vector2>();
        }
        else
        {
            rotateInput = Vector2.zero;
        }
    }

    private void OnRotate(InputValue value)
    {
        rotateInput = value.Get<Vector2>() * 10f;
    }

    private void OnClick(InputValue value)
    {
        bool temp = value.Get<float>() == 1;

        if (temp == click) return;
        click = temp;

        if (!click)
        {
            MouseTouchRelease();
        }
        else
        {
            swipeDirection = Vector2.zero;
        }
    }

    private void OnTap(InputValue value)
    {
        ball.jump = true;
    }

    private void OnJump(InputValue value)
    {
        ball.jump = true;
    }
}
