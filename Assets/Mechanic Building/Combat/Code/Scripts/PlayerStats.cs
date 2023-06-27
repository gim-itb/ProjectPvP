using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerStats : MonoBehaviour
{
    public Action<PlayerStats> OnHealthChanged;

    [SerializeField] float _maxHealth = 100f;
    public float MaxHealth => _maxHealth;
    [ReadOnly] [SerializeField] float _currentHealth;
    public float CurrentHealth {
        get { return _currentHealth; }
        set { _currentHealth = Mathf.Clamp(value, 0, _maxHealth); }
    }

    void Awake()
    {
        _currentHealth = _maxHealth;
    }


    public void Hurt(HitRequest hitRequest, ref HitResult hitResult)
    {
        hitResult.Type = HitType.Entity;
        CurrentHealth -= hitRequest.Damage;
        if(_currentHealth <= 0)
        {
            hitResult.Defeat = true;
        }
        OnHealthChanged?.Invoke(this);
    }
}
