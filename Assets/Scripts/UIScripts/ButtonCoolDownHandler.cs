using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ButtonCoolDownHandler : MonoBehaviour
{
    public float CurrentAbilityCooldown { get; set; }
    private float totalcooldown;

    [SerializeField] Image cooldownImage;
    [SerializeField] UnityEvent<float> OnUpdateCooldown;

    private void Awake()
    {
        CurrentAbilityCooldown = totalcooldown;
    }

    // Update is called once per frame
    void Update()
    {
        OnUpdateCooldown?.Invoke(CurrentAbilityCooldown);
        cooldownImage.fillAmount = (CurrentAbilityCooldown / totalcooldown);
    }

    public void SetTotalcooldown(float value)
    {
        totalcooldown = value;
        CurrentAbilityCooldown = totalcooldown;
    }
}
