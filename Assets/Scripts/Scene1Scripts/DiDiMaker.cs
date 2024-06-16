using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using TMPro;
using UnityEngine.SceneManagement;

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
    public GameObject storyObj;

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

        StartCoroutine(StartStory(0, false));
    }

    private IEnumerator StartStory(int storyIndex, bool isDiddyVisible)
    {
        txt.text = "Story 실행됐다!";
        Debug.Log("Story 실행됐다!");
        storyObj.SetActive(true);
        StoryManager storyManager = storyObj.GetComponent<StoryManager>();
        storyManager.isFinished = false;
        storyManager.StoryIndex = storyIndex;
        storyManager.isDiddyVisible = isDiddyVisible;

        txt.text = "isFinished: " + storyManager.isFinished + "\nstoryindex: " + storyManager.StoryIndex + "\nisDiddyVisible: " + storyManager.isDiddyVisible;

        if (storyIndex == 1)
        {
            StopCoroutine(TouchDiDi());
        }

        while (!storyManager.isFinished)
        {
            yield return null;
        }

        txt.text = "끝!";
        Debug.Log("끝!");

        storyObj.SetActive(false);

        if (storyIndex == 0)
        {
            StartCoroutine(CreateDiDiOnPlane());
            StartCoroutine(TouchDiDi());
            StopCoroutine(StartStory(0, false));
        }
        else if (storyIndex == 1)
        {
            txt.text = "다음 씬을 불러와요!";
            Debug.Log("다음 씬을 불러와요!");
            SceneManager.LoadSceneAsync("Scene_2");
            StopCoroutine(StartStory(1, true));
        }
    }

    private IEnumerator CreateDiDiOnPlane()
    {
        txt.text = "바닥을 인식해요!";
        Debug.Log("바닥을 인식해요!");

        while (true)
        {
            if (touched == false)
            {
                while (CheckCreatedPlane() == false)
                {
                    yield return null;
                }

                if (didiObj == null && arRaycastManager.Raycast(createdPos, hits, TrackableType.PlaneWithinPolygon))
                {
                    var hitPose = hits[0].pose;

                    Vector3 didiPosition = hitPose.position;

                    Camera mainCamera = Camera.main;
                    Vector3 directionToCamera = mainCamera.transform.position - didiPosition;
                    Quaternion didiRotation = Quaternion.LookRotation(-directionToCamera) * didiPrefab.transform.rotation;

                    didiObj = Instantiate(didiPrefab, didiPosition, didiRotation);
                    StopCoroutine(CreateDiDiOnPlane());
                }
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
        txt.text = "터치디디 코루틴";
        Debug.Log("터치 코루틴");
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
                        txt.text = "터치디디";
                        Debug.Log("터치디디");
                        touched = true;
                        Destroy(didiObj);
                        StartCoroutine(StartStory(1, true));
                    }
                }
            }
            yield return null;
        }
    }

}
