using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Slider slider;
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private PlayerHealth playerHealth;

    private float barSizeFactor;

    private void OnEnable() {
        playerStats.MaxHealthChanged += UpdateMaxHealth;
        playerHealth.PlayerHit += UpdateCurrentHealth;
    }

    private void OnDisable() {
        playerStats.MaxHealthChanged -= UpdateMaxHealth;
        playerHealth.PlayerHit -= UpdateCurrentHealth;
    }

    private void Start() {
        ResetBar();
    }

    private void ResetBar() {
        slider.maxValue = playerStats.MaxHealth;
        slider.value = slider.maxValue;

        barSizeFactor = rectTransform.rect.width / playerStats.MaxHealth;
    }

    private void UpdateMaxHealth(float newMaxHealth) {
        float newWidth = newMaxHealth * barSizeFactor;
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newWidth);

        float healthDifference = newMaxHealth - slider.maxValue;
        slider.maxValue = newMaxHealth;
        slider.value += healthDifference;
    }

    public void UpdateCurrentHealth(float newHealth) {
        StartCoroutine(LerpHealthBar(newHealth));
    }

    private IEnumerator LerpHealthBar(float newHealth) {
        float startingHealth = slider.value;
        float lerpDuration = 0.5f;
        float timeElapsed = 0;
        while(timeElapsed < lerpDuration) {
            slider.value = Mathf.Lerp(startingHealth, newHealth, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }
}