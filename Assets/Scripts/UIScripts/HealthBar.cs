using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : StatBarUIBase {

    [SerializeField] private Slider slider;
    [SerializeField] private PlayerHealth playerHealth;

    private void OnEnable() {
        playerStats.MaxHealthChanged += UpdateMaxHealth;
        playerHealth.PlayerHit += UpdateCurrentHealth;
    }

    private void OnDisable() {
        playerStats.MaxHealthChanged -= UpdateMaxHealth;
        playerHealth.PlayerHit -= UpdateCurrentHealth;
    }

    protected override void ResetBar() {
        slider.maxValue = playerStats.MaxHealth;
        slider.value = slider.maxValue;

		currentStatVal = playerStats.MaxHealth;
    }

    private void UpdateMaxHealth(float newMaxHealth) {
		UpdateBarWidth(newMaxHealth);

        float healthDifference = newMaxHealth - slider.maxValue;
        slider.maxValue = newMaxHealth;
        if(healthDifference > 0) slider.value += healthDifference;
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