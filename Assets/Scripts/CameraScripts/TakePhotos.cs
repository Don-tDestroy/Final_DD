using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class TakePhotos : MonoBehaviour
{
    private ARCameraManager arCameraManager;
    private string galleryDirPath;
    // private Rect originalRect; // 원래 카메라의 rect 저장
    // private float targetAspect = 3f / 4f;
    private int targetWidth;
    private int targetHeight;

    void Start()
    {
        // 카메라에 Vuforia 스크립트 추가했는데 빌드본에서도 정상적으로 작동하는지 확인이 필요
        arCameraManager = FindObjectOfType<ARCameraManager>(); // Canvas에서 컴포넌트 가져오기
#if UNITY_EDITOR
        arCameraManager.requestedFacingDirection = CameraFacingDirection.User;
#elif UNITY_IOS || UNITY_ANDROID
        arCameraManager.requestedFacingDirection = CameraFacingDirection.World;
#endif
        galleryDirPath = PhotoGallery.getGalleryDirPath();
    }

    // 사진 촬영 메서드
    public void TakePhoto()
    {
        if (Camera.main == null)
        {
            Debug.LogError("카메라를 찾을 수 없습니다 - TakePhoto()");
        }
        StartCoroutine(TakeAPhoto());
    }

    /*public void SetCameraRect()
    {
        Camera camera = Camera.main;

        // 원래 화면의 가로세로 비율
        float screenAspect = (float)Screen.width / Screen.height;

        // 원래 카메라의 rect 저장
        // originalRect = camera.rect;


        // 화면 비율과 목표 비율을 비교하여 카메라 뷰포트를 조정
        if (screenAspect > targetAspect)
        
            float inset = 1.0f - screenAspect / targetAspect;
            camera.rect = new Rect(0.0f, inset / 2.0f, 1.0f, 1.0f - inset);            
            targetWidth = Screen.width;
            targetHeight = Mathf.RoundToInt(Screen.width / targetAspect);
        }
        else
        {
            float inset = 1.0f - targetAspect / screenAspect;
            camera.rect = new Rect(inset / 2.0f, 0.0f, 1.0f - inset, 1.0f);
            targetHeight = Screen.height;
            targetWidth = Mathf.RoundToInt(Screen.height * targetAspect);
        }
    }*/
    IEnumerator TakeAPhoto()
    {
        yield return new WaitForEndOfFrame();

        Camera camera = Camera.main;
        camera.rect = new Rect(0, 0, 1, 1);

        // 카메라에 Render Texture 설정
        RenderTexture rt = new RenderTexture(targetWidth, targetHeight, 24);
        camera.targetTexture = rt;

        // RenderTexture.active에 있는 Render Texture가 ReadPixels로 읽힐 Render Texture임
        var currentRT = RenderTexture.active;
        RenderTexture.active = rt;

        // 카메라의 뷰를 렌더링함
        camera.Render();

        // 새 텍스처를 생성하고 활성 Render Texture를 읽어들임
        Texture2D image = new Texture2D(targetWidth, targetHeight, TextureFormat.RGB24, false);
        image.ReadPixels(new Rect(0, 0, targetWidth, targetHeight), 0, 0);
        image.Apply();


        // 3:4 비율의 새로운 텍스처 크기 계산
       /*int cropWidth = targetWidth;
        int cropHeight = (targetWidth * 4) / 3;
        if (cropHeight > targetHeight)
        {
            cropHeight = targetHeight;
            cropWidth = (targetHeight * 3) / 4;
        }

        // 중앙 기준으로 자를 영역 계산
        int startX = (targetWidth - cropWidth) / 2;
        int startY = (targetHeight - cropHeight) / 2;

        // 새로운 텍스처 생성 및 픽셀 데이터 복사
        Texture2D croppedImage = new Texture2D(cropWidth, cropHeight, TextureFormat.RGB24, false);
        Color[] pixels = image.GetPixels(startX, startY, cropWidth, cropHeight);
        croppedImage.SetPixels(pixels);
        croppedImage.Apply();*/
        
        camera.targetTexture = null;
        // 원래의 활성 Render Texture를 대체함
        RenderTexture.active = currentRT;


        // 사진 파일 이름, 경로
        string fileName = DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png";
        string filePath = Path.Combine(galleryDirPath, fileName);

        File.WriteAllBytes(filePath, image.EncodeToPNG());
        Debug.Log("사진이 촬영되었습니다. 파일 경로: " + filePath);

        // 카메라의 원래 rect로 복원
        // camera.rect = originalRect;
        camera.rect = new Rect(0, 0, 1, 1);

        Destroy(rt);
        Destroy(image);
    }
}