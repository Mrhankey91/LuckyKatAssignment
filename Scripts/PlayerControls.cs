using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    private Ball ball;
    private Transform helixTransform;

    private Vector2 rotateInput;
    private float rotateSpeed = 10f;
    public bool click = false;//if mouse/touch is down

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
    }

    private void OnRotateMouseTouch(InputValue value)
    {
        if (click)
        {
            rotateInput = -value.Get<Vector2>();
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
    }

    private void OnTap(InputValue value)
    {
        ball.jump = true;
    }
}
