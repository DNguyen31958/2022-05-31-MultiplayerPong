using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class UsernameManager : MonoBehaviourPunCallbacks
{
    public TMP_InputField usernameInput;
    public GameObject submitButton;
    private TMP_Text submitButtonText;

    private void Start()
    {
        submitButtonText = submitButton.GetComponentInChildren<TMP_Text>();
        submitButton.SetActive(false);
    }

    private void Update()
    {
        if (usernameInput.text.Length >= 1)
        {
            submitButton.SetActive(true);
        }
        else
        {
            submitButton.SetActive(false);
        }
    }

    public void OnClick_Submit()
    {
        PhotonNetwork.NickName = usernameInput.text;
        MasterScript.nickName = usernameInput.text;
        PhotonNetwork.AutomaticallySyncScene = true;
        submitButtonText.text = "Connecting...";
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
