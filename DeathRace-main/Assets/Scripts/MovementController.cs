using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MovementController : MonoBehaviour
{
    [SerializeField] LayerMask layermask;

    public float period = 0.0f;

    [SerializeField] private float speed = 29f;

    private void Update()
    {
        Movement();
    }

    private void Movement()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.left), out hit, Mathf.Infinity, layermask))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.left) * hit.distance, Color.red);

            if (period > 7)
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
