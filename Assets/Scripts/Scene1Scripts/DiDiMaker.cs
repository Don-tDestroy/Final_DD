using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using TMPro;

public class DiDiMaker : MonoBehaviour
{
    [System.Serializable]
    public struct DiDiTransformInfo
    {
        public string key;
        public Vector3 value;
    }

    private ARRaycastManager arRaycastManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    public GameObject didiPrefab;

    private readonly float screenBiasWidth = 1440f;
    private readonly float screenBiasHeigth = 2560f;
    private Vector2 createdPos = new Vector2(650f, 1300f); 
    private readonly float didiRadius = 1.1f; // 디디 특정 반경 내에서만 주울 수 있도록 

    [SerializeField]
    private List<DiDiTransformInfo> didiTransformInfo = new List<DiDiTransformInfo>();

    private int partLayerMask;

    private GameObject didiObj;

    //임시
    public TextMeshProUGUI txt;

    private void Awake()
    {
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // 디디 생성 스크린 좌표 스크린 비율에 맞추기
        float originX = createdPos.x;
        float originY = createdPos.y;
        createdPos = new Vector2(originX * screenWidth / screenBiasWidth, originY * screenHeight / screenBiasHeigth);

        arRaycastManager = GetComponent<ARRaycastManager>();
        partLayerMask = LayerMask.GetMask("Part");
    }

    private void Start()
    {
        StartCoroutine(CreateDiDiOnPlane());
    }

    private IEnumerator CreateDiDiOnPlane()
    {
        while (true)
        {
            // 이미 디디가 생성된 경우 추가 생성하지 않음
            if (didiObj == null && arRaycastManager.Raycast(createdPos, hits, TrackableType.PlaneWithinPolygon))
            {
                var hitPose = hits[0].pose;
                Vector3 spawnPosition = hitPose.position + hitPose.forward * 3f - hitPose.up*0.1f; // 위치 조정
                Quaternion spawnRotation = hits[0].pose.rotation * Quaternion.Euler(0f, 180f, 0f);

                didiObj = Instantiate(didiPrefab, spawnPosition, spawnRotation);
                didiObj.transform.localScale = Vector3.one * 0.2f;

                StartCoroutine(TouchDiDi());
            }

            yield return null;
        }
    }

    private IEnumerator TouchDiDi()
    {
        while (true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                txt.text = "터치";
                Vector3 mousePos = Input.mousePosition;
                Ray screenRay = Camera.main.ScreenPointToRay(mousePos);

                RaycastHit hit;

                if (Physics.Raycast(screenRay.origin, screenRay.direction, out hit, Mathf.Infinity, partLayerMask))
                {
                    txt.text = hit.collider.gameObject.name;
                    if (hit.collider.gameObject == didiObj)
                    {
                        txt.text = "디디 터치";
                        Destroy(didiObj);
                        CreateDiDiOnCanvas();
                    }
                }
            }
            yield return null;
        }
    }

    private void CreateDiDiOnCanvas()
    {
        didiObj = Instantiate(didiPrefab, Vector3.zero, Quaternion.identity, GameObject.FindGameObjectWithTag("Canvas").transform);
        didiObj.transform.localPosition = didiTransformInfo[0].value;
        didiObj.transform.localScale = didiTransformInfo[2].value;
        didiObj.transform.localEulerAngles = didiTransformInfo[1].value;
    }

}
