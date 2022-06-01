using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private float speed = 50f;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void ResetPosition()
    {
        rb.velocity = Vector2.zero;
        rb.position = Vector2.zero;
    }

    public void AddStartingForce()
    {
        float leftOrRight = Random.value < 0.5f ? -0.9f : 0.9f;
        float upOrDown = Random.value < 0.5f ? Random.Range(-0.9f, -0.4f) : Random.Range(0.6f, 0.9f);
        Vector2 direction = new Vector2(leftOrRight, upOrDown);
        rb.AddForce(direction * speed);
    }

    private void Update()
    {
        if (transform.position.y > 5 || transform.position.y < -5)
        {
            ResetPosition();
            AddStartingForce();
        }
        if (rb.velocity.y > 10)
        {
            rb.velocity = new Vector2(rb.velocity.x, 9.8f);
        }
        if (rb.velocity.y < -10)
        {
            rb.velocity = new Vector2(rb.velocity.x, -9.8f);
        }
    }
}
