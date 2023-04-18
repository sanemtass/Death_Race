using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGen : MonoBehaviour
{
    public GameObject roadPrefab;
    public List<GameObject> roadList;

    private void Start()
    {
        if (roadList.Count < 1)
        {
            CreateRoad();
        }
        
    }

    public void CreateRoad()
    {
        var newRoad = Instantiate(roadPrefab, transform);
        newRoad.transform.localPosition = new Vector3(0, 0, 500 * roadList.Count);
        roadList.Add(newRoad);
    }

}
