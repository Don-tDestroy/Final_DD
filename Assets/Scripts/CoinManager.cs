using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public TextMeshProUGUI textUI;
    private RaycastHit hitInfo;
    public int getCoinCount;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            Ray screenRay = Camera .main.ScreenPointToRay(mousePos);

            if(Physics.Raycast(screenRay.origin, screenRay.direction * 1000f, out hitInfo))
            {
                if (hitInfo.collider.CompareTag("SPACESHIPPARTS"))
                {
                    hitInfo.collider.gameObject.SetActive(false);
                    getCoinCount++;

                    textUI.text = "part count : " + getCoinCount;
                }
            }
        }
    }
}
