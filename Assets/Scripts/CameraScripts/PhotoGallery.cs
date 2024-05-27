using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class PhotoGallery : MonoBehaviour
{
    public GameObject photoPrefab; // 사진을 표시할 프리팹 (UI 이미지나 3D 오브젝트)
    public Transform galleryContainer; // 사진을 배치할 부모 객체
    private string galleryDirPath;

    void Start()
    {
        galleryDirPath = getGalleryDirPath();
        LoadPhotos();
    }

    public static string getGalleryDirPath()
    {
        string savePath = Application.persistentDataPath;
        string dirName = "Exploring Ewha With Diddy";
        string dirPath;

        if (savePath.IndexOf("Android") > 0) // 안드로이드 플랫폼인 경우 DCIM 하위에 폴더를 생성
            dirPath = Path.Combine(savePath.Substring(0, savePath.IndexOf("Android")) + "/DCIM/", dirName);
        else // 다른 플랫폼이어서 경로가 없을 때(PC 플레이 모드 등) 에러 발생 방지
            dirPath = Path.Combine(savePath, dirName);
        if (!Directory.Exists(dirPath)) // 해당 디렉토리 없을 시 생성
        {
            Directory.CreateDirectory(dirPath);
            Debug.Log("폴더를 생성하였습니다. 폴더 경로: " + dirPath);
        }
        return dirPath;
    }

    // 사진 로드 및 갤러리에 추가
    public void LoadPhotos()
    {
        if (!Directory.Exists(galleryDirPath))
        {
            Debug.LogError("갤러리 경로를 찾을 수 없습니다. galleryDirPath: " + galleryDirPath);
            return;
        }
        string[] files = Directory.GetFiles(galleryDirPath, "*.png");
        if (files == null || files.Length == 0)
        {
            Debug.LogWarning("갤러리에 저장된 사진이 없습니다. galleryDirPath: " + galleryDirPath);
            return;
        }
        foreach (string filePath in files)
        {
            StartCoroutine(LoadPhoto(filePath));
        }
        Debug.Log("갤러리에서 사진을 로드 완료했습니다.");
    }

    private IEnumerator LoadPhoto(string filePath)
    {
        byte[] fileData = File.ReadAllBytes(filePath);
        Texture2D texture = new Texture2D(2, 2);
        if (texture.LoadImage(fileData))
        {
            yield return new WaitForEndOfFrame();
            AddPhotoToGallery(texture);
        }
        else
        {
            Debug.LogError("사진 로드 실패: " + filePath);
        }
    }

    private void AddPhotoToGallery(Texture2D texture)
    {
        GameObject photoObject = Instantiate(photoPrefab, galleryContainer);

        // photoObject의 RectTransform 가져와서 비율을 조정
        /*RectTransform rectTransform = photoObject.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            float originalWidth = rectTransform.sizeDelta.x;
            float newHeight = originalWidth * Screen.height / Screen.height;
            rectTransform.sizeDelta = new Vector2(originalWidth, newHeight);
        }
        else
        {
            Debug.LogError("photoPrefab에 RectTransform 컴포넌트가 없습니다.");
        }*/

        // UI 이미지에 텍스처 설정
        Image imageComponent = photoObject.GetComponent<Image>();
        if (imageComponent != null)
        {
            imageComponent.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            Debug.Log("이미지에 텍스처를 추가했습니다.");
        }
        else
        {
            Debug.LogError("photoPrefab에 UI Image 컴포넌트가 없습니다.");
        }
    }
}
