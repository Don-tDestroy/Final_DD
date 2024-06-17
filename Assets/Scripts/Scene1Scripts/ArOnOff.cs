using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ArOnOff : MonoBehaviour
{
    public ARPlaneManager _arPlaneManager;
    public ARPointCloudManager _arPointCloudManager;

    private void Start()
    {
        _arPlaneManager = GetComponent<ARPlaneManager>();
        _arPointCloudManager = GetComponent<ARPointCloudManager>();
    }


    public void OnAr()
    {
        _arPlaneManager.enabled = true;
        _arPointCloudManager.enabled = true;
    }

    public void OffAr()
    {
        foreach (var plane in _arPlaneManager.trackables)
        {
            plane.gameObject.SetActive(false);
        }
        foreach (var plane in _arPointCloudManager.trackables)
        {
            plane.gameObject.SetActive(false);
        }


        _arPlaneManager.enabled = false;
        _arPointCloudManager.enabled = false;

    }
}
