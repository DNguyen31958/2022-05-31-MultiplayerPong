using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class PlayerItem : MonoBehaviourPunCallbacks
{
    public TMP_Text playerName;

    public void SetPlayerInfo(Player playerPerameter)
    {
        playerName.text = playerPerameter.NickName;
    }

}
