using UnityEngine;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
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

    [SerializeField] Vector2 _attackOffset;
    public Vector2 AttackOffset => _attackOffset;

    [SerializeField] float freezeResistance = 0.5f;
    float freezeTrauma = 0;
    
    [SerializeField] float _height;
    public float Height => _height;

    float _stunTimer = 1;
    public float StunTimer { get => _stunTimer; set => _stunTimer = value; }

    [Header("Others")]
    [SerializeField] Transform _skinTrans;
    [SerializeField] Transform _freezedSkinTrans;
    [SerializeField] SpriteRenderer _freezeTraumaBar;
    [HideInInspector] public LayerMask PlayerLayerMask = 1 << 0;
    [HideInInspector] public string PlayerTag = "Player";

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
        
        Collider2D col = Physics2D.OverlapCapsule(
            (Vector2)transform.position + _attackOffset, new Vector2(_attackRadius, _height),
            CapsuleDirection2D.Vertical, 0, PlayerLayerMask
        );

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

        freezeTrauma += 1 - freezeResistance;
        if (freezeTrauma > 0.999f){
            _skinTrans.gameObject.SetActive(false);
            _freezedSkinTrans.gameObject.SetActive(true);

            // GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

            SwitchState(States.Freeze());
        } else {
            print(freezeTrauma);
            _freezeTraumaBar.material.SetFloat("_Progress", freezeTrauma);
        }
    }

    public void UnFreezeSelf()
    {
        _isFrozen = false;
        StaticOnEntityUnfreeze?.Invoke(this);
        _skinTrans.gameObject.SetActive(true);
        _freezedSkinTrans.gameObject.SetActive(false);

        freezeTrauma = 0;
        _freezeTraumaBar.material.SetFloat("_Progress", freezeTrauma);

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
        // Gizmos.DrawWireSphere(_attackOffset+(Vector2)transform.position, _attackRadius);
        DrawWireCapsule(_attackOffset+(Vector2)transform.position, Quaternion.identity, _attackRadius/2, _height, Color.blue);
    }

    public void DrawWireCapsule(Vector3 _pos, Quaternion _rot, float _radius, float _height, Color _color = default(Color))
    {
        if (_color != default(Color))
            Handles.color = _color;
        Matrix4x4 angleMatrix = Matrix4x4.TRS(_pos, _rot, Handles.matrix.lossyScale);
        using (new Handles.DrawingScope(angleMatrix))
        {
            var pointOffset = (_height - (_radius * 2)) / 2;

            //draw sideways
            Handles.DrawWireArc(Vector3.up * pointOffset, Vector3.left, Vector3.back, -180, _radius);
            Handles.DrawLine(new Vector3(0, pointOffset, -_radius), new Vector3(0, -pointOffset, -_radius));
            Handles.DrawLine(new Vector3(0, pointOffset, _radius), new Vector3(0, -pointOffset, _radius));
            Handles.DrawWireArc(Vector3.down * pointOffset, Vector3.left, Vector3.back, 180, _radius);
            //draw frontways
            Handles.DrawWireArc(Vector3.up * pointOffset, Vector3.back, Vector3.left, 180, _radius);
            Handles.DrawLine(new Vector3(-_radius, pointOffset, 0), new Vector3(-_radius, -pointOffset, 0));
            Handles.DrawLine(new Vector3(_radius, pointOffset, 0), new Vector3(_radius, -pointOffset, 0));
            Handles.DrawWireArc(Vector3.down * pointOffset, Vector3.back, Vector3.left, -180, _radius);
            //draw center
            Handles.DrawWireDisc(Vector3.up * pointOffset, Vector3.up, _radius);
            Handles.DrawWireDisc(Vector3.down * pointOffset, Vector3.up, _radius);

        }
    }
#endif
}

