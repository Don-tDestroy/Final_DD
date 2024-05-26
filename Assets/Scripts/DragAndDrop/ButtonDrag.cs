using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonDrag : MonoBehaviour, IPointerDownHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector]
    public Transform destPosition; // 드래그 될 위치
    public Transform answerBtn; // 정답과 일치하는 버튼
    private Transform sourceTr; // 이동될 UI

    private Transform originParent; // 원래 부모
    private Vector2 originPosition;
    private Vector2 startingPoint;
    private Vector2 moveBegin;
    private Vector2 moveOffset;

    private void Awake()
    {
        sourceTr = this.transform;
        destPosition = this.transform;
        originParent = this.transform.parent.transform;
        originPosition = transform.localPosition;
    }

    public void ObjInit()
    {
        sourceTr = this.transform;
        destPosition = this.transform;
    }

    // 드래그 시작 위치 지정
    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        if (destPosition == answerBtn)
        {
            QuizManager.Instance.AnswerCntDown();
        }
        if (destPosition != sourceTr)
        {
            QuizManager.Instance.CurrentCntDown();
        }
        moveBegin = eventData.position;
        GetComponent<Image>().raycastTarget = false;
        destPosition = sourceTr;
        transform.SetParent(originParent);
        transform.SetAsFirstSibling();
        startingPoint = transform.localPosition;
    }

    // 드래그 : 마우스 커서 위치로 이동
    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        moveOffset = eventData.position - moveBegin;
        sourceTr.localPosition = startingPoint + moveOffset;
    }

    // 드래그 : 마우스 뗐을 때
    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("마우스뗌");
        if(destPosition == sourceTr)
        {
            transform.localPosition = originPosition;
        }
        else
        {
            transform.localPosition = destPosition.localPosition;
            transform.SetParent(destPosition);
            if(destPosition == answerBtn)
            {
                QuizManager.Instance.AnswerCntUp();
            }
            QuizManager.Instance.CurrentCntUp();
        }
        GetComponent<Image>().raycastTarget = true;
    }
}
