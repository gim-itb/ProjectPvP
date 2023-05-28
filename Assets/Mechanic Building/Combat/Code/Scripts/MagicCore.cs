using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
public class MagicCore : EntityCore
{
    [SerializeField] MagicData _staticData;
    MagicData _data;
    public MagicData Data {get => _data;}
    [SerializeField] Transform _staffTrans;
    [SerializeField] Transform _spawnPoint;
    public Transform SpawnPoint {get => _spawnPoint;} 
    [SerializeField] IceBall _iceBall;

    public Action<MagicCore> OnMagicChanged;
    Action<MagicCore> OnIceBallHit;
    

    void Awake()
    {
        _data = Instantiate(_staticData);
        ΩLul.Global.MagicCore = this;
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
        _data.Magic += Time.deltaTime * _data.IceRefillRate * _data.MaxMagic;
        OnMagicChanged?.Invoke(this);
    }


    [Header("Ice Ball")]
    [SerializeField] ParticleSystem _castParticle;
    void CastIce()
    {
        if(_data.Magic < _data.IceCost) return;
        _castParticle.Play();
        _iceBall.Shoot(this, _spawnPoint.position, _staffTrans.right * _data.IceSpeed);
        _data.Magic -= _data.IceCost;
    }
    public void OnIceHit()
    {
        _data.Magic += _data.IceRefillOnHit;
    }


    [Header("Fire Wrath")]
    [SerializeField] float _fireMaxRange = 4;
    [SerializeField] float _fireMaxHeight = 5;
    [SerializeField] ParticleSystem _castFireParticle;
    [SerializeField] FireWrath _fireWrath;
    [SerializeField] LayerMask _groundLayer;

    void CastFire()
    {
        if(_data.Magic < _data.FireCost)
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
            _data.Magic -= _data.FireCost;
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
            _data.Magic -= _data.FireCost;
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
