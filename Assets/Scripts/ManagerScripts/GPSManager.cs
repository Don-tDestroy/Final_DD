using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Android;
using TMPro;

public class GPSManager : MonoBehaviour
{
    private static GPSManager _instance; // 싱글톤 객체
    
    public TextMeshProUGUI currGpsPosTxt; // (테스트용) 현재 GPS 위치

    public GameObject warningPopup; // GPS 연결 실패 시 띄우는 팝업

    private double currLatitude; // 현재 위도
    private double currLongitude; // 현재 경도

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }


    // GPS 동의 얻어오고 초기 세팅
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
            print("Timed out");
            OpenWarningPopup();
            yield break;
        }

        if (Input.location.status == LocationServiceStatus.Failed)
        {
            print("Unable to determine device location");
            OpenWarningPopup();
            yield break;
        }
        else
        {
            print("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);

            while (Input.location.status == LocationServiceStatus.Running)
            {
                yield return null;

                currLatitude = Input.location.lastData.latitude; // 현재 위도 저장
                currLongitude = Input.location.lastData.longitude; // 현재 경도 저장
                currGpsPosTxt.text = "현재 GPS 위치 \n" + "현재 위도: " + currLatitude + "\n현재 경도: " + currLongitude;
            }

            print("Unable to determine device location");
            OpenWarningPopup();
        }

        Input.location.Stop();
    }

    private void OpenWarningPopup()
    {
        SoundEffectManager.Instance.Play(1);
        warningPopup.SetActive(true);
    }


    // 싱글톤 객체 초기화
    public static GPSManager Instance
    {
        get
        {
            if (_instance == null)
            {
                return null;
            }
            return _instance;
        }
    }

    
    // 위도 얻어오기
    public double GetCurrLatitude()
    {
        return currLatitude;
    }


    // 경도 얻어오기
    public double GetCurrLongtitude()
    {
        return currLongitude;
    }


    // 현재 GPS가 목표 반경(radius) 이내에 들어오는지 확인하는 함수 (True / False 리턴)
    public bool CheckCurrPosInRadius(double targetLat, double targetLon, double targetRadius = 15f)
    {
        double remainDistance = GetDistance(currLatitude, currLongitude, targetLat, targetLon);

        return (remainDistance >= 0 && remainDistance <= targetRadius);
    }


    // 지표면 거리 계산 공식(하버사인 공식)
    public double GetDistance(double lat1, double lon1, double lat2, double lon2)
    {
        double theta = lon1 - lon2;

        double dist = Math.Sin(Deg2Rad(lat1)) * Math.Sin(Deg2Rad(lat2)) + Math.Cos(Deg2Rad(lat1)) * Math.Cos(Deg2Rad(lat2)) * Math.Cos(Deg2Rad(theta));

        dist = Math.Acos(dist);

        dist = Rad2Deg(dist);

        dist = dist * 60 * 1.1515;

        dist *= 1609.344; // 미터 변환

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