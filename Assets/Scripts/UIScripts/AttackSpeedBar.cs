using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSpeedBar : StatBarUIBase {

	private void OnEnable() {
		playerStats.AttackSpeedChanged += UpdateBarWidth;
	}

	private void OnDisable() {
		playerStats.AttackSpeedChanged -= UpdateBarWidth;
	}

	protected override void ResetBar() {
		currentStatVal = playerStats.AttackSpeed;
	}
}