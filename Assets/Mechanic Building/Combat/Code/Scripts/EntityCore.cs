using UnityEngine;

public abstract class EntityCore : MonoBehaviour
{
    public virtual void OnHurt(HitRequest hitRequest, ref HitResult hitResult){}
    public static System.Action<EntityCore> StaticOnEntityUnfreeze;
}
