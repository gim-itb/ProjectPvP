using UnityEngine;
using System.Collections;
public class Projectile : MonoBehaviour
{
    [SerializeField] protected float _lifeTime = 5;
    [SerializeField] public Element Element = Element.Normal;
    protected EntityCore _shooterCore;
    public static System.Action<Projectile> StaticOnProjectileHit;
    void OnTriggerEnter2D(Collider2D col)
    {
        OnDestroySelf();
        if(col.attachedRigidbody == null) return;
        if(!col.attachedRigidbody.CompareTag("Entity"))return;

        EntityCore _otherCore = col.attachedRigidbody.GetComponent<EntityCore>();
        if (_otherCore != null)
        {
            OnHit(_otherCore);
        }
    }
    public virtual void OnHit(EntityCore otherCore)
    {
        
    }
    public virtual void OnDestroySelf()
    {

    }
    public virtual void OnSpawn()
    {

    }

    public Projectile Shoot(EntityCore shooterCore, Vector2 spawnPosition, Vector2 velocity)
    {
        Projectile projectile = Instantiate(this, spawnPosition, Quaternion.identity);
        projectile._shooterCore = shooterCore;
        projectile.GetComponent<Rigidbody2D>().velocity = velocity;
        projectile.DestroySelfAfterTime(projectile._lifeTime);
        projectile.OnSpawn();
        return projectile;
    }
    public void DestroySelfAfterTime(float time)
    {
        StartCoroutine(IEDestroySelfAfterTime(time));
    }
    byte _key = 0;
    public IEnumerator IEDestroySelfAfterTime(float time)
    {
        byte requirement = ++_key;
        yield return new WaitForSeconds(time);
        if(requirement == _key)
        OnDestroySelf();
    }
}
