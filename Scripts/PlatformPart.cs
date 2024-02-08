using System.Collections;
using UnityEngine;

public class PlatformPart : MonoBehaviour
{
    private Rigidbody rigidBody;
    private MeshCollider meshCollider;

    private Vector3 startPosition;
    private Quaternion startRotation;

    private Coroutine disableCoroutine;
    private WaitForSeconds disableTime = new WaitForSeconds(2f);

    private bool alreadyBroken = false;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        meshCollider = GetComponent<MeshCollider>();

        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    public void Reset()
    {
        if (disableCoroutine != null)
        { 
            StopCoroutine(disableCoroutine); 
            disableCoroutine = null; 
        }

        rigidBody.isKinematic = true;
        transform.position = startPosition;
        transform.rotation = startRotation;
        meshCollider.enabled = true;
        alreadyBroken = false;
        gameObject.SetActive(true);
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }

    public void PlatformPassedByBall()
    {
        if (!isActiveAndEnabled || alreadyBroken) return;

        meshCollider.enabled = false;
        rigidBody.isKinematic = false;
        rigidBody.AddRelativeForce((Vector3.forward + new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f))) * 2f, ForceMode.Impulse);
        rigidBody.angularVelocity = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
        disableCoroutine = StartCoroutine(DisableAfterTime());
        alreadyBroken = true;
    }

    private IEnumerator DisableAfterTime()
    {
        yield return disableTime;
        gameObject.SetActive(false);
        disableCoroutine = null;
    }
}
