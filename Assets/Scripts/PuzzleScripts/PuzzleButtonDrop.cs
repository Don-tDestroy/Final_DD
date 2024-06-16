using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PuzzleButtonDrop : MonoBehaviour, IDropHandler
{
    void IDropHandler.OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        PuzzleButtonDrag buttonDrag = dropped.GetComponent<PuzzleButtonDrag>();
        buttonDrag.destPosition = transform;
    }
}
