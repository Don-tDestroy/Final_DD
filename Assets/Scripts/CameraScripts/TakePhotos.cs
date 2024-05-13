using System;
using System.Collections;
using System.IO;
using UnityEngine;

public class TakePhotos : MonoBehaviour
{
    // 사진 촬영 메서드
    public void TakePhoto()
    {
        if (Camera.main == null)
        {
            Debug.LogError("카메라를 찾을 수 없습니다 - TakePhoto()");
        }
        StartCoroutine(TakeAPhoto());
    }

    IEnumerator TakeAPhoto()
    {
        yield return new WaitForEndOfFrame();

        Camera camera = Camera.main;

        // 카메라에 Render Texture 설정
        int width = Screen.width;
        int height = Screen.height;
        RenderTexture rt = new RenderTexture(width, height, 24);
        camera.targetTexture = rt;

        // RenderTexture.active에 있는 Render Texture가 ReadPixels로 읽힐 Render Texture임
        var currentRT = RenderTexture.active;
        RenderTexture.active = rt;

        // 카메라의 뷰를 렌더링함
        camera.Render();

        // 새 텍스처를 생성하고 활성 Render Texture를 읽어들임
        Texture2D image = new Texture2D(width, height);
        image.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        image.Apply();

        camera.targetTexture = null;

        // 원래의 활성 Render Texture를 대체함
        RenderTexture.active = currentRT;

        // 사진 저장 관련 정보
        string fileName = DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png";
        string SavePath = Application.persistentDataPath;
        string dirName = "Exploring Ewha With Diddy";

        string galleryPath;
        if (SavePath.IndexOf("Android") > 0) // 안드로이드 플랫폼인 경우 DCIM 하위에 폴더를 생성
            galleryPath = Path.Combine(SavePath.Substring(0, SavePath.IndexOf("Android")) + "/DCIM/", dirName);
        else // 경로가 없을 때(PC 플레이 모드 등) 에러 발생 방지
            galleryPath = Path.Combine(Application.persistentDataPath, dirName);

        if (!Directory.Exists(dirName)) // 해당 디렉토리 없을 시 생성
        {
            Directory.CreateDirectory(galleryPath);
            Debug.Log("폴더를 생성하였습니다. 폴더 경로: " + galleryPath);
        }
        string filePath = Path.Combine(galleryPath, fileName);

        File.WriteAllBytes(filePath, image.EncodeToPNG());
        Debug.Log("사진이 촬영되었습니다. 파일 경로: " + filePath);

        Destroy(rt);
        Destroy(image);
    }
}