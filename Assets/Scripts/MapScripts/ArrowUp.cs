using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowUp : MonoBehaviour
{
    private Vector3 initialPos;
    private Vector3 targetPos;

    public float moveDistance = 1f; // 이동 거리

    void Start()
    {
        initialPos = transform.localPosition; // 현재 위치 
        targetPos = initialPos + Vector3.up * moveDistance; // 목표 위치

        StartCoroutine(UpAnimation());
    }

    private IEnumerator UpAnimation()
    {
        while (true)
        {
            // 위로 이동
            transform.localPosition = targetPos;
            yield return new WaitForSeconds(0.8f);

            // 원위치로 돌아가기
            transform.localPosition = initialPos;
            yield return new WaitForSeconds(0.8f);
        }
    }
}
