using UnityEngine;

[CreateAssetMenu(menuName = "Game/Gun/Create Gun",fileName = "Gun")]

public class Gun : ScriptableObject
{
    public GameObject gunPrefab;
    public GameObject bulletPrefab;
    public float fireRate;
    public Gun nextGun;
}

