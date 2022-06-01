using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class GameManager : MonoBehaviourPunCallbacks, IPunObservable
{
    public GameObject playerPrefab;
    public GameObject ballPrefab;
    public Transform[] spawnPoints;

    public TMP_Text countdownText;
    public GameObject leaveButton;
    private GameObject ballMade;
    private Ball ball;

    private GameObject paddleMade;
    private Paddle paddlePaddle;
    private string masterName;
    private string otherName;

    public TMP_Text masterScoreText;
    private bool isMasterNameSet;
    private int masterScore;

    public TMP_Text otherScoreText;
    public TMP_Text pingText;
    private int otherScore;
    private bool isGameEnd;

    private void Start()
    {
        leaveButton.SetActive(false);
        Vector2 pos;
        if(PhotonNetwork.IsMasterClient)
        {
            pos = spawnPoints[0].position;
            ballMade = PhotonNetwork.Instantiate(ballPrefab.name, spawnPoints[2].position, Quaternion.identity);
            ball = ballMade.GetComponent<Ball>();
        }
        else
        {
            pos = spawnPoints[1].position;
        }
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if(isMasterNameSet == false)
            {
                masterName = player.NickName;
                isMasterNameSet = true;
            }
            else
            {
                otherName = player.NickName;
            }
        }
        paddleMade = PhotonNetwork.Instantiate(playerPrefab.name, pos, Quaternion.identity);
        paddlePaddle = paddleMade.GetComponent<Paddle>();
        StartCoroutine(CountDownCoroutine());
    }

    public void NewGame()
    {
        masterScore = 0;
        otherScore = 0;
        StartRound();
    }

    public void StartRound()
    {
        paddlePaddle.ResetPosition();
        if (PhotonNetwork.IsMasterClient && isGameEnd == false)
        {
            ball.ResetPosition();
            ball.AddStartingForce();
        }
        if (PhotonNetwork.IsMasterClient && isGameEnd == true)
        {
            ball.ResetPosition();
        }
    }

    public void MasterScores()
    {
        masterScore = masterScore + 1;
        StartRound();
    }

    public void OtherScores()
    {
        otherScore = otherScore + 1;
        StartRound();
    }

    IEnumerator CountDownCoroutine()
    {
        masterScoreText.text = "";
        otherScoreText.text = "";
        countdownText.text = "Starting in: 3";
        yield return new WaitForSeconds(1.0f);
        countdownText.text = "Starting in: 2";
        yield return new WaitForSeconds(1.0f);
        countdownText.text = "Starting in: 1";
        yield return new WaitForSeconds(1.0f);
        countdownText.text = "GO!";
        yield return new WaitForSeconds(1.0f);
        countdownText.text = "";
        yield return null;
        NewGame();
    }

    private void Update()
    {
        pingText.text = "Ping: " + PhotonNetwork.GetPing();
        masterScoreText.text = masterName + ": " + masterScore;
        otherScoreText.text = otherName + ": " + otherScore;
        if ((masterScore >= 3 || otherScore >= 3))
        {
            Time.timeScale = 0.01f;
            if(masterScore == 3)
            {
                countdownText.text = "Winner: " + masterName;
            }
            else
            {
                countdownText.text = "Winner: " + otherName;
            }
            leaveButton.SetActive(true);
            isGameEnd = true;
        }
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void OnClick_LeaveRoom()
    {
        Time.timeScale = 1;
        PhotonNetwork.LeaveRoom();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        OnClick_LeaveRoom();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(masterScore);
            stream.SendNext(otherScore);
        }
        else
        {
            this.masterScore = (int)stream.ReceiveNext();
            this.otherScore = (int)stream.ReceiveNext();
        }
    }

}
