using UnityEngine;

[System.Serializable]
public class HitObjectParams
{
	public float force;
	public float duration;
	public int damage;
	public Vector3 direction;
	public HitObjectParams(float force, float duration, int damage, Vector3 direction)
	{
		this.force = force;
		this.duration = duration;
		this.damage = damage;
		this.direction = direction;
	}
}
