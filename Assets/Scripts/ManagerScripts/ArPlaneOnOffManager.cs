using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


public class ArPlaneOnOffManager : MonoBehaviour
{
    private ARPlaneManager _arPlaneManager;

    [SerializeField]
    private Material transparent;
    [SerializeField]
    private Material visual;

    private void Start()
    {
        _arPlaneManager = GetComponent<ARPlaneManager>();
    }

    private void HideShowArPlane(bool isOn)
    {
        foreach (var plane in _arPlaneManager.trackables)
        {
            if (isOn)
            {
                plane.gameObject.GetComponent<MeshRenderer>().material = visual;
            }
            else
            {
                plane.gameObject.GetComponent<MeshRenderer>().material = transparent;
            }
        }
    }

    public void OnClickHideShowArPlane(bool isOn)
    {
        HideShowArPlane(isOn);
    }
}
