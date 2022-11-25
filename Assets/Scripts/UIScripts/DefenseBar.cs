using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseBar : StatBarUIBase {

	private void OnEnable() {
		playerStats.DefenseChanged += UpdateBarWidth;
	}

	private void OnDisable() {
		playerStats.DefenseChanged -= UpdateBarWidth;
	}

	protected override void ResetBar() {
		currentStatVal = playerStats.Defense;
	}
}