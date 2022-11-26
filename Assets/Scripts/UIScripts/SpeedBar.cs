using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBar : StatBarUIBase {

	private void OnEnable() {
		playerStats.SpeedChanged += UpdateBarWidth;
	}

	private void OnDisable() {
		playerStats.SpeedChanged -= UpdateBarWidth;
	}

	protected override void ResetBar() {
		currentStatVal = playerStats.Speed;
	}
}