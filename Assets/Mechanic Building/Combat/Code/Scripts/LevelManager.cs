using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Header("Global VFX")]
    [SerializeField] ParticleSystem _frozenVFX;

    void OnEnable()
    {
        Projectile.StaticOnProjectileHit += OnProjectileHit;
        EntityCore.StaticOnEntityUnfreeze += OnEntityUnfreeze;
        _winTrigger.OnEnter += OnWinTriggerEnter;
        _loseTrigger.OnEnter += OnLoseTriggerEnter;
    }

    void OnDisable()
    {
        Projectile.StaticOnProjectileHit -= OnProjectileHit;
        EntityCore.StaticOnEntityUnfreeze -= OnEntityUnfreeze;
        _winTrigger.OnEnter -= OnWinTriggerEnter;
        _loseTrigger.OnEnter -= OnLoseTriggerEnter;
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

    [SerializeField] TransitionAnimation _transition;
    [SerializeField] string _nextSceneName;
    [SerializeField] TriggerEvent _winTrigger, _loseTrigger;
    void OnWinTriggerEnter()
    {
        _transition.StartSceneTransition(_nextSceneName);
    }
    void OnLoseTriggerEnter()
    {
        _transition.StartSceneTransition(SceneManager.GetActiveScene().name);
    }
}