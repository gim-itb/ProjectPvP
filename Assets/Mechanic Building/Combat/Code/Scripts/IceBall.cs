using UnityEngine;
using System.Collections;
public class IceBall : Projectile
{
    public float FreezeDuration = 3;
    [SerializeField] Transform _skinTrans;
    [SerializeField] Rigidbody2D _rb;

    public override void OnSpawn()
    {
        transform.right = _rb.velocity;
    }
    public override void OnHit(EntityCore otherCore)
    {
        HitResult hitResult = new HitResult();
        otherCore.OnHurt(new HitRequest(
            damage: 10,
            knockback: 0,
            duration: FreezeDuration,
            direction: _skinTrans.right,
            element: Element.Ice 
        ), ref hitResult);
        if(hitResult.Type == HitType.Entity)
        {
            ((MagicCore)_shooterCore).OnIceHit();
        }
        
    }
    public override void OnDestroySelf()
    {
        // gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
