using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class PuzzleButtonDrag: MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector]
    public Transform destPosition; // 드래그 될 위치
    public Transform answerBtn; // 정답과 일치하는 버튼
    private Transform sourceTr; // 이동될 UI
    //private Transform originParent; // 원래 부모
    private CanvasGroup canvasGroup; // 상호작용 제어를 위한

    private Vector3 originPosition; // 원래 위치
    private Vector3 originScale; // 원래 크기
    private Vector2 startingPoint;
    private Vector2 moveBegin;
    private Vector2 moveOffset;

    [SerializeField]
    private int stageIdx;


    private void Awake()
    {
        sourceTr = this.transform;
        destPosition = this.transform;
        //originParent = this.transform.parent.transform;
        originPosition = this.transform.localPosition;
        originScale = this.transform.localScale;
        canvasGroup = this.GetComponent<CanvasGroup>();
        EnableDrag();
    }

    public void ObjInit()
    {
        sourceTr = this.transform;
        destPosition = this.transform;
    }

    public bool canDrag = true; // 드래그 가능 여부를 나타내는 플래그


    // 드래그를 비활성화하고 싶을 때
    public void DisableDrag()
    {
        canDrag = false;
    }

    // 드래그를 다시 활성화하고 싶을 때
    public void EnableDrag()
    {
        canDrag = true;
    }

    // 드래그 시작 위치 지정
    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        if (!canDrag)
        {
            return; // 드래그가 비활성화된 경우 이벤트를 무시
        }

        //if (destPosition == answerBtn)
        //{
        //    PuzzleManager.Instance.AnswerCntDown();
        //}
        //if (destPosition != sourceTr)
        //{
        //    PuzzleManager.Instance.CurrentCntDown();
        //}
        destPosition = sourceTr;

        transform.localScale = new Vector3(transform.localScale.x * 1.1f, transform.localScale.y * 1.1f, transform.localScale.z);
        //transform.SetParent(originParent);
        //transform.SetAsLastSibling();

        moveBegin = eventData.position;
        startingPoint = transform.localPosition;

        canvasGroup.blocksRaycasts = false;
        SoundEffectManager.Instance.Play(0);
    }

    // 드래그 : 마우스 커서 위치로 이동
    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        if (!canDrag)
        {
            return; // 드래그가 비활성화된 경우 이벤트를 무시
        }
        moveOffset = eventData.position - moveBegin;
        sourceTr.localPosition = startingPoint + moveOffset;
    }

    // 드래그 : 마우스 뗐을 때
    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        if (!canDrag)
        {
            return; // 드래그가 비활성화된 경우 이벤트를 무시
        }

        Debug.Log("마우스뗌");
        if (destPosition == sourceTr)
        {
            transform.localPosition = originPosition;
            transform.localScale = originScale;
        }
        else
        {
            transform.localPosition = destPosition.localPosition;
            transform.localScale = destPosition.localScale;

            PuzzleManager.Instance.AnswerCntUp();
            PuzzleManager.Instance.CurrentCntUp();
            PuzzleManager.Instance.UpdateStagePullyCnt(stageIdx);

            // 부품이 드롭되면 해당 스테이지 슬롯 비활성화
            DisableDrag();

            // 좀 있다가 자기 자리로
            transform.localPosition = originPosition;
            transform.localScale = originScale;
            Debug.Log("비활성화");

        }

        ////transform.SetParent(destPosition);

        //if (destPosition == answerBtn)
        //{


        //}


        canvasGroup.blocksRaycasts = true;
    }
}
