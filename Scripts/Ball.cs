using System;
using System.Collections;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private GameController gameController;
    private LevelController levelController;
    private DecalsController decalsController;
    private SoundPlayComponent soundPlayComponent;
    private ScoreComponent scoreComponent;
    private Rigidbody rigidbody;
    private Animator animator;
    private TrailRenderer trailRenderer;
    private Material ballMaterial;

    private Vector3 startPosition;
    private float bounceForce = 5f;
    private bool addedForce = false;
    private int frameWait = 0;
    private Vector3 velocityBeforePhysicsUpdate;

    public int passedPlatforms = 0;
    private int platformsPassedWithoutHits = 0;
    private int platformsPassedBeforePowerMode = 2;
    private float lowestYPosition;

    private Color normalColor;
    public Color powerColor = Color.red;

    private void Awake()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        levelController = gameController.GetComponent<LevelController>();
        decalsController = gameController.GetComponent<DecalsController>();
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        soundPlayComponent = GetComponent<SoundPlayComponent>();
        scoreComponent = gameController.GetComponent<ScoreComponent>();
        trailRenderer = transform.Find("Trail").GetComponent<TrailRenderer>();
        ballMaterial = transform.Find("BallRenderer").GetComponent<MeshRenderer>().material;//use material instead of sharedmaterial, otherwise global material will be changed
        
        gameController.onRestarLevel += OnRestarLevel;

        startPosition = transform.position;
        normalColor = ballMaterial.color;

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

        BallColor();
    }

    void FixedUpdate()
    {
        velocityBeforePhysicsUpdate = rigidbody.velocity;
    }

    private void Bounce(Vector3 position)
    {
        soundPlayComponent?.PlayAudioClip("bounce");
        rigidbody.AddForce(new Vector3(0f, bounceForce, 0f), ForceMode.Impulse);
        frameWait = 0;
        addedForce = true;
        platformsPassedWithoutHits = 0;
        decalsController.SpawnDecal(position);
        animator?.SetTrigger("bounce");
        trailRenderer.emitting = false;
    }

    private void BallColor()
    {
        ballMaterial.color = Color.Lerp(ballMaterial.color, platformsPassedWithoutHits > platformsPassedBeforePowerMode ? powerColor : normalColor, Time.deltaTime);
        trailRenderer.startColor = trailRenderer.endColor = ballMaterial.color;
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

    private void OnCollisionEnter(Collision collision)
    {
        if (addedForce) return; //Check if force been added same frame, prevents addforce multiple times when hitting multiple platforms same time

        //Ball is below or beside the collider
        if (collision.contacts[0].normal.y < 0.55f)
        {
            levelController.BreakPlatform(passedPlatforms + 1);
            rigidbody.velocity = velocityBeforePhysicsUpdate;
            return;
        }

        if (platformsPassedWithoutHits > platformsPassedBeforePowerMode)
        {
            levelController.BreakPlatform(passedPlatforms + 1);
        }

        if (collision.transform.TryGetComponent(out IBouncable bounceable))
        {
            if (bounceable.Bounce())
            {
                Bounce(collision.contacts[0].point);
            }
            else
            {
                gameController.GameOver();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (addedForce) return; //Check if force been added same frame, prevents addforce multiple times when hitting multiple platforms same time

        if(other.transform.TryGetComponent(out PlatformPart part))
        {
            scoreComponent.Score += part.platformValue;
        }
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
