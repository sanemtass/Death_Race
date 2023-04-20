using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Car", menuName = "Plus Umbrella/Cars/Create Car", order = 0)]
public class Car : ScriptableObject
{
    public GameObject carPrefab;
    public int health;
    public int maxHealth;
    public int armor;
    public int maxArmor;
}
