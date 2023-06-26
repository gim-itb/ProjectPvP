using UnityEngine;

public class BullCore : Core<BullCore, BullStates>
{
    [Header("Stats")]
    [SerializeField] float _speed = 10f;
    public float Speed => _speed;
    
    [SerializeField] float _damage = 5f;
    public float Damage => _damage;

    [SerializeField] float _detectRadius = 8;
    public float DetectRadius => _detectRadius;

    [SerializeField] float _attackRadius = 0.6f;
    public float AttackRadius => _attackRadius;

    float _stunTimer = 1;
    public float StunTimer { get => _stunTimer; set => _stunTimer = value; }

    [Header("Others")]
    [HideInInspector] public LayerMask PlayerLayerMask = 1 << 0;
    [HideInInspector] public string PlayerTag = "Player";

    [SerializeField] Transform _skinTrans;
    [SerializeField] Transform _freezedSkinTrans;
    [HideInInspector] public Transform ChaseTarget;
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] SpriteRenderer _spriteRenderer;

    void Awake()
    {
        States = new BullStates(this);
        CurrentState = States.Idle();
        CurrentState.StateEnter();
    }


    void FixedUpdate()
    {
        CurrentState.StateFixedUpdate();
    }

    public void Rotation()
    {
        _rb.angularVelocity = 0;
        _rb.rotation = 0;
    }

    public void Move()
    {
        float direction = (ChaseTarget.position - transform.position).x;
        if(direction > 1 || direction < -1)
        {
            direction = Mathf.Sign(direction);
            _spriteRenderer.flipX = direction < 0;
        }

        if((ChaseTarget.position - transform.position).sqrMagnitude < 1.5f)
        {
            Stop();
            return;
        }
        _rb.velocity = new Vector2(direction * _speed, _rb.velocity.y);
    }
    public void Stop()
    {
        _rb.velocity = Vector2.zero;
    }

    bool _isFrozen = false;
    public override void Hurt(HitRequest hitRequest, ref HitResult hitResult)
    {
        hitResult.Type = HitType.Entity;
        _stunTimer = hitRequest.StunDuration;

        if(hitRequest.Element == Element.Ice)
        {
            FreezeSelf(hitRequest);
        }
        else if(hitRequest.Element == Element.Fire)
        {
            BurnSelf(hitRequest);
        }
    }
    void FreezeSelf(HitRequest hitRequest)
    {
        if(CurrentState == States.Freeze())
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.velocity = Vector2.zero;
            rb.AddForce(
                hitRequest.Direction*hitRequest.Knockback, ForceMode2D.Impulse
            );
            return;
        }
        GetComponent<Rigidbody2D>().AddForce(
            hitRequest.Direction*hitRequest.Knockback, 
            ForceMode2D.Impulse
        );

        _skinTrans.gameObject.SetActive(false);
        _freezedSkinTrans.gameObject.SetActive(true);

        // GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

        SwitchState(States.Freeze());
    }
    

    public void UnFreezeSelf()
    {
        _isFrozen = false;
        StaticOnEntityUnfreeze?.Invoke(this);
        _skinTrans.gameObject.SetActive(true);
        _freezedSkinTrans.gameObject.SetActive(false);


        // GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
    }

    void BurnSelf(HitRequest hitObjectParams)
    {
        // rb.bodyType = RigidbodyType2D.Dynamic;
        _rb.velocity = new Vector2(_rb.velocity.x, 0);
        _rb.AddForce(
            hitObjectParams.Direction*hitObjectParams.Knockback, ForceMode2D.Impulse
        );
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _detectRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _attackRadius);
    }
#endif

    
}
