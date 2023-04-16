using UnityEngine;

public abstract class EntityCore : MonoBehaviour
{
    public abstract void OnHurt(HitObjectParams hitObjectParams);
}
