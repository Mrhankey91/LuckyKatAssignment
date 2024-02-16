using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saw : SpawnObject, IBouncable
{
    private Transform bladeTransform;

    private bool show = true;

    private Vector3 showPosition;
    private Vector3 hidePosition = new Vector3(0f,-.1f, 1.25f);

    private WaitForSeconds timeBetween = new WaitForSeconds(3f);

    protected override void Awake()
    {
        base.Awake();

        bladeTransform = transform.Find("SawBlade");
        showPosition = bladeTransform.localPosition;

        StartCoroutine(ShowOrHide());
    }

    private IEnumerator ShowOrHide()
    {
        yield return timeBetween;
        show = !show;

        float time = 0f;
        float duration = 2f;

        while (time < duration)
        {
            time += Time.deltaTime;

            bladeTransform.localPosition = Vector3.Lerp(show ? hidePosition : showPosition, show ? showPosition : hidePosition, time / duration);
            yield return null;

            if (time >= duration) break;
        }

        StartCoroutine(ShowOrHide());
    }

    public bool Bounce()
    {
        return true;
    }
}
