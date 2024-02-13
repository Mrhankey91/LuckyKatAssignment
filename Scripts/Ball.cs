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

    public int currentFloor = 0;
    public int reachedFloor = 0;
    private float highestYPosition;

    private Color normalColor;
    public Color powerColor = Color.red;

    public bool jump = false;

    public delegate void OnCurrentFloorChange(int floor);
    public OnCurrentFloorChange onCurrentFloorChange;

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
        highestYPosition = Mathf.Max(highestYPosition, transform.position.y);

        if (!trailRenderer.emitting && rigidbody.velocity.y < 0)
        {
            trailRenderer.emitting = true;
        }

        CheckCurrentFloor(true);
        //BallColor();
    }

    void FixedUpdate()
    {
        velocityBeforePhysicsUpdate = rigidbody.velocity;
    }

    private void Bounce(Vector3 position)
    {
        soundPlayComponent?.PlayAudioClip("bounce");
        rigidbody.AddForce(new Vector3(0f, bounceForce * (jump ? 2f : 1f), 0f), ForceMode.Impulse);
        frameWait = 0;
        addedForce = true;
        decalsController.SpawnDecal(position);
        animator?.SetTrigger("bounce");
        trailRenderer.emitting = false;
        jump = false;
        CheckCurrentFloor();
    }

    private void CheckCurrentFloor(bool checkIfGoingDownOnly = false)
    {
        int tempFloor = Mathf.FloorToInt(transform.position.y / levelController.distanceBetweenFloors);

        if ((!checkIfGoingDownOnly && tempFloor != currentFloor) || (checkIfGoingDownOnly && tempFloor < currentFloor))
        {
            currentFloor = tempFloor;
            onCurrentFloorChange?.Invoke(currentFloor);
        }

        reachedFloor = Mathf.Max(currentFloor, reachedFloor);
    }

    private void BallColor()
    {
        ballMaterial.color = Color.Lerp(ballMaterial.color, false ? powerColor : normalColor, Time.deltaTime);
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

    public float GetHighestYPosition()
    {
        return highestYPosition;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (addedForce) return; //Check if force been added same frame, prevents addforce multiple times when hitting multiple platforms same time

        if(collision.transform.TryGetComponent(out IDamage damage))
        {
            if (damage.Damage() > 0)
            {
                gameController.GameOver();
            }
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
        highestYPosition = 0f; 
        reachedFloor = 0;
        currentFloor = 0;
        rigidbody.velocity = Vector3.zero;
        jump = false; 
    }
}
