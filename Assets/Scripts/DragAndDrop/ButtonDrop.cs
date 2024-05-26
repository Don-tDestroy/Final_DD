using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonDrop : MonoBehaviour, IDropHandler
{
    void IDropHandler.OnDrop(PointerEventData eventData)
    {
        Debug.Log("drop");
        GameObject dropped = eventData.pointerDrag;
        ButtonDrag buttonDrag = dropped.GetComponent<ButtonDrag>();
        buttonDrag.destPosition = transform;
    }
}
