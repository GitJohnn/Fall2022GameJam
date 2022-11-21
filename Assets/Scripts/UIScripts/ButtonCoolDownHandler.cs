using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ButtonCoolDownHandler : MonoBehaviour
{
    public float CurrentAbilityCooldown { get; set; }
    public float totalcooldown;

    [SerializeField] Image cooldownImage;

    private void Awake()
    {
        CurrentAbilityCooldown = totalcooldown;
    }

    // Update is called once per frame
    void Update()
    {
        cooldownImage.fillAmount = (CurrentAbilityCooldown / totalcooldown);
    }
}
