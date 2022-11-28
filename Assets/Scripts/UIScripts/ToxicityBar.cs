using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToxicityBar : StatBarUIBase {

    [SerializeField] private Slider slider;
	[SerializeField] private TextMeshProUGUI toxicityValueText;

    private void OnEnable() {
        playerStats.ToxicityChanged += UpdateToxicityBar;
    }

    private void OnDisable() {
        playerStats.ToxicityChanged -= UpdateToxicityBar;
    }

    protected override void ResetBar() {
        slider.maxValue = playerStats.ToxicityThreshold;
        slider.value = 0;
		toxicityValueText.text = $"0/{playerStats.ToxicityThreshold}";
    }

    private void UpdateToxicityBar(float toxicity) {
        StartCoroutine(LerpToxicityBar(toxicity));
		toxicityValueText.text = $"{toxicity}/{playerStats.ToxicityThreshold}";
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