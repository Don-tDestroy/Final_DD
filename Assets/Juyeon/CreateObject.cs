using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using TMPro;
//using UnityEngine.XR.OpenXR.Input;

public class CreateObject : MonoBehaviour
{
    private ARRaycastManager arRaycastManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();
    public TextMeshProUGUI pos_text;
    public TextMeshProUGUI another_text;
    int i = 0;
    public GameObject[] Parts;


    private float screenBiasWidth = 1440;
    private float screenBiasHeigth = 2560;
    private List<Vector2> createdPos = new List<Vector2>() { new Vector2(650, 100), new Vector2(650, 900), new Vector2(650, 1600) };

    private bool isRunningCoroutine = false;


    private void Awake()
    {
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;
        Debug.Log(screenWidth);
        Debug.Log(screenHeight);

        for (int i = 0; i < createdPos.Count; i++)
        {
            float originX = createdPos[i].x;
            float originY = createdPos[i].y;
            createdPos[i] = new Vector2(originX * screenWidth / screenBiasWidth, originY * screenHeight / screenBiasHeigth);
            pos_text.text += createdPos[i].ToString() + "\n";
        }

        arRaycastManager = GetComponent<ARRaycastManager>();
        //StartCoroutine(giveDelay());
    }
    void Update()
    {
        // 임의로 터치했을 때 생성되도록 하고 있음 
        // [TODO] GPS 적용해서 특정 반경 내에 들어왔다면 아래 생성문 실행되도록
        if (Input.touchCount == 0 || isRunningCoroutine)
        {
            return;
        }


        pos_text.text = Input.GetTouch(0).position.ToString();
        StartCoroutine(giveDelay());

    }

    void makePart()
    {
        if (arRaycastManager.Raycast(new Vector2(500f, 800f), hits, TrackableType.PlaneWithinPolygon))
        {
            var hitPose = hits[0].pose;
            int randObj = Random.Range(0, 2);
            Instantiate(Parts[randObj], hitPose.position, hitPose.rotation);
            pos_text.text = Input.GetTouch(0).position.ToString();
        }
    }

    IEnumerator giveDelay()
    {
        isRunningCoroutine = true;
        for (int j = 0; j < createdPos.Count; j++)
        {
            //makePart();
            if (arRaycastManager.Raycast(createdPos[j], hits, TrackableType.PlaneWithinPolygon))
            {
                var hitPose = hits[0].pose;
                int randObj = Random.Range(0, 2);
                Instantiate(Parts[randObj], hitPose.position, hitPose.rotation);
            }
            another_text.text = j.ToString() + "th alien";
            yield return new WaitForSeconds(1);
        }

        isRunningCoroutine = false;
    }
}
