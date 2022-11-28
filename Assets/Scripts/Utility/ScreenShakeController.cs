using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShakeController : MonoBehaviour
{
    public static ScreenShakeController Instance { get; private set; }
    private float _shakeTimeRemaining, _shakePower, _shakeFadeTime, _shakeRotation;

    [SerializeField] float _rotationMultiplier = 15f;

    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    void LateUpdate()
    {
        if (_shakeTimeRemaining > 0)
        {
            _shakeTimeRemaining -= Time.deltaTime;

            float xAmount = Random.Range(-1f, 1f) * _shakePower;
            float yAmount = Random.Range(-1f, 1f) * _shakePower;

            transform.position += new Vector3(xAmount, yAmount, 0);

            _shakePower = Mathf.MoveTowards(_shakePower, 0, _shakeFadeTime * Time.deltaTime);

            _shakeRotation = Mathf.MoveTowards(_shakeRotation, 0, _shakeFadeTime * _rotationMultiplier * Time.deltaTime);
        }   

        transform.rotation = Quaternion.Euler(0f, 0f, _shakeRotation * Random.Range(-1f, 1f)); 
    }

    [Button]
    public void StartShake(float length, float power)
    {
        _shakeTimeRemaining = length;
        _shakePower = power;

        _shakeFadeTime = power / length;
        _shakeRotation = power * _rotationMultiplier;
    }
}
