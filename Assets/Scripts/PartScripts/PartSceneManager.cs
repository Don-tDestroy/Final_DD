using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using TMPro;
using UnityEngine.SceneManagement;

[System.Serializable]
public struct PartTransformInfo
{
    public string key;
    public Vector3 value;
}

public class PartSceneManager : MonoBehaviour
{
    // 부품 생성 관련 ray info
    private ARRaycastManager arRaycastManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    // 부품 줍기 관련 ray info
    private RaycastHit hitInfo;
    private int partCnt = 0;

    public TextMeshProUGUI screenPosTxt; // (테스트용) 스크린 클릭 position
    public TextMeshProUGUI partInfoTxt; // (테스트용) 부품 생성 확인
    public TextMeshProUGUI partCntTxt; // 주운 부품 개수 확인 [TODO] 디자인 적용해야함
    public TextMeshProUGUI distancePart; // (테스트용) 클릭한 부품과의 거리

    public GameObject partPrefab;
    public GameObject firstPartPopup; // 처음 부품 주운 후 나오는 팝업
    public GameObject partGuideCanavas; // 씬 시작할 때 나오는 부품 줍기 가이드 팝업
    public GameObject planeSnackbar; // 바닥 인식 중에 뜨는 스낵바
    public GameObject partSnackbar; // 부품 줍기 중에 뜨는 스낵바

    private readonly float screenBiasWidth = 1440f;
    private readonly float screenBiasHeigth = 2560f;
    private readonly List<Vector2> createdPos = new List<Vector2>() { new Vector2(650f, 1300f), new Vector2(650f, 2000) }; // (1440, 2560) 기준 좌표
    private readonly float partRadius = 1.1f; // 부품 특정 반경 내에서만 주울 수 있도록 

    private bool isCreatingCoroutine = false; // 부품 생성 코루틴 동작 여부
    private bool picked = true; // 부품 주운 후

    [SerializeField]
    private List<double> pathLatitude = new List<double>(); // GPS 경로 (위도)
    [SerializeField]
    private List<double> pathLongitude = new List<double>(); // GPS 경로 (경도)
    [SerializeField]
    private double targetRadius; // 목표 반경
    [SerializeField]
    private List<PartTransformInfo> partTransformInfo = new List<PartTransformInfo>(); // part transform 정보
    [SerializeField]
    private string lastPartTriggerScene; // 마지막 부품 주웠을 때 이동할 씬 이름


    private int currPathIdx = 0; // 현재까지 온 길 번호 (pathGpsInfo 의 index)
    private int partLayerMask; // 부품 레이어 마스크 (부품 주울 때, 부품 layer에만 ray 쏠 때 사용)

    private List<GameObject> lastPathParts = new List<GameObject>(); // 마지막 반경에서 생성된 부품 저장


    private void Awake()
    {
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;
        Debug.Log(screenWidth);
        Debug.Log(screenHeight);

        // 물체 생성 스크린 좌표 스크린 비율에 맞추기
        screenPosTxt.text = "물체 생성 위치\n";
        for (int i = 0; i < createdPos.Count; i++)
        {
            float originX = createdPos[i].x;
            float originY = createdPos[i].y;
            createdPos[i] = new Vector2(originX * screenWidth / screenBiasWidth, originY * screenHeight / screenBiasHeigth);
            screenPosTxt.text += createdPos[i].ToString() + "\n";
        }

        arRaycastManager = GetComponent<ARRaycastManager>();
        partLayerMask = LayerMask.GetMask("Part");
    }

    private void Start()
    {
        StartCoroutine(PartGuide());
    }

    private IEnumerator PartGuide()
    {
        yield return new WaitForSeconds(0.1f);

        // 가이드 캔버스
        SoundEffectManager.Instance.Play(1);
        partGuideCanavas.SetActive(true);

        // 기다리기
        yield return new WaitForSeconds(3f);

        // 가이드 캔버스 지우기
        partGuideCanavas.SetActive(false);

        // GPS path 및 줍기 활성화
        StartCoroutine(CheckGPSPath());
        StartCoroutine(CheckPickPart());
    }

