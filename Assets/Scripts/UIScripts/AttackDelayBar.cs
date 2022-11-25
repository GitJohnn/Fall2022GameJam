using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDelayBar : StatBarUIBase {

	private void OnEnable() {
		playerStats.AttackDelayChanged += UpdateBarWidth;
	}

	private void OnDisable() {
		playerStats.AttackDelayChanged -= UpdateBarWidth;
	}

	protected override void ResetBar() {
		currentStatVal = playerStats.AttackDelay;
	}
}