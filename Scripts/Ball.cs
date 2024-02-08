using UnityEngine;

public class Ball : MonoBehaviour
{
    private LevelController levelController;
    private SoundPlayComponent soundPlayComponent;
    private Rigidbody rigidbody;

    private float bounceForce = 5f;
    private bool addedForce = false;
    private int frameWait = 0;

    public int passedPlatforms = 0;

    private void Awake()
    {
        levelController = GameObject.Find("GameController").GetComponent<LevelController>();
        rigidbody = GetComponent<Rigidbody>();
        soundPlayComponent = GetComponent<SoundPlayComponent>();
    }

    private void Update()
    {
        if(transform.position.y <= -levelController.distanceBetweenPlatforms * passedPlatforms)
        {
            passedPlatforms++;
            levelController.PassedPlatform(passedPlatforms);
        }
    }

    private void LateUpdate()
    {
        if(addedForce) {
            frameWait++;
            if (frameWait >= 5)
            {
                addedForce = false;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (addedForce) return; //CHeck if force been added same frame, prevents addforce multiple times when hitting multiple platforms same time

        soundPlayComponent?.PlayAudioClip("bounce");
        rigidbody.AddForce(new Vector3(0f, bounceForce, 0f), ForceMode.Impulse);
        frameWait = 0;
        addedForce = true;
    }

}
