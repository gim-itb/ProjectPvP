using UnityEngine;

[System.Serializable]
public class HitRequest
{
    public float Damage;
    public float Knockback = 0;
    public Vector3 Direction = Vector3.zero;
    public float StunDuration = 0;
    public Element Element = Element.Normal;

    public HitRequest(){}
    // Everything
    public HitRequest(float damage = 0, float knockback = 0, Vector3 direction = default, Element element = Element.Normal, float stunDuration = 1)
    {
        Damage = damage;
        Knockback = knockback;
        Direction = direction;
        Element = element;
        StunDuration = stunDuration;
    }
}
