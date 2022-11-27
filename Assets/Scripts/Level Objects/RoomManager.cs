using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RoomManager : MonoBehaviour {

	[SerializeField] private List<GameObject> roomDoors;

	private bool roomLockedFlag;

	[SerializeField] private UnityEvent OnPlayerEnter;

	private void Awake() {
		UnlockRoom();
		roomLockedFlag = false;
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		var player = collision.GetComponent<PlayerStats>();
		if(player) {
			OnPlayerEnter?.Invoke();
		}
	}

	public void LockRoom() {
		if(roomLockedFlag) return;
		roomLockedFlag = true;
		foreach(var d in roomDoors) {
			d.SetActive(true);
		}
	}

	public void UnlockRoom() {
		foreach(var d in roomDoors) {
			d.SetActive(false);
		}
	}
}