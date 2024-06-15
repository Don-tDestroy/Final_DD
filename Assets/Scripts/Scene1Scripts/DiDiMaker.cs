using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using TMPro;

[System.Serializable]
public struct DiDiTransformInfo
{
    public string key;
    public Vector3 value;
}

public class DiDiMaker : MonoBehaviour
{
 
    private ARRaycastManager arRaycastManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    public GameObject didiPrefab;
    public GameObject planeSnackbar;

    private readonly float screenBiasWidth = 1440f;
    private readonly float screenBiasHeigth = 2560f;
    private Vector2 createdPos = new Vector2(650f, 1700f); 

    [SerializeField]
    private List<DiDiTransformInfo> didiTransformInfo = new List<DiDiTransformInfo>();

    private int partLayerMask;

    private GameObject didiObj;
    private bool touched = false;

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
        StartCoroutine(TouchDiDi());
    }

    private IEnumerator CreateDiDiOnPlane()
    {
        while (true)
        {
            if(touched == false)
            {
                while (CheckCreatedPlane() == false)
                {
                    yield return null;
                }
            }

            if (didiObj == null && arRaycastManager.Raycast(createdPos, hits, TrackableType.PlaneWithinPolygon))
            {
                var hitPose = hits[0].pose;

                Vector3 didiPosition = hitPose.position;

                Camera mainCamera = Camera.main;
                Vector3 directionToCamera = mainCamera.transform.position - didiPosition;
                Quaternion didiRotation = Quaternion.LookRotation(-directionToCamera) * didiPrefab.transform.rotation;

                didiObj = Instantiate(didiPrefab, didiPosition, didiRotation);
            }

            yield return null;
        }
    }

    private bool CheckCreatedPlane()
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

    private IEnumerator TouchDiDi()
    {
        while (true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mousePos = Input.mousePosition;
                Ray screenRay = Camera.main.ScreenPointToRay(mousePos);

                RaycastHit hit;

                if (Physics.Raycast(screenRay.origin, screenRay.direction, out hit, Mathf.Infinity, partLayerMask))
                {
                    txt.text = hit.collider.gameObject.name;
                    if (hit.collider.gameObject == didiObj)
                    {
                        touched = true;
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