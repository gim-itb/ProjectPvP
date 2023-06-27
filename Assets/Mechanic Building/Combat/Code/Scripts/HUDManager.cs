using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    [SerializeField] HealthBar _healthBar;
    [SerializeField] TransitionAnimation _transitionAnimation;
    PlayerStats _playerStats;
    void Awake()
    {
        Time.timeScale = 1f;
        _playerStats = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();
    }
    void OnEnable()
    {
        _playerStats.OnHealthChanged += OnHealthChanged;
    }

    void OnHealthChanged(PlayerStats stats)
    {
        _healthBar.SetValue(stats.CurrentHealth/stats.MaxHealth);
        if(stats.CurrentHealth <= 0)
        {
            Time.timeScale = 0.3f;
            Invoke(nameof(Restart), 0.5f);
        }
    }

    void Restart()
    {
        Time.timeScale = 1f;
        _transitionAnimation.StartSceneTransition(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}
