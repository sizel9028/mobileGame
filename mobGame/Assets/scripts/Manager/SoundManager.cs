using UnityEngine;


// 사운드 관리
public class SoundManager : MonoBehaviour
{
    public static SoundManager soundManager;
    private AudioSource bgmSource;

    [Header("BGM Clip")]
    public AudioClip mapBGM;
    public AudioClip battleBGM;
    public AudioClip bossBGM;

    public enum BGMType
    {
        Map, Battle, EliteBattle
    }

    void Awake()
    {
        if (soundManager != null)
        {
            Destroy(gameObject);
            return;
        }

        soundManager = this;
        DontDestroyOnLoad(gameObject);

        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.loop = true;
        bgmSource.playOnAwake = true;
        bgmSource.clip = mapBGM;      // 기본은 맵 브금
        bgmSource.Play();
    }

    public void PlayBGM(BGMType type)
    {
        AudioClip clipToPlay = null;

        switch (type)
        {
            case BGMType.Map:
                clipToPlay = mapBGM;
                break;
            case BGMType.Battle:
                clipToPlay = battleBGM;
                break;
            case BGMType.EliteBattle:
                clipToPlay = bossBGM;
                break;
        }

        if (clipToPlay == null) return;
        if (bgmSource.clip == clipToPlay && bgmSource.isPlaying) return;

        bgmSource.clip = clipToPlay;
        bgmSource.Play();
    }

    //hit sound는 몹별로 아마 같게 통일을 할듯?
    public void GetHitSound()
    {
        //TODO hit sound 틀어주기
    }

}
