using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Target_Trigger : MonoBehaviour
{
    public TMP_Text text;

    private void OnTriggerEnter(Collider other)
    {
        text.text = other.name;


    }
}
