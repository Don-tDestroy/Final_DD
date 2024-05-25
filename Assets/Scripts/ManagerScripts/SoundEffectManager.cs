using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[System.Serializable]
public struct SoundEffectInfo
{
    public string name;
    public AudioClip audioClip;
}

public class SoundEffectManager : MonoBehaviour
{
    private static SoundEffectManager _instance; // 싱글톤 객체
    public bool soundEffectOn = true; // true 이면 효과음 재생

    private AudioSource audioSource;     // audio source

    // audio clip 들 초기화. 실제 audio clip은 inspector 창에서 적용
    [SerializeField]
    private List<SoundEffectInfo> soundEffectClips = new List<SoundEffectInfo>();

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public static SoundEffectManager Instance
    {
        get
        {
            if (_instance == null)
            {
                return null;
            }
            return _instance;
        }
    }

    public void Play(int audioClipNum)
    {
        if (!soundEffectOn || soundEffectClips[audioClipNum].audioClip == null)
        {
            return;
        }
        audioSource.PlayOneShot(soundEffectClips[audioClipNum].audioClip);
    }

}