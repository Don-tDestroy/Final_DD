using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene0Initialize : MonoBehaviour
{
    public GameObject prologuePanel;
    public GameObject startPanel;
    public GameObject galleryPanel;

    void Start()
    {
        int isOpening = PlayerPrefs.GetInt("IsPrologueWatched");
        if (isOpening == 0)
        {
            SeePrologue();
        }
        else
        {
            SeeStart();
        }
    }

    public void StartGame() // 게임 시작
    {
        PlayerPrefs.SetInt("IsPrologueWatched", 1);
        SceneManager.LoadScene("Scene_1"); // 다음 씬으로 이동
    }

    public void SeePrologue() // 프롤로그 보기: 추후 다시보기 기능 추가 시에도 호출
    {
        startPanel.SetActive(false);
        galleryPanel.SetActive(false);
        prologuePanel.SetActive(true);
    }
    
    public void SeeStart() // 프롤로그 보기: 추후 다시보기 기능 추가 시에도 호출
    {
        prologuePanel.SetActive(false);
        galleryPanel.SetActive(false);
        startPanel.SetActive(true);
    }

    public void SeeGallery() // 갤러리 보기
    {
        startPanel.SetActive(false);
        prologuePanel.SetActive(false);
        galleryPanel.SetActive(true);
    }
}
