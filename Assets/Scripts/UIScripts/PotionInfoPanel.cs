using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PotionInfoPanel : MonoBehaviour {

    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TextMeshProUGUI buffText;
    [SerializeField] private TextMeshProUGUI debuffText;

    private bool panelVisible;

	private void Awake() {
		canvasGroup.alpha = 0;
	}

	public void InitializePanel(PotionBase potion) {
        buffText.color = Color.green;
        debuffText.color = Color.red;

        buffText.text = $"{potion.StatToBuff} +{potion.BuffPercentage}%";
        debuffText.text = $"{potion.StatToDebuff} -{potion.DebuffPercentage}%";
    }

	public void ShowPanel() {
		if(panelVisible || !gameObject.activeInHierarchy) return;
		panelVisible = true;
		StartCoroutine(LerpPanelAlpha(1));
	}

	public void HidePanel() {
		if(!panelVisible || !gameObject.activeInHierarchy) return;
		panelVisible = false;
		StartCoroutine(LerpPanelAlpha(0));
	}

	private IEnumerator LerpPanelAlpha(float newAlpha) {
		float startingAlpha = canvasGroup.alpha;
		float lerpDuration = Mathf.Abs(startingAlpha - newAlpha) * 0.5f;
		float timeElapsed = 0;
		while(timeElapsed < lerpDuration) {
			canvasGroup.alpha = Mathf.Lerp(startingAlpha, newAlpha, timeElapsed / lerpDuration);
			timeElapsed += Time.deltaTime;
			yield return null;
		}
	}
}