using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class StatBarUIBase : MonoBehaviour {

	[SerializeField] protected PlayerStats playerStats;
	[SerializeField] protected RectTransform rectTransform;
	[SerializeField] private float barLerpDuration;

	protected float originalWidth;
	protected float currentWidth;
	protected float currentStatVal;

	protected abstract void ResetBar();

	private void Start() {
		if(rectTransform) {
			originalWidth = rectTransform.rect.width;
			currentWidth = originalWidth;
		}
		ResetBar();
	}

	protected virtual void UpdateBarWidth(float newStatVal) {
		float newWidthPercentage = newStatVal / currentStatVal;
		float newWidth = currentWidth * newWidthPercentage;
		StartCoroutine(LerpBarWidth(newWidth));
		currentStatVal = newStatVal;
	}

	private IEnumerator LerpBarWidth(float newWidth) {
		float startingWidth = currentWidth;
		float tempWidth;
		float timeElapsed = 0;
		while(timeElapsed < barLerpDuration) {
			tempWidth = Mathf.Lerp(startingWidth, newWidth, timeElapsed / barLerpDuration);
			rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, tempWidth);
			timeElapsed += Time.deltaTime;
			yield return null;
		}
		rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newWidth);
		currentWidth = newWidth;
	}
}