using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float speedAcceleration;
    [SerializeField] float speedDecceleration;
    [SerializeField] float speedAccelerationOnAir;
    [SerializeField] float speedDeccelerationOnAir;
    [SerializeField] float timeOnGround;
    [SerializeField] Transform groundCheck;
    [SerializeField] Vector2 groundPoint = new Vector2(0.5f, 0.1f);
    [SerializeField] LayerMask groundMask;
    [SerializeField] float jumpForce;
    private Rigidbody2D rb;
    private float InputMove;
    private bool isJumping;
    [SerializeField] KeyCode jumpKey = KeyCode.Space;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();   
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(jumpKey))
        {
            isJumping = true;
        }
        InputMove = Input.GetAxisRaw("Horizontal");
        if (Physics2D.OverlapBox(groundCheck.position, groundPoint, 0, groundMask))
        {
            timeOnGround = 0.1f;
        }
        else
        {
            timeOnGround -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        Run();
    }

    private void Run()
    {
        float TargetSpeed = InputMove * speed;
        float accelrate = 0f;
        #region Acceleration Calculation
        if (timeOnGround > 0)
        {
            accelrate = (Mathf.Abs(TargetSpeed) > 0.1f) ? speedAcceleration : speedDecceleration;   
        }
        else
        {
            accelrate = (Mathf.Abs(TargetSpeed) > 0.1f) ? speedAcceleration * speedAccelerationOnAir : speedDecceleration * speedDeccelerationOnAir;
        }
        #endregion

    }

    private void Jump()
    {
        #region Jump
        if (isJumping && timeOnGround > 0)
        {
            rb.AddForce(jumpForce * Vector2.up);

        }
        #endregion
        #region JumpCut

        #endregion
    }
}
