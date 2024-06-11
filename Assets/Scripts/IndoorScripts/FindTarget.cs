using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindTarget : MonoBehaviour
{
    public GameObject findPopup;

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player") {
            findPopup.SetActive(true);
        }


    }
}
