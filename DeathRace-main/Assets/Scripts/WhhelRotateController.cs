using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhhelRotateController : MonoBehaviour
{
    private Vector3 currentPos;

    public float Speed;
   
    void FixedUpdate()
    {
        //currentPos.x += Speed;
        transform.Rotate(new Vector3(transform.localPosition.x * Speed, transform.localPosition.y, transform.localPosition.z));
    }
}
