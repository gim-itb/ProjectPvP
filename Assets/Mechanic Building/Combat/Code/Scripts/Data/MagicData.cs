using UnityEngine;

[CreateAssetMenu(fileName = "MagicData", menuName = "Data/Magic")]
public class MagicData : ScriptableObject
{
    // Constant
    public float MaxMagic = 100;
    public float IceCost = 10;
    public float IceRefillRate = 1f/3f; // 3 seconds until max
    public float IceRefillOnHit = 30;
    public float IceSpeed = 5;
    public float FireCost = 10;

    // Dynamic
    [HideInInspector] float _magic;
    [HideInInspector] public float Magic { get => _magic; set => _magic = Mathf.Clamp(value, 0, MaxMagic);}
}