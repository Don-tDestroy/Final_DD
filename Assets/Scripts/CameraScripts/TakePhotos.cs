using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class TakePhotos : MonoBehaviour
{
    private ARCameraManager arCameraManager;
    private string galleryDirPath;
    private int targetWidth;
    private int targetHeight;
    private LayerMask originalCullingMask;
    public AudioSource audioSource;

    void Start()
    {
        arCameraManager = FindObjectOfType<ARCameraManager>(); // Canvas에서 컴포넌트 가져오기
        arCameraManager.requestedFacingDirection = CameraFacingDirection.User;
        galleryDirPath = PhotoGallery.getGalleryDirPath();
        targetWidth = Screen.width;
        targetHeight = Screen.height;
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

    IEnumerator TakeAPhoto()
    {
        yield return new WaitForEndOfFrame();
        audioSource.Play();

        Camera camera = Camera.main;

        // Save the original culling mask
        originalCullingMask = camera.cullingMask;

        // Exclude the CameraUI layer
        int cameraUILayer = LayerMask.NameToLayer("CameraUI");
        camera.cullingMask = originalCullingMask & ~(1 << cameraUILayer);

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
        
        camera.targetTexture = null;
        RenderTexture.active = currentRT;

        // Restore the original culling mask
        camera.cullingMask = originalCullingMask;

        // 사진 파일 이름, 경로
        string fileName = DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png";
        string filePath = Path.Combine(galleryDirPath, fileName);

        File.WriteAllBytes(filePath, image.EncodeToPNG());
        Debug.Log("사진이 촬영되었습니다. 파일 경로: " + filePath);

        Destroy(rt);
        Destroy(image);
    }
}