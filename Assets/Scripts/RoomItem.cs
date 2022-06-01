using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoomItem : MonoBehaviour
{
    public TMP_Text roomName;
    private LobbyRoomManager manager;

    private void Start()
    {
        manager = FindObjectOfType<LobbyRoomManager>();
    }

    public void SetRoomName(string _roomName)
    {
        roomName.text = _roomName;
    }

    public void OnClick_JoinRoom()
    {
        manager.ScriptClick_JoinRoom(roomName.text);
    }
}
