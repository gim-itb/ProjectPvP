using UnityEngine;

[System.Serializable]
public class HitObjectParams
{
	public float Force;
	public float Duration;
	public int Damage;
	public Vector3 Direction;
	public Element Element;
	public HitObjectParams(float force, float duration, int damage, Vector3 direction, Element element)
	{
		this.Force = force;
		this.Duration = duration;
		this.Damage = damage;
		this.Direction = direction;
		this.Element = element;
	}
}
