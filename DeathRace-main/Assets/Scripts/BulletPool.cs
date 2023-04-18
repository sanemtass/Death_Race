using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UmbrellaUtils.Pool;

public class BulletPool : MonoBehaviour, IResettable
{
    public void Reset()
    {
        gameObject.SetActive(false);
    }
}
