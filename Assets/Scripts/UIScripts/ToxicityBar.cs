using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToxicityBar : MonoBehaviour {

    [SerializeField] private Slider slider;
    [SerializeField] private PlayerStats playerStats;

    private void OnEnable() {
        playerStats.ToxicityChanged += UpdateToxicityBar;
    }

    private void OnDisable() {
        playerStats.ToxicityChanged -= UpdateToxicityBar;
    }

    private void Start() {
        ResetBar();
    }

    private void ResetBar() {
        slider.maxValue = playerStats.ToxicityThreshold;
        slider.value = 0;
    }

    private void UpdateToxicityBar(float toxicity) {
        StartCoroutine(LerpToxicityBar(toxicity));
    }

    private IEnumerator LerpToxicityBar(float newToxicity) {
        float startingToxicity = slider.value;
        float lerpDuration = 0.75f;
        float timeElapsed = 0;
        while(timeElapsed < lerpDuration) {
            slider.value = Mathf.Lerp(startingToxicity, newToxicity, timeElapsed/lerpDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }
}