using System.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class CameraSwitch : MonoBehaviour
{
    [SerializeField] private ARCameraManager arCameraManager;

    void Start()
    {
        arCameraManager = FindObjectOfType<ARCameraManager>(); // Canvas에서 컴포넌트 가져오기
    }

    // 전면/후면 카메라 전환 메서드
    public void SwitchCamera()
    {
        StartCoroutine(SwitchCameraDirection());
    }

    IEnumerator SwitchCameraDirection()
    {
        yield return null;
        yield return null;

        // 현재 카메라 방향 확인
        CameraFacingDirection currentFacingDirection = arCameraManager.requestedFacingDirection;

        // 요청된 방향을 반대로 변경
        if (currentFacingDirection == CameraFacingDirection.World)
        {
            Debug.Log("전면 카메라로 전환합니다.");
            arCameraManager.requestedFacingDirection = CameraFacingDirection.User;
        }
        else
        {
            Debug.Log("후면 카메라로 전환합니다.");
            arCameraManager.requestedFacingDirection = CameraFacingDirection.World;
        }
    }
}