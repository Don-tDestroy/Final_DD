using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class HintManager : MonoBehaviour
{
    // 팝업 및 UI 관련 매니저 스크립트
    private PopupManager popupManager;

    // 힌트 생성 관련 ray info
    private ARRaycastManager arRaycastManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    // 힌트 줍기 관련 ray info
    private RaycastHit hitInfoHint;
    private int hintCnt = 0;

    // 전체 힌트 개수
    private int totalHintCnt = 5;

    // 힌트 오브젝트
    public GameObject hintPrefab;

    [SerializeField]
    private string hintLayerMaskName; // 힌트 레이어 마스크 이름 ("Hint")
    [SerializeField]
    private List<string> hintMessage = new List<string>(); // 힌트 메세지

    private int hintLayerMask; // 힌트 아이템 레이어 마스트

    private readonly float screenBiasWidth = 1440f;
    private readonly float screenBiasHeigth = 2560f;
    // 힌트 생성 범위
    private readonly List<List<Vector2>> createdPosInterval = new List<List<Vector2>>() { new List<Vector2>() { new Vector2(200f, 1300f), new Vector2(500f, 1800f) },
                                                                                        new List<Vector2>() { new Vector2(700f, 1300f), new Vector2(1300f, 1800f) },
                                                                                        new List<Vector2>() { new Vector2(200f, 1900f), new Vector2(500f, 2300f) },
                                                                                        new List<Vector2>() { new Vector2(700f, 1900f), new Vector2(1300f, 2300f) },
                                                                                        }; // (1440, 2560) 기준 좌표
    [SerializeField]
    private List<PartTransformInfo> hintTransformInfo = new List<PartTransformInfo>(); // hint transform 정보

    private void Awake()
    {
        // popup manager 스크립트
        popupManager = GameObject.Find("PopupManager").GetComponent<PopupManager>();

        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // 물체 생성 스크린 좌표 스크린 비율에 맞추기
        for (int i = 0; i < createdPosInterval.Count; i++)
        {
            for(int j = 0; j < createdPosInterval[i].Count; j++)
            {
                float originX = createdPosInterval[i][j].x;
                float originY = createdPosInterval[i][j].y;
                createdPosInterval[i][j] = new Vector2(originX * screenWidth / screenBiasWidth, originY * screenHeight / screenBiasHeigth);
            }
        }

        arRaycastManager = GetComponent<ARRaycastManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        hintLayerMask = LayerMask.GetMask(hintLayerMaskName);

        // 힌트 아이템 생성 및 줍기 활성화
        StartCoroutine(CheckPickPart());
    }

    private IEnumerator CheckPickPart()
    {
        while (true)
        {
            // 힌트 줍기
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mousePos = Input.mousePosition;
                Ray screenRay = Camera.main.ScreenPointToRay(mousePos);

                if (Physics.Raycast(screenRay.origin, screenRay.direction, out hitInfoHint, Mathf.Infinity, hintLayerMask))
                {
                    SoundEffectManager.Instance.Play(0);

                    Destroy(hitInfoHint.collider.gameObject);

                    popupManager.OpenHint(hintMessage[hintCnt]);
                    hintCnt++;

                    popupManager.OpenFirstPartPopup();

                    popupManager.SetHintCntTxt(hintCnt, totalHintCnt);
                }
            }
            yield return null;
        }
    }

    private bool CreateHint(Vector2 pos)
    {
        // 인식한 바닥 (TrackableType.PlaneWithinPolygon) 과 닿았다면 부품 생성
        if (arRaycastManager.Raycast(pos, hits, TrackableType.PlaneWithinPolygon))
        {
            var hitPose = hits[0].pose;
            GameObject hint = Instantiate(hintPrefab, hitPose.position, hitPose.rotation);
            hint.transform.localPosition += hintTransformInfo[0].value;
            hint.transform.localEulerAngles = hintTransformInfo[1].value;
            hint.transform.localScale = hintTransformInfo[2].value;

            return true;
        }

        return false;
    }


    // 바닥 생성됐는지 확인하는 함수
    private bool CheckCreatedPlane(Vector2 createdPos)
    {
        if (arRaycastManager.Raycast(createdPos, hits, TrackableType.PlaneWithinPolygon) == false)
        {
            return false;
        }
        return true;
    }

    public void CreateHintItem()
    {
        StartCoroutine(CreateHintItemCoroutine());
    }

    private IEnumerator CreateHintItemCoroutine()
    {
        Vector2 createPos = new Vector2(0f, 0f);
        while (true)
        {
            // 좌표 랜덤으로 설정
            int randIdx = Random.Range(0, createdPosInterval.Count);
            List<Vector2> createIntervalList = createdPosInterval[randIdx];
            float x = Random.Range(createIntervalList[0].x, createIntervalList[1].x);
            float y = Random.Range(createIntervalList[0].y, createIntervalList[1].y);
            createPos = new Vector2(x, y);

            if (CheckCreatedPlane(createPos) == false)
            {
                yield return null;
            }
            else
            {
                break;
            }
        }

        CreateHint(createPos);
        yield return new WaitForSeconds(1);
    }
}
