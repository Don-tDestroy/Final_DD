using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitManager : MonoBehaviour
{
    public GameObject quitPopup;
    int ClickCount = 0;

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ClickCount++;
            if (!IsInvoking("DoubleClick"))
                Invoke("DoubleClick", 1.0f);

        }
        else if (ClickCount == 2)
        {
            CancelInvoke("DoubleClick");
            SoundEffectManager.Instance.Play(1);
            quitPopup.SetActive(true);
        }

    }

    void DoubleClick()
    {
        ClickCount = 0;
    }
}
