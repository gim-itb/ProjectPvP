using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class FireWrath: MonoBehaviour
{
    [SerializeField] float _force = 10;
    [SerializeField] float _width = 4;
    [SerializeField] float _height = 10;
    [SerializeField] float _delay = 1f;
    
    [SerializeField] ParticleSystem _fireParticle;
    [SerializeField] LayerMask _enemyLayer;

    IEnumerator DelayFire()
    {
        yield return new WaitForSeconds(_delay);
        OnFire();
    }
    public void OnFire()
    {
        _fireParticle.Play();
        RaycastHit2D[] hit;
        hit = Physics2D.BoxCastAll(transform.position, new Vector2(_width, 1), 0, Vector2.up, _height, _enemyLayer);
        if(hit.Length > 0)
        {
            foreach(RaycastHit2D h in hit)
            {
                EntityCore other = h.collider.attachedRigidbody.GetComponent<EntityCore>();
                other.OnHurt(new HitObjectParams(
                    _force, 0, 10, Vector2.up, Element.Fire
                ));
            }
        }
    }


    [Header("Debug")]
    [SerializeField] float _opacity = 0.3f;
    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1,0,0,_opacity);
        Gizmos.DrawCube(transform.position+Vector3.up*_height/2, new Vector3(_width, _height, 0));
    }
}