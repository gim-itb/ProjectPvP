using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TriggerEvent : MonoBehaviour
{
    public System.Action OnEnter;
    [SerializeField] string _tag = "Player";
    bool _isTriggered = false;
    void OnTriggerEnter2D(Collider2D col)
    {
        if(_isTriggered) return;
        if(col.attachedRigidbody != null && col.attachedRigidbody.CompareTag(_tag))
        {
            _isTriggered = true;
            OnEnter?.Invoke();
        }
    }
}