    private IEnumerator CheckGPSPath()
    {
        while (true)
        {
            // GPS 가 path 반경 안에 들어왔는지 확인
            bool isInRadius = GPSManager.Instance.CheckCurrPosInRadius(pathLatitude[currPathIdx], pathLongitude[currPathIdx], targetRadius);
            if (!isCreatingCoroutine && isInRadius && picked)
            {
                picked = false;
                partInfoTxt.text = (currPathIdx + 1).ToString() + "번째 반경 ";
                CreateManyPart(currPathIdx == 0);
                currPathIdx++;
            }

            yield return null;
        }
    } 

    private IEnumerator CheckPickPart()
    {
        while (true)
        {
            // 부품 줍기
            if (Input.GetMouseButtonDown(0))
            {
                screenPosTxt.text = "현재 클릭 위치\n" + Input.mousePosition.ToString();
                screenPosTxt.text += "부품 줍기";
                Vector3 mousePos = Input.mousePosition;
                Ray screenRay = Camera.main.ScreenPointToRay(mousePos);

                if (Physics.Raycast(screenRay.origin, screenRay.direction, out hitInfo, Mathf.Infinity, partLayerMask))
                {
                    // (테스트용) 거리 확인
                    distancePart.text = "부품 거리 " + hitInfo.distance;

                    // 부품이 특정 반경 내에 있을 경우에만 주울 수 있도록
                    if (hitInfo.distance <= partRadius)
                    {
                        screenPosTxt.text += "없어짐 ! ";
                        SoundEffectManager.Instance.Play(0);

                        // 마지막 부품 클릭했으면 카메라 씬으로 이동
                        GameObject lastPart = GetLastPart();
                        if (lastPart != null && hitInfo.collider.gameObject == lastPart)
                        {
                            Debug.Log("마지막 반경에서 가장 마지막으로 생성된 부품을 클릭했습니다.");
                            SceneManager.LoadScene(lastPartTriggerScene);
                        }

                        Destroy(hitInfo.collider.gameObject);
                        partCnt++;

                        // 부품 처음 줍는 거라면 팝업 띄우기
                        if (partCnt == 1 && lastPart == null)
                        {
                            firstPartPopup.SetActive(true);
                            Time.timeScale = 0; // 시간 멈추기 (그 다음 부품 생성되는 시간 맞추기 위해)
                        }

                        partCntTxt.text = "part count : " + partCnt;
                        StartCoroutine(PickingPart(2f));
                    }
                    else
                    {
                        // 특정 반경 내에 없으면 스낵바 띄워서 안내
                        StartCoroutine(OpenPartSnackbar());
                    }
                   
                }
            }
            yield return null;
        }
    }

    
    private IEnumerator OpenPartSnackbar()
    {
        partSnackbar.SetActive(true);
        partSnackbar.GetComponent<FadeInOut>().FadeInAll();

        yield return new WaitForSeconds(2f);

        partSnackbar.SetActive(false);
        partSnackbar.GetComponent<FadeInOut>().FadeOutAll();
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
        // 인식한 바닥 (TrackableType.PlaneWithinPolygon) 과 닿았다면 부품 생성
        if (arRaycastManager.Raycast(pos, hits, TrackableType.PlaneWithinPolygon))
        {
            var hitPose = hits[0].pose;
            GameObject part = Instantiate(partPrefab, hitPose.position, hitPose.rotation);
            part.transform.localEulerAngles = partTransformInfo[1].value;


            // 마지막 부품 반경에서 생성된 부품일 경우
            if (currPathIdx == pathLatitude.Count - 1)
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
            if (planeSnackbar.activeSelf == false)
            {
                planeSnackbar.SetActive(true);
                planeSnackbar.GetComponent<FadeInOut>().FadeInAll();
            }
            return false;
        }
        planeSnackbar.SetActive(false);
        planeSnackbar.GetComponent<FadeInOut>().FadeOutAll();
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
                partInfoTxt.text += i.ToString() + "th part ";
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
            partInfoTxt.text += "only 1th part";
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


    public void PopupOkButton()
    {
        Time.timeScale = 1; // 시간 다시 흐르기
    }
}
