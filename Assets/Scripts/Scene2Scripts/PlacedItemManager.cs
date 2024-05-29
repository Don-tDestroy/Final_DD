using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using TMPro;
using UnityEngine.SceneManagement;


public class PlacedItemManager : MonoBehaviour
{
    // 부품 생성 관련 ray info
    private ARRaycastManager arRaycastManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    // 부품 줍기 관련 ray info
    private RaycastHit hitInfo;

    public TextMeshProUGUI screenPosTxt; // (테스트용) 스크린 클릭 position
    public TextMeshProUGUI itemInfoTxt; // (테스트용) 아이템 생성 확인
    public TextMeshProUGUI itemGetTxt; // 아이템 주움 여부 확인 [TODO] 디자인 적용해야함
    public TextMeshProUGUI distanceItem; // (테스트용) 클릭한 부품과의 거리
    
    public GameObject itemPrefab;
    public GameObject itemGuidePopup; // 씬 시작할 때 나오는 아이템 줍기 가이드 팝업
    public GameObject GetItemPopup; // 아이템 주운 후 나오는 팝업
    public GameObject itemSnackbar; // 부품 줍기 중에 뜨는 스낵바
    public GameObject planeSnackbar; // 바닥 인식 중에 뜨는 스낵바

    private readonly float screenBiasWidth = 1440f;
    private readonly float screenBiasHeigth = 2560f;
    private readonly List<Vector2> createdPos = new List<Vector2>() { new Vector2(650f, 1300f), new Vector2(650f, 2000) }; // (1440, 2560) 기준 좌표
    private readonly float itemRadius = 1.1f; // 부품 특정 반경 내에서만 주울 수 있도록 

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
    private string afterPickCameraTrigger;

    private int currPathIdx = 0; // 현재까지 온 길 번호 (pathGpsInfo 의 index) // PlacedItemManager에서는 사용되지 x, 추후 삭제
    private int partLayerMask; // 부품 레이어 마스크 (부품 주울 때, 부품 layer에만 ray 쏠 때 사용)


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

        // 고정된 카메라 생성
        GameObject guidePartObj = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity, GameObject.FindGameObjectWithTag("Canvas").transform);
        guidePartObj.transform.localPosition = partTransformInfo[0].value;
        guidePartObj.transform.localScale = partTransformInfo[2].value;
        guidePartObj.transform.localEulerAngles = partTransformInfo[1].value;

        // 하단 팝업 생성
        SoundEffectManager.Instance.Play(1);
        itemGuidePopup.SetActive(true);

        // 기다리기
        yield return new WaitForSeconds(3f);

        // 고정된 카메라 & 하단 팝업 지우기
        Destroy(guidePartObj);
        itemGuidePopup.SetActive(false);

        // GPS path 및 줍기 활성화
        StartCoroutine(CheckGPSPath());
        StartCoroutine(CheckPickPart());
    }

    private IEnumerator CheckGPSPath()
    {
        yield return new WaitForSeconds(3f); // 일정 시간 뒤 카메라 생성

        while (true)
        {
            // GPS 가 path 반경 안에 들어왔는지 확인
            bool isInRadius = GPSManager.Instance.CheckCurrPosInRadius(pathLatitude[currPathIdx], pathLongitude[currPathIdx], targetRadius);
            if (!isCreatingCoroutine && isInRadius && picked)
            {
                // 1번만 카메라 아이템을 생성
                picked = false;
                itemInfoTxt.text = (currPathIdx + 1).ToString() + "번째 반경에서 카메라 아이템을 생성...";
                StartCoroutine(CreateOnePart(createdPos[0]));
                currPathIdx++;
                yield break; // 끝냄
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
                screenPosTxt.text += "아이템 줍기";
                Vector3 mousePos = Input.mousePosition;
                Ray screenRay = Camera.main.ScreenPointToRay(mousePos);

                if (Physics.Raycast(screenRay.origin, screenRay.direction, out hitInfo, Mathf.Infinity, partLayerMask))
                {
                    // (테스트용) 거리 확인
                    distanceItem.text = "부품 거리 " + hitInfo.distance;

                    // 아이템이 특정 반경 내에 있을 경우에만 주울 수 있도록
                    if (hitInfo.distance <= itemRadius)
                    {
                        SoundEffectManager.Instance.Play(0);
                        Destroy(hitInfo.collider.gameObject);

                        // 카메라를 주우면 종료
                        if (picked == false)
                        {
                            Debug.Log("카메라를 주웠습니다");
                            GetItemPopup.SetActive(true);
                            picked = true;
                            // yield break; // 끝냄
                            yield return new WaitForSeconds(1f);
                            SceneManager.LoadScene(afterPickCameraTrigger);
                        }
                    }
                    else
                    {
                        // 특정 반경 내에 없으면 스낵바 띄워서 안내
                        StartCoroutine(OpenItemSnackbar());
                    }

                }
            }
            yield return null;
        }
    }

    private IEnumerator OpenItemSnackbar()
    {
        itemSnackbar.SetActive(true);
        itemSnackbar.GetComponent<FadeInOut>().FadeInAll();

        yield return new WaitForSeconds(2f);

        itemSnackbar.SetActive(false);
        itemSnackbar.GetComponent<FadeInOut>().FadeOutAll();
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


    private IEnumerator CreateOnePart(Vector2 pos)
    {
        // 그 다음 부터는 가장 마지막 createdPos만 생성
        while (CheckCreatedPlane(pos) == false)
        {
            yield return null;
        }

        // 인식한 바닥 (TrackableType.PlaneWithinPolygon) 과 닿았다면 부품 생성
        if (arRaycastManager.Raycast(pos, hits, TrackableType.PlaneWithinPolygon))
        {
            var hitPose = hits[0].pose;
            GameObject part = Instantiate(itemPrefab, hitPose.position, hitPose.rotation);
            part.transform.localEulerAngles = partTransformInfo[1].value;
        }
    }
}
