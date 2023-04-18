using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelController : MonoBehaviour
{
    public GameObject[] wheels; 

    private float wheelSpeed = 1000f; 

    private void Update()
    {
        foreach (GameObject wheel in wheels)
        {
            wheel.transform.Rotate(Vector3.right, wheelSpeed * Time.deltaTime);
        }
    }
}
