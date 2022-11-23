using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralDungeonCreation : MonoBehaviour
{
    [SerializeField] private GameObject _startingRoom = null;
    [SerializeField] private List<GameObject> _pathRooms = new List<GameObject>();
    [SerializeField] private GameObject _bossRoom = null;

    private List<RoomData> _roomsInfo = new List<RoomData>();

    private int _room = 0;
    private RoomData _currentRoom;
    private Transform _currentRoomDoor;

    private RoomData _nextRoom;
    private Transform _nextRoomDoor;

    private List<Transform> _currrentRoomUsedDoors = new List<Transform>();
    private List<Transform> _nextRoomUsedDoors = new List<Transform>();
    private bool _currentDoorValid = false;
    private bool _nextDoorValid = false;

    private void Start()
    {
        GetRoomInfo();
        SpawnRoom();
        SpawnDungeon();
    }

    private void GetRoomInfo()
    {
        _roomsInfo.Add(_startingRoom.GetComponent<SpawnObjectsInRoom>().RoomInfo);
        foreach (var room in _pathRooms)
        {
            _roomsInfo.Add(room.GetComponent<SpawnObjectsInRoom>().RoomInfo);
        }
        _roomsInfo.Add(_bossRoom.GetComponent<SpawnObjectsInRoom>().RoomInfo);
    }

    private void SpawnDungeon()
    {
        if (_room < _roomsInfo.Count - 1)
        {
            _currentRoom = _roomsInfo[_room];
            _nextRoom = _roomsInfo[_room + 1];
        }
        else
        {
            _currentRoom = _roomsInfo[_room];
            _nextRoom = _roomsInfo[_roomsInfo.Count];
        }


        while (!_currentDoorValid)
        {
            _currentRoomDoor = _roomsInfo[0].DoorLocations[UnityEngine.Random.Range(0, _roomsInfo[0].DoorLocations.Count)];
            if (_currrentRoomUsedDoors.Contains(_currentRoomDoor)) _currentDoorValid = false;
            else
            {
                _currrentRoomUsedDoors.Add(_currentRoomDoor);
                _currentDoorValid = true;
                break;
            }
        }

        while (!_nextDoorValid)
        {
            _nextRoomDoor = _roomsInfo[0].DoorLocations[UnityEngine.Random.Range(0, _roomsInfo[0].DoorLocations.Count)];
            if (_nextRoomUsedDoors.Contains(_nextRoomDoor)) _nextDoorValid = false;
            else
            {
                _nextRoomUsedDoors.Add(_nextRoomDoor);
                _nextDoorValid = true;
                break;
            }
        }
    }

    private void SpawnRoom(Transform spawnTransform)
    {
        var newRoom = Instantiate(_pathRooms[_room],transform);
        newRoom.transform.position = Vector3.zero;
        newRoom.transform.position = spawnTransform.position;
        _currentDoorValid = false;
        _nextDoorValid = false;
        _room++;
    }
}
