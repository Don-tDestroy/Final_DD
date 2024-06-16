using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroDataScript : MonoBehaviour
{
    Dictionary<int, string[]> introData;

    void Awake()
    {
        introData = new Dictionary<int, string[]>();
        GenerateData();
    }

    void GenerateData()
    {
        introData.Add(2, new string[] { "이화여대를 대표하는 건물인 ECC에 도착했어!", "ECC는 세계적으로 유명한 프랑스 건축가 도미니크 페로가 지은 복합단지야.", "수업을 듣는 강의실 뿐만 아니라 예쁜 스탠드 불이 새어나오는 열람실, 편하게 공부하고 먹고 심지어 누워 잘 수도 있는 잉여 계단, 교보문고, 피트니스 센터 등 복합단지라는 이름에 걸맞게 다양한 시설들이 있으니 꼭 이용해봐!", "아, 길을 잃지 않게 조심해~" });
        introData.Add(3, new string[] { "휴! 언덕길을 오르느라 힘들었지? 여기는 사회과학대학 건물인 포스코관이야! 줄여서 포관이라고 많이 부르는 곳이지~", "건물이 익숙하다고? 맞아. 이 포스코관에서 학부 입학 수시 면접과 논술 시험이 이루어지지. 예비 이화인이 아닌 진정한 이화인으로서 이 건물에 다시 서게 된 것을 축하해!", "배가 고플 때는 지하 1층에 꼭 가보도록 해. 각종 도시락과 빵, 커피로 배를 채울 수 있다구~"});
        introData.Add(4, new string[] { "이곳은 종합과학관! 자연과학대학 대학원생들이 많이 이용하는 건물이야.", "A동부터 D동까지 있는데, 서로 다 연결되어 있어. 특정 동을 찾는다면 아무 입구로 들어가서 건물 내에 친절하게 표시된 안내를 따라 찾으면 돼!", "이곳에도 강의실이 있긴 하지만 거의 다 실험실, 연구실이어서 수업을 들을 일이 많지는 않을 거야." });
        introData.Add(5, new string[] { "마지막 목적지인 공대에 도착했어!", "바로 앞에 보이는 건물이 아산공학관이고, 그 뒤에 있는 건물이 신공학관이야. 두 건물이 연결되어 있어서 아무 곳으로나 들어가면 돼.", "공대는 24시간 열려있고, 그래서 그런지 2층에 있는 공학도서관에서 많은 공대 벗들이 밤을 새곤 해.", "지하 2층에서는 학식도 먹을 수 있어." });
    }

    public string[] GetData(int stageNum)
    {
        return introData[stageNum];
    }
}
