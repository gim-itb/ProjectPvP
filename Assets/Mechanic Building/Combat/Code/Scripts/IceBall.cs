using UnityEngine;
using System.Collections;
public class IceBall : MonoBehaviour
{
    public float FreezeDuration = 3;
    public float DurationUntilDeath = 3;
    [SerializeField] Transform _skinTrans;
    [SerializeField] TrailRenderer _trail;
    [SerializeField] ParticleSystem _iceParticle;
    [SerializeField] ParticleSystem _frozenParticle;
    Rigidbody2D _rb;
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        _skinTrans.right = _rb.velocity;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        DestroySelf();
        if(!col.attachedRigidbody.CompareTag("Entity"))return;
        EntityCore _otherCore = col.attachedRigidbody.GetComponent<EntityCore>();
        if (_otherCore != null)
        {
            _otherCore.OnHurt(new HitObjectParams(
                10, FreezeDuration, 0, _skinTrans.right, Element.Ice
            ));
        }
    }
    void DestroySelf()
    {
        ΩLul.Global.PlayFrozenHitParticle(transform.position);
        _iceParticle.Clear();
        _iceParticle.Stop(true);
        _trail.Clear();
        gameObject.SetActive(false);
        ΩLul.Global.IceMagicCore.ReleaseIceBall(this);
    }
    public void EmitTrail()
    {
        gameObject.SetActive(true);
        StartCoroutine(DestroySelfAfterTime(DurationUntilDeath));
        _iceParticle.Play(true);
    }

    ushort _key = 0;
    IEnumerator DestroySelfAfterTime(float time)
    {
        ushort requirement = ++_key;
        yield return new WaitForSeconds(time);
        if(requirement == _key)
        DestroySelf();
    }
}
