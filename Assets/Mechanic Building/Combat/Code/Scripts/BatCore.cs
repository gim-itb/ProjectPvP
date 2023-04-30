using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatCore : EntityCore
{
    [SerializeField] Transform _skinTrans;
    [SerializeField] Transform _freezedSkinTrans;
    bool _isFrozen = false;
    void FreezeSelf(HitObjectParams hitObjectParams)
    {
        if(_isFrozen)
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.velocity = Vector2.zero;
            rb.AddForce(
                hitObjectParams.Direction*hitObjectParams.Force, ForceMode2D.Impulse
            );
            return;
        }
        GetComponent<Rigidbody2D>().AddForce(
                hitObjectParams.Direction*hitObjectParams.Force, ForceMode2D.Impulse
            );
        ++_key;
        _isFrozen = true;
        // _frozenParticle.Play();
        _skinTrans.gameObject.SetActive(false);
        _freezedSkinTrans.gameObject.SetActive(true);

        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        ΩLul.Global.IceMagicCore.CurrentMagic += 30;
        StartCoroutine(UnFreezeSelfAfterTime(hitObjectParams.Duration));
    }
    
    IEnumerator UnFreezeSelfAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        UnFreezeSelf();
    }
    void UnFreezeSelf()
    {
        _isFrozen = false;
        ΩLul.Global.PlayFrozenHitParticle(transform.position);
        _skinTrans.gameObject.SetActive(true);
        _freezedSkinTrans.gameObject.SetActive(false);

        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        StartCoroutine(ToInitialTransform(_initialPosition, 1f));
    }

    void BurnSelf(HitObjectParams hitObjectParams)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(
            hitObjectParams.Direction*hitObjectParams.Force, ForceMode2D.Impulse
        );
        ΩLul.Global.IceMagicCore.CurrentMagic += 5;
    }
    public override void OnHurt(HitObjectParams hitObjectParams)
    {
        if(hitObjectParams.Element == Element.Ice)
        {
            FreezeSelf(hitObjectParams);
        }
        else if(hitObjectParams.Element == Element.Fire)
        {
            BurnSelf(hitObjectParams);
        }
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
