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
        // 마우스 및 터치 입력 처리
        if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Input.touchCount > 0)
                ray = mainCamera.ScreenPointToRay(Input.GetTouch(0).position);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform == transform)
                {
                    dragging = true;
                    offset = hit.transform.position - GetInputWorldPos();
                    DiddyEmotionRotate();
                }
            }
        }

        if ((Input.GetMouseButton(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)) && dragging)
        {
            Vector3 targetPosition = GetInputWorldPos() + offset;
            transform.position = ClampPositionToScreen(targetPosition);
        }

        if ((Input.GetMouseButtonUp(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)) && dragging)
        {
            dragging = false;
        }
    }

    private Vector3 GetInputWorldPos()
    {
        Vector3 inputPoint = Input.mousePosition;
        if (Input.touchCount > 0)
            inputPoint = Input.GetTouch(0).position;

        inputPoint.z = mainCamera.WorldToScreenPoint(transform.position).z;
        return mainCamera.ScreenToWorldPoint(inputPoint);
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