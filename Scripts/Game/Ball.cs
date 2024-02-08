using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody rigidbody;
    private float bounceForce = 5f;
    private bool addedForce = false;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if(addedForce) { addedForce = false; }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (addedForce) return; //CHeck if force been added same frame, prevents addforce multiple times when hitting multiple platforms same time
        
        rigidbody.AddForce(new Vector3(0f, bounceForce, 0f), ForceMode.Impulse);
        addedForce = true;
    }
}
