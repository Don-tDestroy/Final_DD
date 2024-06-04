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

    public void StartGame(string nextScene) // ���� ����
    {
        PlayerPrefs.SetInt("IsPrologueWatched", 1);
        SceneManager.LoadScene(nextScene); // ���� ������ �̵�
    }

    public void SeePrologue() // ���ѷα� ����: ���� �ٽú��� ��� �߰� �ÿ��� ȣ��
    {
        startPanel.SetActive(false);
        galleryPanel.SetActive(false);
        prologuePanel.SetActive(true);
    }
    
    public void SeeStart() // ���ѷα� ����: ���� �ٽú��� ��� �߰� �ÿ��� ȣ��
    {
        prologuePanel.SetActive(false);
        galleryPanel.SetActive(false);
        startPanel.SetActive(true);
    }

    public void SeeGallery() // ������ ����
    {
        startPanel.SetActive(false);
        prologuePanel.SetActive(false);
        galleryPanel.SetActive(true);
    }
}
