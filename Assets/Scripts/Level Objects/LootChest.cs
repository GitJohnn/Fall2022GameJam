using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using SoundSystem;

public class LootChest : TriggerEventHandler
{
    public KeyCode interactKeycode = KeyCode.I;
    public UnityEvent OnOpenChest;

    public bool CanOpen { get; set; } = true;

    [SerializeField] GameObject openParticles;
    private Vector3 _startPos;
    private float _timer;
    private Vector3 _randomPos;
    float _time = 0.5f;
    float _distance = 0.1f;
    float _delayBetweenShakes = 0f;
    [SerializeField] SFXEvent _sfxOpen;

    private void Update()
    {
        if (!CanOpen)
            return;

        if(isInsideTrigger && Input.GetKeyDown(interactKeycode))
        {
            Open();
        }
    }

    [Button]
    private void Open()
    {
        // TODO: This will be called by the player when they try to open a chest?
        CanOpen = false;
        _startPos = gameObject.transform.position;
        StartCoroutine(shake());
        _sfxOpen.Play();
        Utility.SpawnParticles(openParticles, gameObject, false);
        OnOpenChest?.Invoke();
    }

    IEnumerator shake()
    {
        _timer = 0f;

        while (_timer < _time)
        {
            _timer += Time.deltaTime;

            _randomPos = _startPos + (Random.insideUnitSphere * _distance);

            transform.position = _randomPos;

            if (_delayBetweenShakes > 0f)
            {
                yield return new WaitForSeconds(_delayBetweenShakes);
            }
            else
            {
                yield return null;
            }
        }

        transform.position = _startPos;
    }
}
