using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using TMPro;
using System.Runtime.CompilerServices;

public class SpaceshipPartsManager : MonoBehaviour
{
    private ARRaycastManager arRaycastManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    public double[] parts_lat;
    public double[] parts_long;

    public List<GameObject> parts = new List<GameObject>();

    public GameObject SpawnPoint;
    public TextMeshProUGUI myText;
    public bool[] isFirst;

    private void Awake()
    {
        arRaycastManager = GetComponent<ARRaycastManager>();
    }

    IEnumerator Start()
    {
        while (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            yield return null;
            Permission.RequestUserPermission(Permission.FineLocation);
        }

        if (!Input.location.isEnabledByUser)
            yield break;

        Input.location.Start(10, 1);

        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        if (maxWait < 1)
        {
            //myText.text = "Timed out";
            print("Timed out");
            yield break;
        }

        if (Input.location.status == LocationServiceStatus.Failed)
        {
            //myText.text = "Unable to determine device location";
            print("Unable to determine device location");
            yield break;
        }
        else
        {
            print("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);

            while (true)
            {
                yield return null;
                //text_ui.text = Input.location.lastData.latitude + " / " + Input.location.lastData.longitude;
            }
        }

    }

    private void checkandSpawnPart(double myLat, double myLong, double part_lat, double part_long, bool isFirst)
    {
        double remainDistance = distance(myLat, myLong, part_lat, part_long);
        myText.text = "the distance is = "+remainDistance;

        if (remainDistance <= 100f) // 10m..?
        {
            //Instantiate(arRaycastManager.raycastPrefab, SpawnPoint.transform.GetChild(0).transform.position, Quaternion.identity);
            Instantiate(arRaycastManager.raycastPrefab, SpawnPoint.transform.position, Quaternion.identity);
        }
    }

    void Update()
    {

        if (Input.location.status == LocationServiceStatus.Running)
        {
            double myLat = Input.location.lastData.latitude;
            double myLong = Input.location.lastData.longitude;

            for(int i=0; i<1; i++)
            {
                if (!isFirst[i])
                {
                    checkandSpawnPart(myLat, myLong, parts_lat[i], parts_long[i], isFirst[i]);
                    isFirst[i] = true;
                    myText.text = "working!!: "+i;
                }
            }   
        }
    }

    private double distance(double lat1, double lon1, double lat2, double lon2)
    {
        double theta = lon1 - lon2;

        double dist = Math.Sin(Deg2Rad(lat1)) * Math.Sin(Deg2Rad(lat2)) + Math.Cos(Deg2Rad(lat1)) * Math.Cos(Deg2Rad(lat2)) * Math.Cos(Deg2Rad(theta));

        dist = Math.Acos(dist);

        dist = Rad2Deg(dist);

        dist = dist * 60 * 1.1515;

        dist = dist * 1609.344; // ���� ��ȯ

        return dist;
    }

    private double Deg2Rad(double deg)
    {
        return (deg * Mathf.PI / 180.0f);
    }

    private double Rad2Deg(double rad)
    {
        return (rad * 180.0f / Mathf.PI);
    }
}
