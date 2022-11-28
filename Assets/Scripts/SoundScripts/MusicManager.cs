using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{

    private static MusicManager _instance;
    public static MusicManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<MusicManager>();
                if (_instance == null)
                {
                    GameObject singletonGO = new GameObject("SFXManger_singleton");
                    _instance = singletonGO.AddComponent<MusicManager>();

                    DontDestroyOnLoad(singletonGO);
                }
            }
            return _instance;
        }
    }

    [SerializeField] AudioClip mainMenu;
    [SerializeField] AudioClip outOfCombat;
    [SerializeField] AudioClip inCombat;
    [SerializeField] AudioClip BossMuisc;

    [SerializeField] AudioSource source01;
    [SerializeField] AudioSource source02;

    bool isPlayerSource01;
    Coroutine fadeTrackRoutine;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        source01.clip = outOfCombat;
        source02.clip = BossMuisc;
        source01.Play();
        source02.Stop();
        isPlayerSource01 = true;

    }

    public void SwapTrack()
    {
       if (fadeTrackRoutine != null) StopCoroutine(fadeTrackRoutine);
       fadeTrackRoutine = StartCoroutine(FadeTrack());
    }

    IEnumerator FadeTrack()
    {
        float timeToFade = 0.25f;
        float timeElapsed = 0f;
        if (isPlayerSource01)
        {
            source02.Play();
            while (timeElapsed < timeToFade)
            {
                source02.volume = Mathf.Lerp(0, 0.1f, timeElapsed / timeToFade);
                source01.volume = Mathf.Lerp(0.1f, 0, timeElapsed / timeToFade);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            source01.Stop();
        }
        else
        {
            source01.Play();
            while (timeElapsed < timeToFade)
            {
                source01.volume = Mathf.Lerp(0, 0.1f, timeElapsed / timeToFade);
                source02.volume = Mathf.Lerp(0.1f, 0, timeElapsed / timeToFade);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            source02.Stop();
        }
    }
}
