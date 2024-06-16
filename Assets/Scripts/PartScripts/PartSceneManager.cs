using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.SceneManagement;

[System.Serializable]
public struct PartTransformInfo
{
    public string key;
    public Vector3 value;
}

public class PartSceneManager : MonoBehaviour
{
    // 팝업 및 UI 관련 매니저 스크립트
    private PopupManager popupManager;

    // 힌트 아이템 매니저 스크립트
    private HintManager hintManager;

    // 부품 생성 관련 ray info
    private ARRaycastManager arRaycastManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    // 부품 줍기 관련 ray info
    private RaycastHit hitInfoPart;
    private int partCnt = 0;

    private int totalPartCnt = 0; // 부품 전체 개수. 씬마다 다름. GPS 값마다 부품 생성값에 따라 계산
    private readonly float screenBiasWidth = 1440f;
    private readonly float screenBiasHeigth = 2560f;
    private readonly List<Vector2> createdPos = new List<Vector2>() { new Vector2(650f, 1300f), new Vector2(650f, 1600f), new Vector2(650f, 1900f) }; // (1440, 2560) 기준 좌표
    private readonly float partRadius = 1.1f; // 부품 특정 반경 내에서만 주울 수 있도록 

    private bool isCreatingCoroutine = false; // 부품 생성 코루틴 동작 여부
    private bool picked = true; // 부품 주운 후

    public GameObject partPrefab; // 부품 오브젝트

    [SerializeField]
    private List<double> pathLatitude = new List<double>(); // GPS 경로 (위도)
    [SerializeField]
    private List<double> pathLongitude = new List<double>(); // GPS 경로 (경도)
    [SerializeField]
    private List<int> partCntPerPoint = new List<int>(); // 지점마다 생성되는 부품 개수
    [SerializeField]
    private double targetRadius; // 목표 반경
    [SerializeField]
    private List<PartTransformInfo> partTransformInfo = new List<PartTransformInfo>(); // part transform 정보
    [SerializeField]
    private string lastPartTriggerScene; // 마지막 부품 주웠을 때 이동할 씬 이름
    [SerializeField]
    private string partLayerMaskName; // 부품 레이어 마스크 이름 (기본 부품이면 "Part")
    [SerializeField]
    private GameObject aimObj; // 부품 조준하는 에임 오브젝트

    private int currPathIdx = 0; // 현재까지 온 길 번호 (pathGpsInfo 의 index)
    private int partLayerMask; // 부품 레이어 마스크 (부품 주울 때, 부품 layer에만 ray 쏠 때 사용)

    private List<GameObject> lastPathParts = new List<GameObject>(); // 마지막 반경에서 생성된 부품 저장

    private Vector3 originAimPos; // 부품 에임 기본 위치

    private int hintInterval, hintIntervalTarget; // 힌트 생성되는 간격

    private bool partInSamePoint = false; // 부품이 같은 포인트 내에서 생성되는 중인지 true / false. true -> GPS 반경 보지 않고 부품 생성
    private int createdPartCnt = 0; // 생성된 부품 개수


    private void Awake()
    {
        // popup manager 스크립트
        popupManager = GameObject.Find("PopupManager").GetComponent<PopupManager>();
        hintManager = gameObject.GetComponent<HintManager>();

        float screenWidth = Screen.width;
        float screenHeight = Screen.height;
        Debug.Log(screenWidth);
        Debug.Log(screenHeight);

        // 물체 생성 스크린 좌표 스크린 비율에 맞추기
        for (int i = 0; i < createdPos.Count; i++)
        {
            float originX = createdPos[i].x;
            float originY = createdPos[i].y;
            createdPos[i] = new Vector2(originX * screenWidth / screenBiasWidth, originY * screenHeight / screenBiasHeigth);
            popupManager.SetDebuggingScreenPosTxt(i, createdPos[i]);
        }

        arRaycastManager = GetComponent<ARRaycastManager>();
    }

