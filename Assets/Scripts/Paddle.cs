using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Paddle : MonoBehaviourPunCallbacks
{
    private float moveSpeed = 20f;
    private Rigidbody2D rb;
    private Vector2 direction;
    private PhotonView pv;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        pv = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if(!pv.IsMine)
        {
            return;
        }

        if (Input.GetKey(KeyCode.W))
        {
            direction = Vector2.up;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            direction = Vector2.down;
        }
        else
        {
            direction = Vector2.zero;
        }
    }

    private void FixedUpdate()
    {
        if (direction.sqrMagnitude != 0)
        {
            rb.AddForce(direction * moveSpeed);
        }
    }

    public void ResetPosition()
    {
        rb.velocity = Vector2.zero;
        rb.position = new Vector2(rb.position.x, 0f);
    }
}
