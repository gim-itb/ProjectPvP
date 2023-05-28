using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Global VFX")]
    [SerializeField] ParticleSystem _frozenVFX;

    void OnEnable()
    {
        Projectile.StaticOnProjectileHit += OnProjectileHit;
        EntityCore.StaticOnEntityUnfreeze += OnEntityUnfreeze;
    }

    void OnDisable()
    {
        Projectile.StaticOnProjectileHit -= OnProjectileHit;
        EntityCore.StaticOnEntityUnfreeze -= OnEntityUnfreeze;
    }

    void OnProjectileHit(Projectile projectile)
    {
        if(projectile.Element == Element.Ice)
        {
            _frozenVFX.transform.position = projectile.transform.position;
            _frozenVFX.Play();
        }
    }
    void OnEntityUnfreeze(EntityCore core)
    {
        _frozenVFX.transform.position = core.transform.position;
        _frozenVFX.Play();
    }
}