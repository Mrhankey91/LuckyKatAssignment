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

    void Awake()
    {
        ball = GameObject.Find("Ball").GetComponent<Ball>();
        helixTransform = GameObject.Find("Helix").transform;
    }

    void Update()
    {
        helixTransform.eulerAngles += new Vector3(0f, rotateSpeed * rotateInput.x * Time.deltaTime, 0f);
    }

    private void MouseTouchRelease()
    {
        //ball.jump = true;
        rotateInput = Vector2.zero;

        if (swipeDirection.y > 2f && Mathf.Abs(swipeDirection.x) < swipeDirection.y)
            ball.jump = true;
    }

    private void OnRotateMouseTouch(InputValue value)
    {
        if (click)
        {
            rotateInput = -value.Get<Vector2>();
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
        //ball.jump = true;
    }
}