    private void Start()
    {
        partLayerMask = LayerMask.GetMask(partLayerMaskName);

        originAimPos = RectTransformUtility.WorldToScreenPoint(null, aimObj.GetComponent<Image>().rectTransform.position);

        // 전체 부품 개수 계산
        totalPartCnt = 0;
        for (int i = 0; i < partCntPerPoint.Count; i++)
        {
            totalPartCnt += partCntPerPoint[i];
        }
        hintInterval = totalPartCnt / hintManager.totalHintCnt;
        hintIntervalTarget = hintInterval / 2;

        popupManager.SetPartCntTxt(0, totalPartCnt);

        StartCoroutine(PartGuide());
    }

    private IEnumerator PartGuide()
    {
        yield return new WaitForSeconds(0.1f);

        float interval = 3f;
        popupManager.OpenPartGuide(interval);
        yield return new WaitForSeconds(interval);
        
        // GPS path 및 줍기 활성화
        StartCoroutine(CheckGPSPath());
        StartCoroutine(CheckPickPart());
    }

    private IEnumerator CheckGPSPath()
    {
        while (true)
        {
            if (currPathIdx == pathLatitude.Count)
            {
                yield break;
            }

            // GPS 가 path 반경 안에 들어왔는지 확인
            bool isInRadius = GPSManager.Instance.CheckCurrPosInRadius(pathLatitude[currPathIdx], pathLongitude[currPathIdx], targetRadius);
            if (!isCreatingCoroutine && (isInRadius || partInSamePoint) && picked)
            {
                picked = false;
                partInSamePoint = true;
                CreateManyPart(currPathIdx == 0);

                if (isInRadius && createdPartCnt == 0)
                {
                    currPathIdx++;
                }

                popupManager.SetPartInfoTxt(currPathIdx, createdPartCnt);
            }

            yield return null;
        }
    }

    private int CheckAimedPart(Vector3 rayPos)
    {
        popupManager.SetScreenPosTxt(rayPos);

        Ray screenRay = Camera.main.ScreenPointToRay(rayPos);

        if (Physics.Raycast(screenRay.origin, screenRay.direction, out hitInfoPart, Mathf.Infinity, partLayerMask))
        {
            // (테스트용) 거리 확인
            popupManager.SetDistancePart(hitInfoPart.distance);

            // 부품이 특정 반경 내에 있을 경우에만 주울 수 있도록
            if (hitInfoPart.distance <= partRadius)
            {
                return 2;
            }
            else
            {
                return 1;
            }
        }
        return 0;
    }

    private IEnumerator CheckPickPart()
    {        
        while (true)
        {
            int checkAimedPartIdx = CheckAimedPart(originAimPos);
            // raycast 계속 쏘면서 부품 주울 수 있는지 확인
            if (checkAimedPartIdx == 2)
            {
                popupManager.OpenPickPartSnackbar();
                SoundEffectManager.Instance.Play(0);

                Destroy(hitInfoPart.collider.gameObject);
                partCnt++;

                float interval = 1.5f;

                // 마지막 부품 클릭했으면 카메라 씬으로 이동
                GameObject lastPart = GetLastPart();
                if (lastPart != null && hitInfoPart.collider.gameObject == lastPart)
                {
                    Debug.Log("마지막 반경에서 가장 마지막으로 생성된 부품을 클릭했습니다.");
                    popupManager.partInfoTxt.text = "마지막 부품 주움 !!";
                    yield return new WaitForSeconds(1f);
                    SceneManager.LoadScene(lastPartTriggerScene);
                }

                // 부품 처음 줍는 거라면 팝업 띄우기
                if (partCnt == 1 && lastPart == null)
                {
                    popupManager.OpenFirstPartPopup();
                }

                popupManager.SetPartCntTxt(partCnt, totalPartCnt);
                popupManager.SetDebuggingPartTxt(partCnt);
                StartCoroutine(PickingPart(interval));
            }
            else if (checkAimedPartIdx == 1)
            {
                // 특정 반경 내에 없으면 스낵바 띄워서 안내
                popupManager.OpenPartScnackbar();
            }

            yield return null;
        }
    }

