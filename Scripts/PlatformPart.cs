using System.Collections;
using UnityEngine;

public class PlatformPart : MonoBehaviour, IBouncable
{
    private Rigidbody rigidBody;
    private MeshCollider meshCollider;
    private MeshRenderer meshRenderer;

    public Material goodMaterial;
    public Material badMaterial;
    public Material triggerMaterial;
    public Material finishMaterial;

    private Vector3 startPosition;
    private Quaternion startRotation;

    private Coroutine disableCoroutine;
    private WaitForSeconds disableTime = new WaitForSeconds(2f);

    private bool alreadyBroken = false;
    public PlatformType type = PlatformType.Basic;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        meshCollider = GetComponent<MeshCollider>();
        meshRenderer = GetComponent<MeshRenderer>();

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
        //meshCollider.isTrigger = (type == PlatformType.Trigger || type == PlatformType.Finish);
        alreadyBroken = false;
        gameObject.SetActive(true);
    }

    public void SetPartAsBasic()
    {
        if (type == PlatformType.Basic) { return; }
        type = PlatformType.Basic;
        meshRenderer.material = goodMaterial;
        meshCollider.isTrigger = false;
    }

    public void SetPartAsBad()
    {
        if (type == PlatformType.Bad) { return; }
        type = PlatformType.Bad;
        meshRenderer.material = badMaterial;
        meshCollider.isTrigger = false;
    }

    public void SetPartAsTrigger()
    {
        if (type == PlatformType.Trigger) { return; }
        type = PlatformType.Trigger;
        meshRenderer.material = triggerMaterial;
        meshCollider.isTrigger = true;
    }

    public void SetPartAsFinish()
    {
        type = PlatformType.Finish;
        meshRenderer.material = finishMaterial;
        meshCollider.isTrigger = true;
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

    public bool Bounce()
    {
        return type != PlatformType.Bad;
    }
}
