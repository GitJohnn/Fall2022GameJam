using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerDeath : MonoBehaviour
{
    [SerializeField] private AudioClip _deathAudio = null;

    public UnityEvent OnScreenFade;

    public void OnPlayerDeath()
    {
        OnScreenFade?.Invoke();
    }
}
