using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GPSText : MonoBehaviour
{
    private TextMeshProUGUI gpsPos;
    private void Start()
    {
        gpsPos = GetComponent<TextMeshProUGUI>();

    }
    private void Update()
    {
        if (GPSManager.Instance.GetIsSuccess())
        {
            gpsPos.text = $"현재 GPS \n 위도 {GPSManager.Instance.GetCurrLatitude()} \n 경도 \n {GPSManager.Instance.GetCurrLongtitude()}";
        }
    }
}
