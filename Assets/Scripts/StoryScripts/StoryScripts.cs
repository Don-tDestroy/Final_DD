using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryScripts : MonoBehaviour
{
    public List<List<string>> Lines = new List<List<string>>()
    {
        new List<string>()
        {
            "오늘은 드디어 설레는 개강일!",
            "공대를 가는 방법은 잘 모르겠지만 일단 정문에 도착했다!",
            "그런데... 뭔가 이상한데?",
            "어디선가... 우는 소리 들리지 않아?"
        },
        new List<string>()
        {
            "엥??? 이 작은 친구는 누구야? 다른 사람들은 얘가 안 보이나?",
            "엉엉엉...",
            "너... 괜찮아?",
            "아니... 안 괜찮아...",
            "무슨 일인데? 내가 도와줄게.",
            "정말?",
            "응, 그럼.",
            "사실은... 난 우주선을 타고 우주 곳곳을 여행하고 있었어. 엄마 없이 나 혼자서는 처음 여행하는 거라 엄청 신났었거든...",
            "그런데 갑자기 우주선이 고장이 나면서 부서져 버린 거야.",
            "저어기, 위에 있는 건물에 추락해버려서 날 도와줄 사람을 찾았는데, 아무도 없어서...",
            "...결국 여기까지 와버렸어...",
            "(저기 위쪽 건물이라면... 공대 건물이려나?)",
            "저기, 울지 마. 나도 어차피 그쪽으로 가는 길이었어!",
            "우리 같이 가볼까?",
            "정말? 너무 고마워... 넌 내 은인이야."
        },
        new List<string>()
        {
            "결국, 디디의 우주선을 수리해주는 건 실패해버렸다.",
            "디디는 나에게 여기까지 도와준 것만으로도 고맙다는 인사를 전하며...",
            "다른 사람에게 부탁하겠다고 이야기하고선 떠나갔다.",
            "디디와 소중한 추억을 쌓을 수 있었던 건 좋았지만...",
            "집으로 잘 돌아갔을까, 작은 외계인 디디?"
        }
    };

    public List<List<int>> Emotions = new List<List<int>>()
    {
        new List<int>() {0,0,0,0},
        new List<int>() {7,7,7,4,4,0,0,5,5,4,7,7,0,0,2},
        new List<int>() {0,0,0,0,0}
    };

    public List<List<int>> Names = new List<List<int>>()
    {
        new List<int>() {0,0,0,0},
        new List<int>() {0,1,0,1,0,1,0,1,1,1,1,0,0,0,1},
        new List<int>() { 0, 0, 0, 0, 0 }
    };

}
