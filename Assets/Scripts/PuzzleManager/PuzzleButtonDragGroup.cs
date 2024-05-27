using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PuzzleButtonDragGroup : MonoBehaviour
{
    public TextMeshProUGUI pullyCntText; // 부품 남은 개수 표시

    public void setPullyCntText(int cnt)
    {
        pullyCntText.text = string.Format("x {0:D2}", cnt);
    }

    public void SetPullySuccessText()
    {
        pullyCntText.text = "완료!";
    }
}
