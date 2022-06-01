using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class LobbyRoomManager : MonoBehaviourPunCallbacks
{
    public GameObject lobbySection;
    public TMP_InputField createInput;
    public TMP_Text messageText;

    public RoomItem roomItemPrefab;
    private byte roomMaxPlayer = 2;
    private List<RoomItem> roomItemList = new List<RoomItem>();
    public Transform roomListContent;

    public float timeBetweenUpdates = 1.5f;
    private float nextUpdateTime;

    public GameObject roomSection;
    public TMP_Text roomName;
    public GameObject startButton;

    public PlayerItem playerItemPrefab;
    private List<PlayerItem> playerItemList = new List<PlayerItem>();
    public Transform playerListContent;

    void Start()
    {
        Time.timeScale = 1;
        messageText.text = "";
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        lobbySection.SetActive(true);
        roomSection.SetActive(false);
        messageText.color = new Color(255f, 255f, 255f, 255f);
        messageText.text = "Hello: " + PhotonNetwork.LocalPlayer.NickName;
    }

    public void OnClick_CreateRoom()
    {
        if(createInput.text.Length >= 1)
        {
            messageText.color = new Color(255f, 255f, 255f, 255f);
            messageText.text = "Creating...";
            PhotonNetwork.CreateRoom(createInput.text, new RoomOptions() { MaxPlayers = roomMaxPlayer, BroadcastPropsChangeToAll = true });
        }
        else
        {
            messageText.color = new Color(255f, 100f, 100f, 255f);
            StartCoroutine(CountDownCoroutine("Enter room name"));
        }
        createInput.text = "";
    }

    public override void OnJoinedRoom()
    {
        lobbySection.SetActive(false);
        roomSection.SetActive(true);
        bool isFirst = false;
        foreach(Player player in PhotonNetwork.PlayerList)
        {
            if(isFirst == false)
            {
                roomName.text = "Room Name: " + PhotonNetwork.CurrentRoom.Name + ", Room Owner: " + player.NickName;
                isFirst = true;
            }
        }
        UpdatePlayerList();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        messageText.color = new Color(255f, 100f, 100f, 255f);
        StartCoroutine(CountDownCoroutine("Room Create Failed"));
        createInput.text = "";
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        messageText.color = new Color(255f, 100f, 100f, 255f);
        StartCoroutine(CountDownCoroutine("Room Join Fail"));
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        if (Time.time >= nextUpdateTime)
        {
            UpdateRoomList(roomList);
            nextUpdateTime = Time.time + timeBetweenUpdates;
        }
    }

    private void UpdateRoomList(List<RoomInfo> list)
    {
        foreach (RoomItem item in roomItemList)
        {
            Destroy(item.gameObject);
        }
        roomItemList.Clear();
        foreach (RoomInfo room in list)
        {
            RoomItem newRoom = Instantiate(roomItemPrefab, roomListContent);
            newRoom.SetRoomName(room.Name);
            roomItemList.Add(newRoom);
        }
    }

    public void ScriptClick_JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }
    public void OnClick_LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        lobbySection.SetActive(true);
        roomSection.SetActive(false);
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerList();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerList();
    }

    private void UpdatePlayerList()
    {
        foreach (PlayerItem item in playerItemList)
        {
            Destroy(item.gameObject);
        }
        playerItemList.Clear();
        if (PhotonNetwork.CurrentRoom == null)
        {
            return;
        }
        foreach(Player player in PhotonNetwork.PlayerList)
        {
            PlayerItem newPlayerItem = Instantiate(playerItemPrefab, playerListContent);
            newPlayerItem.SetPlayerInfo(player);
            playerItemList.Add(newPlayerItem);
        }
    }

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount == roomMaxPlayer)
        {
            startButton.SetActive(true);
        }
        else
        {
            startButton.SetActive(false);
        }
    }

    public void OnClick_StartGame()
    {
        PhotonNetwork.LoadLevel("3 Game");
    }

    IEnumerator CountDownCoroutine(string text)
    {
        messageText.text = text;
        yield return new WaitForSeconds(2.0f);
        yield return null;
        messageText.text = "";
    }
}
