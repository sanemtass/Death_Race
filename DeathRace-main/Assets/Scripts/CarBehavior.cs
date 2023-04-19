using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using SencanUtils.Utils.Extensions;
using UnityEngine.UI;

public class CarBehavior : MonoBehaviour, IDamagable
{
    public Car car;
    public int health;
    public int maxHealth;
    public int armor;
    public int maxArmor;

    private int baseMaxHealth;
    private float lastDamageTime;

    [SerializeField] private bool istakeDamage;
    [SerializeField] private List<GameObject> fallingObjects;

    private bool healthBarUpdated = false;

    private GameManager manager;
    private UIManager uIManager;

    [SerializeField] private List<Transform> particlePoints; // Transform listesi ekle
    [SerializeField] private ParticleSystem damageParticlePrefab; // Damage particle prefab ekle

    private int[] healthThresholds;
    private int currentThreshold;

    [SerializeField] private List<GameObject> carMeshes;
    private int activeCarMeshIndex;
    private GameObject activeCarMesh;


    private void OnEnable()
    {
        UIManager.onAddHealth += InitHealth;
    }
    private void OnDestroy()
    {
        UIManager.onAddHealth -= InitHealth;
    }

    private void Awake()
    {
        manager = FindObjectOfType<GameManager>();
        uIManager = FindObjectOfType<UIManager>();
        baseMaxHealth = car.maxHealth;
    }

    private void Start()
    {
        InitHealth();
        Initialized();
        RegenerateArmor();

        healthThresholds = new int[] { maxHealth * 3 / 4, maxHealth / 2, maxHealth / 4 };
        currentThreshold = 0;
    }

    void Update()
    {
        if (!healthBarUpdated)
        {
            uIManager.UpdateHealthBarPlayer();
            healthBarUpdated = true;
        }
    }

    public void Die()
    {
        // Burada ölme işlemi
    }

    public void TakeDamage(int value)
    {
        lastDamageTime = Time.time;
        istakeDamage = true;

        if (armor > 0)
        {
            int remainingDamage = Mathf.Max(value - armor, 0);
            armor = Mathf.Max(armor - value, 0);
            value = remainingDamage;
        }

        if (value > 0)
        {
            health -= value;
            health = Mathf.Max(health, 0);

            // Health belirli bir eşikten azaldığında partikül sistemi başlat
            if (currentThreshold < healthThresholds.Length && health <= healthThresholds[currentThreshold])
            {
                var particleInstance = Instantiate(damageParticlePrefab, particlePoints[currentThreshold].position, Quaternion.identity);
                particleInstance.Play();
                currentThreshold++;
            }

            if (fallingObjects.Count > 0)
            {
                var fallingObj = fallingObjects.GetRandom();
                fallingObj.AddComponent<Rigidbody>().AddForce(Vector3.up * 3, ForceMode.Impulse);
                fallingObj.transform.parent = null;
                fallingObjects.Remove(fallingObj);
            }

            if (health == 0)
            {
                Die();
            }
        }

        uIManager.UpdateHealthBarPlayer();
    }

    private async void RegenerateArmor()
    {
        while (true)
        {
            await UniTask.Delay(1000);

            if (Time.time - lastDamageTime >= 3.0f)
            {
                armor = maxArmor;
                uIManager.UpdateHealthBarPlayer();
            }
        }
    }

   


    private void Initialized()
    {
        health = maxHealth;
        armor = car.armor;
        maxArmor = car.maxArmor;
    }

    public void InitHealth()
    {
        maxHealth = baseMaxHealth;
        baseMaxHealth = (int)(baseMaxHealth * manager.addHealth.increaseCost);
        health += (int)(baseMaxHealth * manager.addHealth.increaseCost); // Health'i arttır
        health = Mathf.Min(health, maxHealth); // Health'in max değerini aşmamasını sağla

        activeCarMeshIndex++;

        if (activeCarMeshIndex >= 4 && activeCarMeshIndex <= 6)
        {
            carMeshes[1].SetActive(true);
            carMeshes[0].SetActive(false);
        }

        else if (activeCarMeshIndex > 6)
        {
            carMeshes[1].SetActive(false);
            carMeshes[0].SetActive(false);
            carMeshes[2].SetActive(true);
        }
    }

}