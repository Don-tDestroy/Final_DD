using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using TMPro;

public class ARObjectOnPlane : MonoBehaviour
{
    private ARRaycastManager arRaycastManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private float screenBiasWidth = 1440;
    private float screenBiasHeigth = 2560;

    private List<Vector2> createdPos = new List<Vector2>() { new Vector2(700, 2400), new Vector2(700, 2100), new Vector2(700, 1800) };

    public TextMeshProUGUI debugTxt;


    // Start is called before the first frame update
    private void Awake()
    {
        arRaycastManager = GetComponent<ARRaycastManager>();

        float screenWidth = Screen.width;
        float screenHeight = Screen.height;
        Debug.Log(screenWidth);
        Debug.Log(screenHeight);

        for (int i = 0; i < createdPos.Count; i++)
        {
            float originX = createdPos[i].x;
            float originY = createdPos[i].y;
            createdPos[i] = new Vector2(originX * screenWidth / screenBiasWidth, originY * screenHeight / screenBiasHeigth);
            debugTxt.text += createdPos[i].ToString() + "\n";
        }

    }

    // Update is called once per frame
    void Update()
    {
        // 임의로 터치했을 때 생성되도록 하고 있음 
        // [TODO] GPS 적용해서 특정 반경 내에 들어왔다면 아래 생성문 실행되도록
        if (Input.touchCount == 0)
        {
            return;
        }

        // 디디 생성
        float x = Input.GetTouch(0).position.x;
        float y = Input.GetTouch(0).position.y;
        debugTxt.text = Input.GetTouch(0).position.ToString() + "\n";

        for (int i = 0; i < createdPos.Count; i++)
        {
            if (arRaycastManager.Raycast(createdPos[i], hits, TrackableType.PlaneWithinPolygon))
            {
                var hitPose = hits[0].pose;
                GameObject gameObj = Instantiate(arRaycastManager.raycastPrefab, hitPose.position, hitPose.rotation);
                debugTxt.text += gameObj.transform.parent.name;
            }
        }
    }
}
