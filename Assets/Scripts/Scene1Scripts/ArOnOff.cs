using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ArOnOff : MonoBehaviour
{
    public GameObject arPlane;
    public GameObject arPoint;

    public void OnAr()
    {
        arPlane.SetActive(true);
        arPoint.SetActive(true);
}

    public void OffAr()
    {
        arPlane.SetActive(false);
        arPoint.SetActive(false);
    }
}
