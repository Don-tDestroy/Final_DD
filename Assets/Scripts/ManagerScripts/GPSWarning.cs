using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPSWarning : MonoBehaviour
{
    public GameObject warningPopup;

    // Update is called once per frame
    void Update()
    {
        if(!GPSManager.Instance.GetIsSuccess() && !warningPopup.activeSelf)
        {
            warningPopup.SetActive(true);
        }
    }
}
