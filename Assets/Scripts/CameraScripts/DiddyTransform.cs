using UnityEditor;
using UnityEngine;

public class DiddyTransform : MonoBehaviour
{
    private Vector3 offset;
    private bool dragging = false;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        // 마우스 입력 처리
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform == transform)
                {
                    dragging = true;
                    offset = hit.transform.position - GetMouseWorldPos();
                    print("DiddyTransform - 드래깅 시작");
                    DiddyEmotionRotate();
                }
            }
        }

        if (Input.GetMouseButton(0) && dragging)
        {
            Vector3 targetPosition = GetMouseWorldPos() + offset;
            transform.position = ClampPositionToScreen(targetPosition);
        }

        if (Input.GetMouseButtonUp(0) && dragging)
        {
            dragging = false;
            print("DiddyTransform - Dragging 끝");
        }

        // 터치 입력 처리
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Ray ray = mainCamera.ScreenPointToRay(touch.position);

            if (touch.phase == TouchPhase.Began)
            {
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform == transform)
                    {
                        dragging = true;
                        offset = hit.transform.position - GetTouchWorldPos(touch);
                    }
                }
            }

            if (touch.phase == TouchPhase.Moved && dragging)
            {
                Vector3 targetPosition = GetTouchWorldPos(touch) + offset;
                transform.position = ClampPositionToScreen(targetPosition);
            }

            if (touch.phase == TouchPhase.Ended && dragging)
            {
                dragging = false;
            }
        }
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = mainCamera.WorldToScreenPoint(transform.position).z;
        return mainCamera.ScreenToWorldPoint(mousePoint);
    }

    private Vector3 GetTouchWorldPos(Touch touch)
    {
        Vector3 touchPoint = touch.position;
        touchPoint.z = mainCamera.WorldToScreenPoint(transform.position).z;
        return mainCamera.ScreenToWorldPoint(touchPoint);
    }

    private Vector3 ClampPositionToScreen(Vector3 targetPosition)
    {
        Vector3 screenPos = mainCamera.WorldToScreenPoint(targetPosition);
        screenPos.x = Mathf.Clamp(screenPos.x, 0, Screen.width);
        screenPos.y = Mathf.Clamp(screenPos.y, 0, Screen.height);
        return mainCamera.ScreenToWorldPoint(screenPos);
    }

    public void DiddyEmotionRotate()
    {
        Animator animator = GetComponent<Animator>();
        int emotionIndex = animator.GetInteger("emotion");
        emotionIndex = (emotionIndex + 1) % 5;
        animator.SetInteger("emotion", emotionIndex);
        Debug.Log("Diddy의 Emotion을 rotate합니다: " + emotionIndex);
    }
}