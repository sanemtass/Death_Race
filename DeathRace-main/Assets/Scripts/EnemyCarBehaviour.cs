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
    public int armor;
    public int maxArmor;

    [SerializeField] private bool isArmorBroken;
    [SerializeField] private bool istakeDamage;
    [SerializeField] private List<GameObject> fallingObjects;

    private GameManager manager;
    private CarManager carManager;
    private UIManager uIManager;

    private void Awake()
    {
        manager = FindObjectOfType<GameManager>();
        carManager = FindObjectOfType<CarManager>();
        uIManager = FindObjectOfType<UIManager>();
    }

    private void Start()
    {
        Initialized();
        RegenerateArmor();
        uIManager.UpdateHealthBarEnemy(); //? emin degilim
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.W))
        {
            TakeDamage(10);
            //UpdateHealthBar();
        }
    }

    

    public void Die()
    {
        //burada olme islemi
        //particle girecek

        gameObject.SetActive(false);

        carManager.Respawn(); // Yeni aracı oluşturmak için zamanla çağır
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
                if (fallingObjects.Count > 0)
                {
                    var fallingObj = fallingObjects.GetRandom();
                    fallingObj.AddComponent<Rigidbody>().AddForce(Vector3.up * 3, ForceMode.Impulse);
                    fallingObj.transform.parent = null;
                    fallingObjects.Remove(fallingObj);
                    uIManager.UpdateHealthBarEnemy();
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
        armor = enemyCar.armor;
        maxArmor = enemyCar.maxArmor;
    }

}


