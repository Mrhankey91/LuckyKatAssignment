using UnityEngine;

public class Ball : MonoBehaviour
{
    private LevelController levelController;
    private DecalsController decalsController;
    private SoundPlayComponent soundPlayComponent;
    private Rigidbody rigidbody;

    private float bounceForce = 5f;
    private bool addedForce = false;
    private int frameWait = 0;
    private Vector3 velocityBeforePhysicsUpdate;

    public int passedPlatforms = 0;

    private float platformsPassedWithoutHits = 0;

    private void Awake()
    {
        levelController = GameObject.Find("GameController").GetComponent<LevelController>();
        decalsController = levelController.GetComponent<DecalsController>();
        rigidbody = GetComponent<Rigidbody>();
        soundPlayComponent = GetComponent<SoundPlayComponent>();
    }

    private void Update()
    {
        if(transform.position.y <= -levelController.distanceBetweenPlatforms * passedPlatforms)
        {
            passedPlatforms++; 
            platformsPassedWithoutHits++;
            levelController.PassedPlatform(passedPlatforms);
        }
    }
    void FixedUpdate()
    {
        velocityBeforePhysicsUpdate = rigidbody.velocity;
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
        if (addedForce) return; //Check if force been added same frame, prevents addforce multiple times when hitting multiple platforms same time

        //Ball is below the collider
        if (collision.contacts[0].normal.y < 0.75f)
        {
            levelController.BreakPlatform(passedPlatforms + 1);
            rigidbody.velocity = velocityBeforePhysicsUpdate;
            return;
        }

        if (platformsPassedWithoutHits >= 2)
        {
            levelController.BreakPlatform(passedPlatforms + 1);
        }

        soundPlayComponent?.PlayAudioClip("bounce");
        rigidbody.AddForce(new Vector3(0f, bounceForce, 0f), ForceMode.Impulse);
        frameWait = 0;
        addedForce = true;
        platformsPassedWithoutHits = 0;
        decalsController.SpawnDecal(collision.contacts[0].point);
    }

}
