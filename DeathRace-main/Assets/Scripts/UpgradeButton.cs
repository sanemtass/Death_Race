using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeButton : MonoBehaviour
{
    private GameObject currentGun;

    public void SetCurrentGun(GameObject gun)
    {
        currentGun = gun;
    }

    public void Upgrade()
    {
        if (currentGun != null)
        {
            var enemy = FindObjectOfType<Enemy>();
            if (enemy != null)
            {
                enemy.Upgrade(currentGun);
                Destroy(currentGun);
            }
        }

        // Upgrade işleminden sonra butonu kapat
        Destroy(gameObject);
    }

}