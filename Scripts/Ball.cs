using System;
using System.Collections;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private GameController gameController;
    private LevelController levelController;
    private DecalsController decalsController;
    private SoundPlayComponent soundPlayComponent;
    private Rigidbody rigidbody;
    private Animator animator;
    private TrailRenderer trailRenderer;

    private Vector3 startPosition;
    private float bounceForce = 5f;
    private bool addedForce = false;
    private int frameWait = 0;
    private Vector3 velocityBeforePhysicsUpdate;

    public int passedPlatforms = 0;

    private int platformsPassedWithoutHits = 0;

    private float lowestYPosition;

    private void Awake()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        levelController = gameController.GetComponent<LevelController>();
        decalsController = gameController.GetComponent<DecalsController>();
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        soundPlayComponent = GetComponent<SoundPlayComponent>();
        trailRenderer = transform.Find("Trail").GetComponent<TrailRenderer>();

        gameController.onRestarLevel += OnRestarLevel;

        startPosition = transform.position;

        StartCoroutine(EndFixedUpdateFrame());
    }

    private void Update()
    {
        lowestYPosition = Mathf.Min(lowestYPosition, transform.position.y);

        if (transform.position.y <= -levelController.distanceBetweenPlatforms * passedPlatforms)
        {
            passedPlatforms++; 
            platformsPassedWithoutHits++;
            levelController.PassedPlatform(passedPlatforms);
        }

        if (!trailRenderer.emitting && rigidbody.velocity.y < 0)
        {
            trailRenderer.emitting = true;
        }
    }
    void FixedUpdate()
    {
        velocityBeforePhysicsUpdate = rigidbody.velocity;
    }

    private void LateUpdate()
    {
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (addedForce) return; //Check if force been added same frame, prevents addforce multiple times when hitting multiple platforms same time

        //Ball is below the collider
        if (collision.contacts[0].normal.y < 0.55f)
        {
            levelController.BreakPlatform(passedPlatforms + 1);
            rigidbody.velocity = velocityBeforePhysicsUpdate;
            return;
        }

        if (platformsPassedWithoutHits >= 2)
        {
            levelController.BreakPlatform(passedPlatforms + 1);
        }

        if (collision.transform.TryGetComponent(out IBouncable bounceable))
        {
            if (bounceable.Bounce())
            {
                soundPlayComponent?.PlayAudioClip("bounce");
                rigidbody.AddForce(new Vector3(0f, bounceForce, 0f), ForceMode.Impulse);
                frameWait = 0;
                addedForce = true;
                platformsPassedWithoutHits = 0;
                decalsController.SpawnDecal(collision.contacts[0].point);
                animator?.SetTrigger("bounce");
                trailRenderer.emitting = false;
            }
            else
            {
                gameController.GameOver();
            }
        }
    }

    private IEnumerator EndFixedUpdateFrame()
    {
        while (true) { 
            yield return new WaitForFixedUpdate();
            if (addedForce)
            {
                frameWait++;
                if (frameWait >= 5)
                {
                    addedForce = false;
                }
            }
        }
    }

    public float GetLowestYPosition()
    {
        return lowestYPosition;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void OnRestarLevel()
    {
        transform.position = startPosition;
        lowestYPosition = 0f; 
        passedPlatforms = 0;
        platformsPassedWithoutHits = 0;
        rigidbody.velocity = Vector3.zero;
    }
}
