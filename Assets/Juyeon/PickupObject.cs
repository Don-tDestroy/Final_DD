using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PickupObject : MonoBehaviour
{
    public TextMeshProUGUI count_text;
    private RaycastHit hitInfo;
    public int getObjectCount;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            Ray screenRay = Camera.main.ScreenPointToRay(mousePos);

            if (Physics.Raycast(screenRay.origin, screenRay.direction * 1000f, out hitInfo))
            {
                if (hitInfo.collider.CompareTag("SPACESHIPPARTS"))
                {
                    hitInfo.collider.gameObject.SetActive(false);
                    getObjectCount++;

                    count_text.text = "part count : " + getObjectCount;
                }
            }
        }
    }
}
