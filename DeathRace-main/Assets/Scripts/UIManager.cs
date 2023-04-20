using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    public static event Action onBuyTurret;
    public static event Action onAddSlot;
    public static event Action onAddHealth;
    public static event Action onMerged;

    public GameObject upgradeButtonPrefab;
    public GameObject activeUpgradeButton;

    [SerializeField] private Slider healthBarSliderPlayer;
    [SerializeField] private Slider healthBarSliderEnemy;

    [SerializeField] private Slider armorBarSliderPlayer;

    private EnemyCarBehaviour enemyCarBehaviour;
    private CarBehavior carBehavior;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        enemyCarBehaviour = FindObjectOfType<EnemyCarBehaviour>();
        carBehavior = FindObjectOfType<CarBehavior>();
    }

    public void BuyTurret()
    {
        if (gameManager.gold.count > gameManager.buyTurret.cost)
        {
            gameManager.gold.count -= gameManager.buyTurret.cost;
            gameManager.buyTurret.LevelUp(); //butonun maliyeti artiyor, silahların pahalaniyo yani
            onBuyTurret?.Invoke();
        }

    }

    public void AddSlot()
    {
        if (gameManager.gold.count > gameManager.addSlot.cost)
        {
            gameManager.gold.count -= gameManager.addSlot.cost;
            gameManager.addSlot.LevelUp();
            onAddSlot?.Invoke();

        }
    }
    public void AddHealth()
    {
        if (gameManager.gold.count > gameManager.addHealth.cost)
        {
            gameManager.gold.count -= gameManager.addHealth.cost;
            gameManager.addHealth.LevelUp();
            onAddHealth?.Invoke();
            UpdateHealthBarPlayer(); // UI güncellemesi için bu satırı ekleyin
        }
    }

    public void UpdateHealthBarPlayer()
    {
        float healthPercentage = (float)carBehavior.health / carBehavior.maxHealth;
        healthBarSliderPlayer.value = healthPercentage;

        float armorPercentage = (float)carBehavior.armor / carBehavior.maxArmor;
        armorBarSliderPlayer.value = armorPercentage;
    }

    public void UpdateHealthBarEnemy()
    {
        float healthPercentage = (float)enemyCarBehaviour.health / enemyCarBehaviour.maxHealth;
        healthBarSliderEnemy.value = healthPercentage;
    }

    public void CreateUpgradeButton(GameObject gun)
    {
        GunBehaviour gunBehaviour = gun?.GetComponent<GunBehaviour>();

        if (gunBehaviour != null && gunBehaviour.nextGun != null)
        {
            if (activeUpgradeButton == null)
            {
                activeUpgradeButton = Instantiate(upgradeButtonPrefab, new Vector3(Screen.width / 3, Screen.height / 3, 0), Quaternion.identity);
                activeUpgradeButton.transform.SetParent(GameObject.Find("Canvas").transform, false);
            }

            activeUpgradeButton.GetComponent<UpgradeButton>().SetCurrentGun(gun);
            activeUpgradeButton.SetActive(true);
        }
        else
        {
            if (activeUpgradeButton != null)
            {
                activeUpgradeButton.SetActive(false);
            }
        }
    }

    public void UpdateEnemyHealthBarOnRespawn(EnemyCarBehaviour newEnemyCar)
    {
        enemyCarBehaviour = newEnemyCar;
        UpdateHealthBarEnemy();
    }
}
