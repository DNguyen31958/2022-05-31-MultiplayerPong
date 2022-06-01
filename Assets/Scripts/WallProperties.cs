using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WallProperties : MonoBehaviour
{
    public bool isLeft;
    public bool isScoreArea;
    private float bounceStrength = 0.5f;
    private GameManager gameManager;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>().GetComponent<GameManager>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D ball;
        if (collision.collider.tag == "Ball")
        {
            ball = collision.gameObject.GetComponent<Rigidbody2D>();
        }
        else
        {
            return;
        }
        if(PhotonNetwork.IsMasterClient)
        {
            if (isLeft && isScoreArea)
            {
                gameManager.OtherScores();
            }
            else if (!isLeft && isScoreArea)
            {
                gameManager.MasterScores();
            }
        }
        if (!isScoreArea)
        {
            if (ball != null)
            {
                Vector2 normal = collision.GetContact(0).normal;
                ball.AddForce(-normal * bounceStrength, ForceMode2D.Impulse);
            }
        }
    }
}
