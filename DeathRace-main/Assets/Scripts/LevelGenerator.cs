using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject[] roadPieces;
    public GameObject[] randomObjects;
    public GameObject startRoad;
    public float roadPieceLength = 50f;
    public float obstacleSpawnInterval = 3f;
    public float roadBendAmount = 5f;
    public float roadWidthChangeAmount = 2f;
    public float obstacleDistanceFromRoad = 5f; // Obstacle'ların yola olan mesafesi
    public float obstacleSideDistance = 2f; // Obstacle'ların yolu hangi mesafede takip etmesi gerektiği

    private float timer;
    private bool spawnObstacle = true;
    private List<GameObject> spawnedObstacles;
    private GameObject currentRoad;

    void Start()
    {
        timer = 0f;
        spawnedObstacles = new List<GameObject>();
        currentRoad = startRoad;

        Vector3 currentPiecePosition = currentRoad.transform.position; //yol parçalarının birbirine bağlanması
        for (int i = 0; i < roadPieces.Length; i++)
        {
            GameObject newRoad = Instantiate(roadPieces[i], currentPiecePosition + Vector3.forward * roadPieceLength, Quaternion.identity);
            currentPiecePosition = newRoad.transform.position;
        }
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= obstacleSpawnInterval && spawnObstacle)
        {
            SpawnObstacle();
            timer = 0f;
        }

        if (currentRoad.transform.position.z < transform.position.z)
        {
            SpawnRoadPiece(); //ilerledikçe yol eklenmesi
        }
    }

    void SpawnObstacle()
    {
        GameObject randomObj = Instantiate(randomObjects[Random.Range(0, randomObjects.Length)], transform.position, Quaternion.identity);
        randomObj.transform.SetParent(transform);
        spawnedObstacles.Add(randomObj);

        Vector3 position = randomObj.transform.position;
        float sideDistance = Random.Range(-obstacleSideDistance, obstacleSideDistance); // Obstacle'ların yolu takip etmesi gereken mesafeyi rastgele belirleyin
        if (sideDistance > 0) // Eğer obstacle yolun sağında yer alıyorsa
        {
            position += new Vector3(sideDistance, 0, 0); // Obstacle'ı yolun sağında konumlandırın
        }
        else // Eğer obstacle yolun solunda yer alıyorsa
        {
            position += new Vector3(sideDistance, 0, -2 * obstacleDistanceFromRoad); // Obstacle'ı yolun solunda konumlandırın
        }
        randomObj.transform.position = position;

        Vector3 rotation = currentRoad.transform.rotation.eulerAngles;
        rotation += new Vector3(0, Random.Range(-roadBendAmount, roadBendAmount), 0);
        currentRoad.transform.rotation = Quaternion.Euler(rotation);
    }

    void SpawnRoadPiece()
    {
        GameObject newRoad = Instantiate(roadPieces[Random.Range(0, roadPieces.Length)], currentRoad.transform.position - Vector3.forward * roadPieceLength, Quaternion.identity);
        newRoad.transform.Rotate(0f, 180f, 0f); // Yolu tam tersi yönde çevirin
        currentRoad = newRoad;
    }
}