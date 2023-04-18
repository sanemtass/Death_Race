using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WheelController : MonoBehaviour
{
    public GameObject[] wheels;

    private float wheelSpeed = 1000f;
    private float suspensionDistance = 0.1f;
    private float suspensionDuration = 1f;

    private void Start()
    {
        foreach (GameObject wheel in wheels)
        {
            AnimateSuspension(wheel);
        }
    }

    private void Update()
    {
        foreach (GameObject wheel in wheels)
        {
            wheel.transform.Rotate(Vector3.right, wheelSpeed * Time.deltaTime);
        }
    }

    private void AnimateSuspension(GameObject wheel)
    {
        Vector3 initialPosition = wheel.transform.localPosition;
        Vector3 upPosition = initialPosition + new Vector3(0, suspensionDistance, 0);
        Vector3 downPosition = initialPosition - new Vector3(0, suspensionDistance, 0);

        wheel.transform.DOLocalMove(upPosition, suspensionDuration).SetEase(Ease.InOutSine).OnComplete(() =>
        {
            wheel.transform.DOLocalMove(downPosition, suspensionDuration).SetEase(Ease.InOutSine).OnComplete(() =>
            {
                wheel.transform.DOLocalMove(initialPosition, suspensionDuration).SetEase(Ease.InOutSine).OnComplete(() =>
                {
                    AnimateSuspension(wheel);
                });
            });
        });
    }
}
