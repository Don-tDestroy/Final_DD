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

    [SerializeField] private float maxSize = 1.4f; // 최대 크기
    [SerializeField] private float growSpeed = 4.0f; // 증가 속도
    [SerializeField] private float shrinkSpeed = 8.0f; // 감소 속도
    [SerializeField] private float destroyDelay = 1.1f; // 파괴까지의 지연 시간

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

    private IEnumerator PlayFixAnim()
    {

        StartCoroutine(AnimateScaleAndDestroy());

        yield return new WaitForSeconds(1f);

    }

    // 드래그 시작 위치 지정
    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        if (!canDrag)
        {
            return; // 드래그가 비활성화된 경우 이벤트를 무시
        }

        destPosition = sourceTr;

        transform.localScale = new Vector3(transform.localScale.x * 1.1f, transform.localScale.y * 1.1f, transform.localScale.z);

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

            StartCoroutine(PlayFixAnim());


            PuzzleManager.Instance.AnswerCntUp();
            PuzzleManager.Instance.CurrentCntUp();
            PuzzleManager.Instance.UpdateStagePullyCnt(stageIdx);

            // 부품이 드롭되면 해당 스테이지 슬롯 비활성화
            DisableDrag();

        }


        canvasGroup.blocksRaycasts = true;
    }

    private IEnumerator AnimateScaleAndDestroy()
    {
        float currentScale = transform.localScale.x; // 현재 크기 초기화
        bool growing = true; // 증가 중인지 여부

        while (true)
        {
            // 증가 및 감소 로직
            if (growing)
            {
                currentScale += Time.deltaTime * growSpeed;
                if (currentScale >= maxSize)
                {
                    growing = false;
                }
            }
            else
            {
                currentScale -= Time.deltaTime * shrinkSpeed;
                if (currentScale <= 0)
                {
                    Destroy(gameObject); // 오브젝트 파괴
                    yield break; // 코루틴 종료
                }
            }

            // 크기 적용
            transform.localScale = new Vector3(currentScale, currentScale, currentScale);

            yield return null; // 다음 프레임까지 대기
        }
    }
}
