using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPowerBar : StatBarUIBase {

	private void OnEnable() {
		playerStats.AttackPowerChanged += UpdateBarWidth;
	}

	private void OnDisable() {
		playerStats.AttackPowerChanged -= UpdateBarWidth;
	}

	protected override void ResetBar() {
		currentStatVal = playerStats.AttackPower;
	}
}