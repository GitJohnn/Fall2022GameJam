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
    }
}
