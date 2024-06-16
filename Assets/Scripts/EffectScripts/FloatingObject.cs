using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    [SerializeField] private float floatDistance = 2.0f; // 떠오르는 거리
    [SerializeField] private float floatSpeed = 1.4f; // 떠오르는 속도

    private Vector3 startPosition; // 초기 위치

    private void Start()
    {
        startPosition = transform.localPosition; // 초기 위치 저장
        StartCoroutine(FloatObject());
    }

    private IEnumerator FloatObject()
    {
        bool movingUp = true;

        while (true)
        {
            float newY = transform.localPosition.y + (movingUp ? floatSpeed : -floatSpeed) * Time.deltaTime;
            transform.localPosition = new Vector3(transform.localPosition.x, newY, transform.localPosition.z);

            if (movingUp && newY >= startPosition.y + floatDistance)
            {
                movingUp = false;
            }
            else if (!movingUp && newY <= startPosition.y)
            {
                movingUp = true;
            }

            yield return null; // 다음 프레임까지 대기
        }
    }
}