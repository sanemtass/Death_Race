using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{
    private LevelGen gen;

    public List<GameObject> randomObjectPrefabs;
    public List<GameObject> randomObjectList = new List<GameObject>();

    public int numObjectsToSpawn;
    public float minX, maxX, minZ, maxZ;
    public float progressZ = 0f;

    private void Start()
    {
        gen = FindObjectOfType<LevelGen>();

        CreateRandomObjects();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Yeni yol olusturuldu.");
            gen.CreateRoad();

            if (gen.roadList.Count > 3)
            {
                gen.roadList[^3].SetActive(false);
            }
        }
    }

    public void CreateRandomObjects()
    {
        for (int i = 0; i < numObjectsToSpawn; i++)
        {
            int randomIndex = Random.Range(0, randomObjectPrefabs.Count);

            float posX = Random.Range(minX, maxX);
            float posZ = Random.Range(minZ, maxZ);

            Vector3 spawnPos = new Vector3(posX + 5, 0, progressZ);

            GameObject newObject = Instantiate(randomObjectPrefabs[randomIndex]);
            newObject.transform.parent = transform;
            newObject.transform.localPosition = spawnPos;
            randomObjectList.Add(newObject);
            progressZ += 4;
        }
    }
}
