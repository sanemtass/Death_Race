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

    [SerializeField] private bool isArmorBroken;
    [SerializeField] private bool istakeDamage;
    [SerializeField] private List<GameObject> fallingObjects;

    private GameManager manager;
    private UIManager uIManager;


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
    }


    private void Start()
    {
        InitHealth();
        Initialized();
        RegenerateArmor();
        uIManager.UpdateHealthBarPlayer(); //? emin degilim
    }

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(10);
            //UpdateHealthBar();
        }

    }


    public void Die()
    {
        //burada olme islemi
    }

    public void TakeDamage(int value)
    {
        istakeDamage = true;

        if (!isArmorBroken)
        {
            if (value <= armor)
            {
                armor -= value;
            }

            else
            {
                isArmorBroken = true;
                armor = 0;
            }
        }

        else
        {
            if (value < health)
            {
                health -= value;
                if (fallingObjects.Count>0)
                {
                    var fallingObj = fallingObjects.GetRandom();
                    fallingObj.AddComponent<Rigidbody>().AddForce(Vector3.up * 3,ForceMode.Impulse);
                    fallingObj.transform.parent = null;
                    fallingObjects.Remove(fallingObj);
                    uIManager.UpdateHealthBarPlayer();
                }
            }

            else
            {
                health = 0;
                Die();  
            }
        }
    }

    private async void RegenerateArmor()
    {
        while (!isArmorBroken)
        {
            await UniTask.Delay(3000);
            istakeDamage = false;
            if (!istakeDamage)
            {
                if (armor <= maxArmor)
                {
                    Debug.Log("here");
                    armor += 3;
                }
                else
                {
                    armor = maxArmor;
                }
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
        maxHealth = car.maxHealth;
        car.maxHealth = (int)(car.maxHealth * manager.addHealth.increaseCost);
    }
}


