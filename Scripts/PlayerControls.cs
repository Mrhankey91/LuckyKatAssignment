using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    private Transform helixTransform;

    private Vector2 rotateInput;
    private float rotateSpeed = 10f;
    public bool click = false;//if mouse/touch is down

    void Awake()
    {
        helixTransform = GameObject.Find("Helix").transform;
    }

    void Update()
    {
        helixTransform.eulerAngles += new Vector3(0f, rotateSpeed * rotateInput.x * Time.deltaTime, 0f);
    }

    private void OnRotateMouseTouch(InputValue value)
    {
        if (click)
        {
            rotateInput = value.Get<Vector2>();
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
        click = value.Get<float>() == 1;
    }
}
