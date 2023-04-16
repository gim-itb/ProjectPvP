using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
public class IceMagicCore : MonoBehaviour
{
    public float DurationToFull = 3;
    public float MaxMagic = 100;

    float _currentMagic = 100;
    public float CurrentMagic {get{return _currentMagic;} set{_currentMagic=Mathf.Clamp(value, 0, MaxMagic);}}
    public float CastCost = 10;
    [SerializeField] Transform staffTrans;
    [SerializeField] Transform spawnPointTrans;
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
        staffTrans.right = mousePos - (Vector2)staffTrans.position;
        if(Input.GetMouseButtonDown(0))
        {
            CastIce();
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
        return Instantiate(_iceBallRbPrefab, spawnPointTrans.position, staffTrans.rotation);
    }
    void OnReturnedToPool(IceBall iceBall)
    {
        iceBall.gameObject.SetActive(false);
    }
    void OnTakeFromPool(IceBall iceBall)
    {
        iceBall.gameObject.SetActive(true);
        iceBall.EmitTrail();
        iceBall.transform.position = spawnPointTrans.position;
        iceBall.transform.right = staffTrans.right;
        iceBall.GetComponent<Rigidbody2D>().AddForce(staffTrans.right * _castForce, ForceMode2D.Impulse);
    }
    void OnDestroyPoolObject(IceBall iceBall)
    {
        Destroy(iceBall.gameObject);
    }

    public void ReleaseIceBall(IceBall iceBall)
    {
        _iceBallPool.Release(iceBall);
    }
}
