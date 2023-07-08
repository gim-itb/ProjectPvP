using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] float _upwardsForce = 200;
    Rigidbody2D _rb;
    void Awake() => _rb = GetComponent<Rigidbody2D>();
    void FixedUpdate() => _rb.AddForce(Vector2.up * _upwardsForce);
}