    private IEnumerator PickingPart(float interval)
    {
        yield return new WaitForSeconds(interval);

        picked = true;
    }

    private void CreateManyPart(bool isFirst)
    {
        // 부품 일정 시간별로 여러 개 생성
        StartCoroutine(CreatePartPerSeconds(isFirst));
    }

    private bool CreateOnePart(Vector2 pos)
    {
        if (partInSamePoint == false)
        {
            return false;
        }

        // 인식한 바닥 (TrackableType.PlaneWithinPolygon) 과 닿았다면 부품 생성
        if (arRaycastManager.Raycast(pos, hits, TrackableType.PlaneWithinPolygon))
        {
            var hitPose = hits[0].pose;
            GameObject part = Instantiate(partPrefab, hitPose.position, hitPose.rotation);
            part.transform.localEulerAngles = partTransformInfo[1].value;
            part.transform.localScale = partTransformInfo[2].value;

            // 생성된 부품 개수 증가
            createdPartCnt++;

            // 힌트 아이템 생성
            float currentPartCnt = createdPartCnt;
            if (currPathIdx - 2 >= 0)
            {
                currentPartCnt += partCntPerPoint[currPathIdx - 2];
            }
            if (currentPartCnt % hintInterval == hintIntervalTarget)
            {
                hintManager.CreateHintItem();
            }

            // 현재 반경의 부품 개수 모두 생성했다면
            if (partCntPerPoint[currPathIdx-1] == createdPartCnt)
            {
                createdPartCnt = 0;
                partInSamePoint = false;
            }

            // 마지막 부품 반경에서 생성된 부품일 경우
            if (currPathIdx == pathLatitude.Count)
            {
                lastPathParts.Add(part);
            }
            return true;
        }

        return false;
    }

    // 바닥 생성됐는지 확인하는 함수
    private bool CheckCreatedPlane(Vector2 createdPos)
    {
        if (arRaycastManager.Raycast(createdPos, hits, TrackableType.PlaneWithinPolygon) == false)
        {
            if (popupManager.planeSnackbar.activeSelf == false)
            {
                popupManager.planeSnackbar.SetActive(true);
                popupManager.planeSnackbar.GetComponent<FadeInOut>().FadeInAll();
            }
            return false;
        }
        popupManager.planeSnackbar.SetActive(false);
        popupManager.planeSnackbar.GetComponent<FadeInOut>().FadeOutAll();
        return true;
    }

    private IEnumerator CreatePartPerSeconds(bool isFirst)
    {
        if (isCreatingCoroutine) yield break;

        isCreatingCoroutine = true;

        if (isFirst)
        {
            // 처음 부품 생성이라면 -> createdPos 만큼 모두 생성
            for (int i = 0; i < createdPos.Count; i++)
            {
                while (CheckCreatedPlane(createdPos[i]) == false)
                {
                    yield return null;
                }

                CreateOnePart(createdPos[i]);
                popupManager.partInfoTxt.text += i.ToString() + "th part ";
                yield return new WaitForSeconds(1);
            }

        }
        else
        {
            // 그 다음 부터는 가장 마지막 createdPos만 생성
            while (CheckCreatedPlane(createdPos[createdPos.Count - 1]) == false)
            {
                yield return null;
            }

            CreateOnePart(createdPos[createdPos.Count - 1]);
            popupManager.partInfoTxt.text += "only 1th part";
            yield return new WaitForSeconds(1);
        }

        isCreatingCoroutine = false;
    }

    private GameObject GetLastPart()
    {
        if (lastPathParts.Count > 0)
        {
            return lastPathParts[lastPathParts.Count - 1];
        }
        else
        {
            return null;
        }
    }

}
