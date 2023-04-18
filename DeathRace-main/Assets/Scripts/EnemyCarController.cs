using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyCarController : MonoBehaviour
{
    [SerializeField] LayerMask layermask;
    [SerializeField] private float speed = 30f; // Yeni default hız değeri
    public float period = 0.0f;

    private void Update()
    {
        period += Time.deltaTime;

        if (period > 2f && period < 3f) // 2 saniye sonra yükselt
        {
            speed = 31f;
        }
        else if (period > 3f && period < 4f) // 3 saniye sonra düşür
        {
            speed = 29f;
        }
        else if (period > 4f) // 4 saniye sonra hızı default speed'e sıfırla
        {
            speed = 30f;
            period = 0f;
        }

        Movement();
    }

    private void Movement()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.right), out hit, Mathf.Infinity, layermask))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.right) * hit.distance, Color.blue);

            if (period > 9)
            {
                int durationTime = Random.Range(1, 3);

                transform.DOMoveX(1.5f, durationTime).OnComplete(() =>
                {
                    transform.DOMoveX(-1.5f, durationTime);
                });

                period = 0f;
            }
        }
        period += Time.deltaTime;
    }
}
