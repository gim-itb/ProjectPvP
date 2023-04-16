using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatCore : EntityCore
{
    [SerializeField] Transform _skinTrans;
    [SerializeField] Transform _freezedSkinTrans;
    // [SerializeField] ParticleSystem _frozenParticle;
    bool _isFrozen = false;
    void FreezeSelf()
    {
        if(_isFrozen)return;

        ++_key;
        _isFrozen = true;
        // _frozenParticle.Play();
        _skinTrans.gameObject.SetActive(false);
        _freezedSkinTrans.gameObject.SetActive(true);

        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        ΩLul.Global.IceMagicCore.CurrentMagic += 30;
    }

    void UnFreezeSelf()
    {
        _isFrozen = false;
        // _frozenParticle.Play();
        ΩLul.Global.PlayFrozenHitParticle(transform.position);
        _skinTrans.gameObject.SetActive(true);
        _freezedSkinTrans.gameObject.SetActive(false);

        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        StartCoroutine(ToInitialTransform(_initialPosition, 1f));
    }

    public override void OnHurt(HitObjectParams hitObjectParams)
    {
        // _frozenParticle.Play();
        FreezeSelf();
        StartCoroutine(UnFreezeSelfAfterTime(hitObjectParams.duration));
    }

    IEnumerator UnFreezeSelfAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        UnFreezeSelf();
    }

    // For testing purpose
    Vector2 _initialPosition = new Vector2(0, 3);
    Quaternion _initialRotation = Quaternion.identity;
    void Start()
    {
        _initialPosition = transform.position;
        _initialRotation = transform.rotation;
    }
    byte _key = 0;
    IEnumerator ToInitialTransform(Vector2 height, float duration)
    {
        float t = 0;
        byte requirement = ++_key;
        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;
        while(t <= 1 && requirement == _key)
        {
            t += Time.deltaTime / duration;
            transform.position = Vector3.Lerp(startPosition, _initialPosition, Ease.InOutQuart(t));
            transform.rotation = Quaternion.Lerp(startRotation, _initialRotation, Ease.InOutQuart(t));
            yield return null;
        }
        if(requirement == _key)
        {
            transform.position = _initialPosition;
            transform.rotation = _initialRotation;
        }
        
    }
}
