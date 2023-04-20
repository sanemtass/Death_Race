using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class CarManager : MonoBehaviour
{
    public List<Car> enemyCarList;

    public Transform playerCar; // Oyuncu aracının referansı
    public float respawnDistance = 20f; // Düşmanın oyuncudan ne kadar uzakta oluşacağını belirleyin (ör. 50 birim)

    private bool isRespawning = false;

    public async UniTask Respawn()
    {
        if (EnemyCarBehaviour.EnemyCarCount > 0 || isRespawning) return;

        isRespawning = true;
        await UniTask.Delay(3000);

        // Oyuncu aracına göre düşman aracının oluşacağı konumu hesaplayın
        Vector3 spawnPosition = playerCar.position - playerCar.forward * respawnDistance;

        // Düşman aracını oyuncu aracının solunda olacak şekilde hesaplayın
        spawnPosition -= playerCar.right * 8;

        // Listedeki rastgele bir düşman arabasını seçin
        Car randomEnemyCar = enemyCarList[Random.Range(0, enemyCarList.Count)];

        GameObject newEnemyCar = Instantiate(randomEnemyCar.carPrefab, spawnPosition, playerCar.rotation);

        // UIManager sınıfında düşman arabasının sağlık çubuğunu güncellemek için yeni bir fonksiyon çağırın
        FindObjectOfType<UIManager>().UpdateEnemyHealthBarOnRespawn(newEnemyCar.GetComponent<EnemyCarBehaviour>());

        isRespawning = false;
    }
}
