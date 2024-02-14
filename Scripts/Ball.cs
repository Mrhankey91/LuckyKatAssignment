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
    private int collisions = 0;

    public int currentFloor = 0;
    public int reachedFloor = 0;
    private float highestYPosition;

    private Color normalColor;
    public Color powerColor = Color.red;

    public bool jump = false;

    public delegate void OnCurrentFloorChange(int floor, bool isFalling);
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
        BallColor();
    }

    void FixedUpdate()
    {
        velocityBeforePhysicsUpdate = rigidbody.velocity;

        if(collisions > 0 && !addedForce) //stuck in a collider
        {
            float y = transform.position.y % levelController.distanceBetweenFloors;
            rigidbody.AddForce(new Vector3(0f, y < 1f ? 10f : -10f, 0f));
        }
    }

    private void Bounce(Vector3 position)
    {
        if(rigidbody.velocity.y > 1f) return;//Already moving upwards

        soundPlayComponent?.PlayAudioClip(jump ? "jump" : "bounce");
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

        if ((!checkIfGoingDownOnly ) || (checkIfGoingDownOnly && tempFloor < currentFloor))
        {
            bool isFalling = currentFloor > tempFloor;
            currentFloor = tempFloor;
            onCurrentFloorChange?.Invoke(currentFloor, isFalling);
        }

        reachedFloor = Mathf.Max(currentFloor, reachedFloor);
    }

    private void BallColor()
    {
        ballMaterial.color = Color.Lerp(ballMaterial.color, jump ? powerColor : normalColor, Time.deltaTime * 10f);
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
        collisions++;
        if (addedForce) return; //Check if force been added same frame, prevents addforce multiple times when hitting multiple platforms same time

        /*if (collision.contacts[0].normal.x > 0.5f || collision.contacts[0].normal.x < -0.5f)
        {
            rigidbody.AddForce(new Vector3(0f, -2f, 0f), ForceMode.Impulse);
            //rigidbody.velocity = velocityBeforePhysicsUpdate;
            return;
        }*/

        if (collision.transform.TryGetComponent(out IDamage damage))
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

    private void OnCollisionExit(Collision collision)
    {
        collisions--;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (addedForce) return; //Check if force been added same frame, prevents addforce multiple times when hitting multiple platforms same time

        if(other.transform.TryGetComponent(out PlatformPart part))
        {
            scoreComponent.Score += part.platformValue;
        }

        if(other.transform.TryGetComponent(out ICollectable collectable))
        {
            collectable.Collect();
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
