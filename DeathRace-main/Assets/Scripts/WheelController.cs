using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WheelController : MonoBehaviour
{
    public GameObject[] wheels;


    private float wheelSpeed = 1000f;
    //private float suspensionDistance = 0.1f;
    //private float suspensionDuration = 1f;

    private void Start()
    {
        //foreach (GameObject wheel in wheels)
        //{
        //    AnimateSuspension(wheel);
        //}
    }

    private void Update()
    {
        foreach (GameObject wheel in wheels)
        {
            // Tekerleklerin yerel dönme ekseni etrafında döndürülmesini sağlayın
            wheel.transform.Rotate(wheel.transform.right, wheelSpeed * Time.deltaTime, Space.World);
        }
    }

    //private void AnimateSuspension(GameObject wheel)
    //{
    //    if (wheel == null) return;

    //    Vector3 initialPosition = wheel.transform.localPosition;
    //    Vector3 upPosition = initialPosition + new Vector3(0, suspensionDistance, 0);
    //    Vector3 downPosition = initialPosition - new Vector3(0, suspensionDistance, 0);

    //    Sequence sequence = DOTween.Sequence();
    //    sequence.Append(wheel.transform.DOLocalMove(upPosition, suspensionDuration).SetEase(Ease.InOutSine))
    //            .Append(wheel.transform.DOLocalMove(downPosition, suspensionDuration).SetEase(Ease.InOutSine))
    //            .Append(wheel.transform.DOLocalMove(initialPosition, suspensionDuration).SetEase(Ease.InOutSine))
    //            .SetLoops(-1, LoopType.Restart);
    //}


}
