using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
public class MagicCore : MonoBehaviour
{
    public float DurationToFull = 3;
    public float MaxMagic = 100;

    float _currentMagic = 100;
    public float CurrentMagic {get{return _currentMagic;} set{_currentMagic=Mathf.Clamp(value, 0, MaxMagic);}}
    public float CastCost = 10;
    [SerializeField] Transform _staffTrans;
    [SerializeField] Transform _spawnPointTrans;
    [SerializeField] ParticleSystem _castParticle;
    [SerializeField] ParticleSystem _frozenParticle;

    [SerializeField] float _castForce = 100;
    [SerializeField] IceBall _iceBallRbPrefab;

    void Awake()
    {
        ΩLul.Global.IceMagicCore = this;
        ΩLul.Global.FrozenHitEffect = _frozenParticle;
        _iceBallPool = new ObjectPool<IceBall>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, 
            OnDestroyPoolObject, false, 10, 50);
    }
    void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _staffTrans.right = mousePos - (Vector2)_staffTrans.position;
        if(Input.GetMouseButtonDown(0))
        {
            CastIce();
        }
        else if(Input.GetMouseButtonDown(1))
        {
            CastFire();
        }
        CurrentMagic = CurrentMagic + Time.deltaTime / DurationToFull * MaxMagic;
    }
    void CastIce()
    {
        if(CurrentMagic < CastCost)
        {
            return;
        }
        _castParticle.Play();
        CurrentMagic -= CastCost;
        _iceBallPool.Get();
    }

    // Object Pooling
    ObjectPool<IceBall> _iceBallPool;

    IceBall CreatePooledItem()
    {
        return Instantiate(_iceBallRbPrefab, _spawnPointTrans.position, _staffTrans.rotation);
    }
    void OnReturnedToPool(IceBall iceBall)
    {Debug.Log("Return" + iceBall.name);
        // iceBall.gameObject.SetActive(false);
    }
    void OnTakeFromPool(IceBall iceBall)
    {Debug.Log("Take" + iceBall.name);
        iceBall.gameObject.SetActive(true);
        iceBall.EmitTrail();
        iceBall.transform.position = _spawnPointTrans.position;
        iceBall.transform.right = _staffTrans.right;
        iceBall.GetComponent<Rigidbody2D>().AddForce(_staffTrans.right * _castForce, ForceMode2D.Impulse);
    }
    void OnDestroyPoolObject(IceBall iceBall)
    {Debug.Log("Destroy" + iceBall.name);
        Destroy(iceBall.gameObject);
    }

    public void ReleaseIceBall(IceBall iceBall)
    {
        _iceBallPool.Release(iceBall);
    }

    [Header("Fire Wrath")]
    [SerializeField] float _fireMaxRange = 4;
    [SerializeField] float _fireMaxHeight = 5;
    [SerializeField] ParticleSystem _castFireParticle;
    [SerializeField] FireWrath _fireWrath;
    [SerializeField] LayerMask _groundLayer;

    void CastFire()
    {
        if(CurrentMagic < CastCost)
        {
            return;
        }

        // Middle to right
        RaycastHit2D hit = Physics2D.Raycast(_staffTrans.position, _staffTrans.right, _fireMaxRange, _groundLayer);
        if(hit)
        {
            _castFireParticle.Play();
            _fireWrath.transform.position = hit.point;
            _fireWrath.OnFire();
            CurrentMagic -= CastCost;
            return;
        }

        // Right to down
        Vector3 rayDownwardsStartPos = _staffTrans.position + _staffTrans.right * _fireMaxRange;
        hit = Physics2D.Raycast(rayDownwardsStartPos, Vector3.down, _fireMaxHeight, _groundLayer);
        if(hit)
        {
            _castFireParticle.Play();
            _fireWrath.transform.position = hit.point;
            _fireWrath.OnFire();
            CurrentMagic -= CastCost;
            return;
        }
    }
    
    [Header("Debug")]
    float _opacity = 1f;
    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1,0,0,_opacity);

        RaycastHit2D hit = Physics2D.Raycast(_staffTrans.position, _staffTrans.right, _fireMaxRange, _groundLayer);
        if(hit)
        {
            Gizmos.DrawLine(_staffTrans.position, hit.point);
            Gizmos.DrawSphere(hit.point, 0.3f);
            return;
        }
        Vector3 rayDownwardsStartPos = _staffTrans.position + _staffTrans.right * _fireMaxRange;
        Gizmos.DrawLine(_staffTrans.position, rayDownwardsStartPos);

        hit = Physics2D.Raycast(rayDownwardsStartPos, Vector3.down, _fireMaxHeight, _groundLayer);
        if(hit)
        {
            Gizmos.DrawLine(rayDownwardsStartPos, hit.point);
            Gizmos.DrawSphere(hit.point, 0.3f);
            return;
        }
        Gizmos.DrawLine(rayDownwardsStartPos, rayDownwardsStartPos+Vector3.down*_fireMaxHeight);

    }
}
