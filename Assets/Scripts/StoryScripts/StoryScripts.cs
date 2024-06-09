using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryScripts : MonoBehaviour
{
    public List<List<string>> Lines = new List<List<string>>()
    {
        new List<string>()
        {
            "테스트 스크립트!테스트 스크립트!테스트 스크립트!테스트테스트 스크립트!테스트 스크립트!테스트 스크립트!테스트 스크립트!테스트 스크립트!",
            "디디의 대사야.",
            "안녕! 난 외계인 디디."
        },
        new List<string>()
        {
            "테스트 스크립트!테스트 스크립트!테스트 스크립트!테스트테스트 스크립트!테스트 스크립트!테스트 스크립트!테스트 스크립트!테스트 스크립트!",
            "디디의 대사야.",
            "안녕! 난 외계인 디디."
        },
        new List<string>()
        {
            "테스트 스크립트!테스트 스크립트!테스트 스크립트!테스트테스트 스크립트!테스트 스크립트!테스트 스크립트!테스트 스크립트!테스트 스크립트!",
            "디디의 대사야.",
            "안녕! 난 외계인 디디."
        }
    };

    public List<List<int>> Emotions = new List<List<int>>()
    {
        new List<int>() {7, 7, 7},
        new List<int>() {1, 2, 3},
        new List<int>() {1, 2, 3}
    };

    public List<List<int>> Names = new List<List<int>>()
    {
        new List<int>() {0, 1, 0},
        new List<int>() {0, 1, 1},
        new List<int>() {1, 1, 1}
    };

}
