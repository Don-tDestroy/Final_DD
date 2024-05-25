using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using TMPro;


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
    public TextMeshProUGUI partCntTxt; // (테스트용) 주운 부품 개수 확인
    public GameObject partPrefab;
    public GameObject firstPartPopup; // 처음 부품 주운 후 나오는 팝업

    private readonly float screenBiasWidth = 1440f;
    private readonly float screenBiasHeigth = 2560f;
    private readonly List<Vector2> createdPos = new List<Vector2>() { new Vector2(650f, 1300f), new Vector2(650f, 2000) }; // (1440, 2560) 기준 좌표

    private bool isCreatingCoroutine = false; // 부품 생성 코루틴 동작 여부
    private bool picked = true; // 부품 주운 후

    [SerializeField]
    private List<double> pathLatitude = new List<double>(); // GPS 경로 (위도)
    [SerializeField]
    private List<double> pathLongitude = new List<double>(); // GPS 경로 (경도)
    [SerializeField]
    private double targetRadius; // 목표 반경


    private int currPathIdx = 0; // 현재까지 온 길 번호 (pathGpsInfo 의 index)
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
                    screenPosTxt.text += "없어짐 ! ";
                    Debug.Log("없어짐 !!");
                    SoundEffectManager.Instance.Play(0);
                    Destroy(hitInfo.collider.gameObject);
                    partCnt++;

                    partCntTxt.text = "part count : " + partCnt;
                    StartCoroutine(PickingPart(2f));
                }
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
        StartCoroutine(CreatePartPerSeconds(isFirst));
    }

    private bool CreateOnePart(Vector2 pos)
    {
        if (arRaycastManager.Raycast(pos, hits, TrackableType.PlaneWithinPolygon))
        {
            var hitPose = hits[0].pose;
            Instantiate(partPrefab, hitPose.position, hitPose.rotation);
            return true;
        }

        return false;
    }

    private IEnumerator CreatePartPerSeconds(bool isFirst)
    {
        if (isCreatingCoroutine) yield break;

        isCreatingCoroutine = true;

        if (isFirst)
        {
            for (int i = 0; i < createdPos.Count; i++)
            {
                while (arRaycastManager.Raycast(createdPos[i], hits, TrackableType.PlaneWithinPolygon) == false)
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
            while (arRaycastManager.Raycast(createdPos[1], hits, TrackableType.PlaneWithinPolygon) == false)
            {
                yield return null;
            }

            CreateOnePart(createdPos[1]);
            partInfoTxt.text += "only 1th part";
            yield return new WaitForSeconds(1);
        }

        isCreatingCoroutine = false;
    }
}
