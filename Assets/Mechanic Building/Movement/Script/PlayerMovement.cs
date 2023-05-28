using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float MovementSpeed = 20f; 
    [SerializeField] float exponentVelocity = 1f; // This is used in the acceleration to get an exponential power movement patern
    [SerializeField] float Accelerate = 0.1f;
    [SerializeField] float Decelerate = -0.1f;
    [Space(10f)]
    public float frictionPower; // This is to slow down player
    private float horizontalMove; // This is to handle the x axis player move;
    private Rigidbody2D playerRigidbody;

    [Header("Jump Attribute")]
    [SerializeField] private float JumpPower = 10f;
    [SerializeField] private float coyoteTime = 0.5f;
    [SerializeField] private float bufferingTime = 0.5f;
    [SerializeField] private float gravityModifier; // 0 - 1
    [SerializeField] private float playerGravity = 5f;
    [SerializeField] float gravityScale;

    private float groundedTime;
    private float lastJumpTime;

    [Header("GroundCheck")]
    [SerializeField] private Transform groundPoint;
    [SerializeField] private Vector2 boxSize = new Vector2(0.5f, 0.03f); // OverlapBox Size to check whether the player is Grounded
    [SerializeField] private LayerMask groundMask;

    [Header("Option")]
    public KeyCode jumpKey = KeyCode.Space;



    private void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerRigidbody.freezeRotation = true;
        gravityScale = playerRigidbody.gravityScale;
    }

    private float MoveHorizontal()
    {
        horizontalMove = Input.GetAxis("Horizontal");
        float targetSpeed = horizontalMove * MovementSpeed;
        float speedDiff = targetSpeed - playerRigidbody.velocity.x;
        float speedAccel = Mathf.Abs(speedDiff) > 0.01f ? Accelerate : Decelerate;
        float movement = Mathf.Pow(Mathf.Abs(speedDiff) * speedAccel, exponentVelocity) * Mathf.Sign(speedDiff);
        return movement;
    }

    private void Friction()
    {
        if (groundedTime > 0f && Mathf.Abs(horizontalMove) < 0.01f)
        {
            float MinAmount = Mathf.Min(Mathf.Abs(frictionPower), Mathf.Abs(playerRigidbody.velocity.x));
            MinAmount *= -(Mathf.Sign(playerRigidbody.velocity.x));
            playerRigidbody.AddForce(Vector2.right * MinAmount, ForceMode2D.Impulse);
            Debug.Log("hehe");
        }
    }
    private void FixedUpdate()
    {
        #region Movement
        horizontalMove = Input.GetAxis("Horizontal");
        float targetSpeed = horizontalMove * MovementSpeed;
        float speedDiff = targetSpeed - playerRigidbody.velocity.x;
        float speedAccel = Mathf.Abs(speedDiff) > 0.01f ? Accelerate : Decelerate;
        float movement = Mathf.Pow(Mathf.Abs(speedDiff) * speedAccel, exponentVelocity) * Mathf.Sign(speedDiff);
        playerRigidbody.AddForce(movement * Vector2.right);
        #endregion
        #region Friction
        if (groundedTime > 0f && Mathf.Abs(horizontalMove) < 0.01f)
        {
            float MinAmount = Mathf.Min(Mathf.Abs(frictionPower), Mathf.Abs(playerRigidbody.velocity.x));
            MinAmount *= -(Mathf.Sign(playerRigidbody.velocity.x));
            playerRigidbody.AddForce(Vector2.right * MinAmount, ForceMode2D.Impulse);
        }
        #endregion
        Jump();
        GravityFall();
    }

    private void Jump()
    {
        if (Physics2D.OverlapBox(groundPoint.position, boxSize, 0, groundMask))
        {
            groundedTime = coyoteTime;
        }
        else
        {
            groundedTime -= Time.deltaTime;
        }
        if (Input.GetKey(jumpKey))
        {
            lastJumpTime = bufferingTime;
        }
        else
        {
            lastJumpTime -= Time.deltaTime;
        }
        if(lastJumpTime > 0f && groundedTime > 0f)
        {
            playerRigidbody.AddForce(JumpPower * Vector2.up, ForceMode2D.Impulse);
            lastJumpTime = 0f;
        }
        if (Input.GetKeyUp(jumpKey) && playerRigidbody.velocity.y > 0f)
        {
            playerRigidbody.AddForce(playerRigidbody.velocity.y * gravityModifier * Vector2.down,ForceMode2D.Force);
            groundedTime = 0f;
        }
    }

    private void GravityFall()
    {
        if (playerRigidbody.velocity.y <= 0f)
        {
            playerRigidbody.gravityScale = playerGravity * gravityScale;
        }
        else
        {
            playerRigidbody.gravityScale = playerGravity;
        }
    }
}
