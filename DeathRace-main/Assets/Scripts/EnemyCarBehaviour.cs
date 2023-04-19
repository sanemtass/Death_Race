using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using SencanUtils.Utils.Extensions;
using UnityEngine.UI;

public class EnemyCarBehaviour : MonoBehaviour, IDamagable
{
    public Car enemyCar;
    public int health;
    public int maxHealth;

    [SerializeField] private bool isArmorBroken;
    [SerializeField] private bool istakeDamage;
    [SerializeField] private List<GameObject> fallingObjects;
    [SerializeField] private List<Transform> particlePoints; // Transform listesi ekle
    [SerializeField] private ParticleSystem damageParticlePrefab; // Damage particle prefab ekle

    private bool healthBarUpdated = false;

    private GameManager manager;
    private CarManager carManager;
    private UIManager uIManager;
    private int baseMaxHealth;

    private int[] healthThresholds;
    private int currentThreshold;

    private void Awake()
    {
        manager = FindObjectOfType<GameManager>();
        carManager = FindObjectOfType<CarManager>();
        uIManager = FindObjectOfType<UIManager>();
        baseMaxHealth = enemyCar.maxHealth;
    }

    private void Start()
    {
        Initialized();

        healthThresholds = new int[] { maxHealth * 3 / 4, maxHealth / 2, maxHealth / 4 };
        currentThreshold = 0;
    }

    void Update()
    {
        if (!healthBarUpdated)
        {
            uIManager.UpdateHealthBarEnemy();
            healthBarUpdated = true;
        }
    }

    public void Die()
    {
        //burada olme islemi
        //particle girecek

        Destroy(gameObject);

        carManager.Respawn(); // Yeni aracı oluşturmak için zamanla çağır
    }

    public void TakeDamage(int value)
    {
        istakeDamage = true;

        if (value < health)
        {
            health -= value;

            if (fallingObjects.Count > 0)
            {
                var fallingObj = fallingObjects.GetRandom();
                fallingObj.AddComponent<Rigidbody>().AddForce(Vector3.up * 3, ForceMode.Impulse);
                fallingObj.transform.parent = null;
                fallingObjects.Remove(fallingObj);
            }
            uIManager.UpdateHealthBarEnemy(); // Hasar alındığında UI güncellemesi

            // Health belirli bir eşikten azaldığında partikül sistemi başlat
            if (currentThreshold < healthThresholds.Length && health <= healthThresholds[currentThreshold])
            {
                var particleInstance = Instantiate(damageParticlePrefab, particlePoints[currentThreshold].position, Quaternion.identity, particlePoints[currentThreshold]);
                particleInstance.Play();
                currentThreshold++;
            }
        }

        else
        {
            health = 0;
            Die();
        }
    }


    private void Initialized()
    {
        health = maxHealth;
        maxHealth = baseMaxHealth;
    }
}
