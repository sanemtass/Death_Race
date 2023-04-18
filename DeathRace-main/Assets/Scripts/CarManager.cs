using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class CarManager : MonoBehaviour
{
    public Car enemyCar;
    public Transform playerCar; // Oyuncu aracının referansı
    public float respawnDistance = 50f; // Düşmanın oyuncudan ne kadar uzakta oluşacağını belirleyin (ör. 50 birim)

    public async UniTask Respawn()
    {
        gameObject.SetActive(false);
        await UniTask.Delay(3000);

        // Oyuncu aracına göre düşman aracının oluşacağı konumu hesaplayın
        Vector3 spawnPosition = playerCar.position - playerCar.forward * respawnDistance;

        // Düşman aracını hesaplanan konumda oluşturun
        Instantiate(enemyCar.carPrefab, spawnPosition, playerCar.rotation);
    }
}
