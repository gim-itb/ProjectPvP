using Pathfinding;
using UnityEngine;
using System.Collections;
using Pathfinding.Util;

public class PlayerDetection : MonoBehaviour
{

    [Header("Enemy Speed Property")]
    [SerializeField] float DetectionRadius; //target detection
    [SerializeField] float NextWaypoint = 3f;
    [SerializeField] float BackToPosSpeed; //the speed to go back to the first pos after target get out from detection
    [SerializeField] float EnemySpeed = 5f; // the speed to chase the target
    [SerializeField] float PathUpdateTime = 0.5f;

    [Header("Jump Property")]
    [SerializeField] float JumpPower = 3f; // The Power to jump
    [SerializeField] float JumpHeightLimit = 0.5f; // The boundary that will detect whether the player need to jump or not
    private float JumpOffset =0.1f; // For the Raycast as an additional ray length to check if the enemy is grounded

    private Rigidbody2D rb; // enemy rigidbody
    private CircleCollider2D enemyCollider; // pseudo collider
    private bool EnemyState = false; // it will detect the player that enter the area
    private int currentWaypoint = 0;
    private GameObject player;
    private bool OnOffCheck = false; // checking if the player that go outside of the radius go back again
    private bool isGrounded;
    [Header("Enemy Time Property")]
    [SerializeField] float timeOffset= 0.5f;

    [Header("EnemyGravity")]
    [SerializeField] private float GravityScaleModifier = 2f;
    [SerializeField] float currentGravityScale;

    private Seeker seeker;
    private Path path;

    
    private void Awake()
    { 
        player = GameObject.FindGameObjectWithTag("Player");
        enemyCollider = transform.GetComponent<CircleCollider2D>();
        enemyCollider.radius = DetectionRadius;
        seeker = transform.GetComponent<Seeker>();
        rb = transform.GetComponent<Rigidbody2D>();
        rb.gravityScale = currentGravityScale;
        InvokeRepeating("UpdatePath", 0f, PathUpdateTime);

    }

    void onPathComplete (Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }
    private void FixedUpdate()
    {
        if (EnemyState)
        {
            PathFollow();
        }
    }
    private void PathFollow()
    {
        if (path == null)
        {
            return;
        }
        if (currentWaypoint >= path.vectorPath.Count)
        {
            return;
        }
        Gravity();
        RaycastHit2D Grounded = Physics2D.Raycast(transform.position, Vector2.down, GetComponent<Collider2D>().bounds.extents.y + JumpOffset);
        Debug.DrawRay(transform.position, Vector2.down * (GetComponent<Collider2D>().bounds.extents.y + JumpOffset), Color.green);
        Debug.Log(Grounded);
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 movement = EnemySpeed * Time.fixedDeltaTime * direction;
        if (Grounded.collider != null && (direction.y >= JumpHeightLimit))
        {
            rb.AddForce(JumpPower * Vector2.up, ForceMode2D.Impulse);
        }
        //movement
        rb.AddForce(movement * Vector2.right, ForceMode2D.Force);
       

        float Distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if(Distance < NextWaypoint)
        {
            currentWaypoint++;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        {
            EnemyState = true;
            OnOffCheck = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OnOffCheck = false;
            StartCoroutine(OffsetTime(timeOffset));
        }
    }

    private IEnumerator OffsetTime(float time)
    {
        yield return new WaitForSeconds(time);
        if(!OnOffCheck)
        {
            EnemyState = false;
        }
    }
    private void UpdatePath()
    {
        if (EnemyState == true)
        {
            seeker.StartPath(rb.position, player.transform.position, onPathComplete);
        }
    }

    private void Gravity()
    {
        if(rb.velocity.y > 0)
        {
            rb.gravityScale = currentGravityScale;
        }
        else
        {
            rb.gravityScale = currentGravityScale + GravityScaleModifier;
        }
    }
}