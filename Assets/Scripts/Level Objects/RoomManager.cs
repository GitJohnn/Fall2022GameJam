using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using SoundSystem;

public class RoomManager : MonoBehaviour {

	[SerializeField] private List<GameObject> roomDoors;

	private bool roomLockedFlag;

	[SerializeField] private UnityEvent OnPlayerEnter;
	[SerializeField] SFXEvent _sfxDoorSlam;
	[SerializeField] SFXEvent _sfxDoorOpen;

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
		_sfxDoorSlam.Play();
		foreach(var d in roomDoors) {
			d.SetActive(true);
		}
	}

	public void UnlockRoom() {
		if (roomLockedFlag == true)
		{
			_sfxDoorOpen.Play();
		}
		foreach(var d in roomDoors) {
			d.SetActive(false);
		}
	}
}