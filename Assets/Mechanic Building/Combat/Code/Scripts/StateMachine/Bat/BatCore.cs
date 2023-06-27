using UnityEngine;
using System.Collections;

public class BatCore : Core<BatCore, BatStates>
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

    [SerializeField] Vector2 _attackOffset;
    public Vector2 AttackOffset => _attackOffset;

    float _stunTimer = 1;
    public float StunTimer { get => _stunTimer; set => _stunTimer = value; }

    [Header("Others")]
    [SerializeField] Transform _skinTrans;
    [SerializeField] Transform _freezedSkinTrans;
    [HideInInspector] public LayerMask PlayerLayerMask = 1 << 0;
    [HideInInspector] public string PlayerTag = "Player";

    [HideInInspector] public Transform ChaseTarget;
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] SpriteRenderer _spriteRenderer;

    
    void Awake()
    {
        States = new BatStates(this);
        CurrentState = States.Idle();
        CurrentState.StateEnter();
    }

    void FixedUpdate()
    {
        CurrentState.StateFixedUpdate();
    }
    float _attackdelay = 0.15f;
    float _attackTimer = 0;
    public void Attack()
    {
        if(_attackTimer > 0)
        {
            _attackTimer -= Time.deltaTime;
            return;
        }
        _attackTimer = _attackdelay;
        
        Collider2D col = Physics2D.OverlapCircle((Vector2)transform.position + _attackOffset, _attackRadius, PlayerLayerMask);
        if(col == null || !col.CompareTag(PlayerTag)) return;

        PlayerStats playerStats = col.GetComponent<PlayerStats>();
        if(playerStats == null) return;

        HitRequest hitRequest = new HitRequest(
            damage: Damage
        );
        HitResult hitResult = new HitResult();

        playerStats.Hurt(hitRequest, ref hitResult);
    }
    public void Rotation()
    {
        _rb.angularVelocity = 0;
        _rb.rotation = 0;
    }

    public void Move()
    {
        Vector2 direction = (ChaseTarget.position - transform.position).normalized;
        if((ChaseTarget.position - transform.position).sqrMagnitude < 1.5f)
        {
            Stop();
            return;
        }
        _rb.velocity = direction * _speed;
        _spriteRenderer.flipX = direction.x < 0;
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
        Debug.Log("Why");
        _skinTrans.gameObject.SetActive(false);
        _freezedSkinTrans.gameObject.SetActive(true);

        // GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        _rb.gravityScale = 1;

        SwitchState(States.Freeze());
    }
    

    public void UnFreezeSelf()
    {
        _isFrozen = false;
        StaticOnEntityUnfreeze?.Invoke(this);
        _skinTrans.gameObject.SetActive(true);
        _freezedSkinTrans.gameObject.SetActive(false);


        // GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        _rb.gravityScale = 0;
    }

    void BurnSelf(HitRequest hitObjectParams)
    {
        // rb.bodyType = RigidbodyType2D.Dynamic;
        _rb.gravityScale = 1;
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
