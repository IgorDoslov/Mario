using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickBounce : MonoBehaviour
{
    public Vector2 velocity = Vector2.zero;
    public float gravity = -9.81f;
    public float bounceForce = 10f;
    public bool wasHit = false;
    Vector2 originalPos = Vector2.zero;
    public int id;

    private void Awake()
    {
        originalPos = transform.position;
    }

    private void OnEnable()
    {
        AABB.onAABBCollision += WasBrickHit;
    }

    private void OnDisable()
    {
        AABB.onAABBCollision -= WasBrickHit;
    }

    private void Update()
    {
        if (wasHit)
            BrickJump();
    }

    void BrickJump()
    {
        velocity.y += gravity * Time.deltaTime;

        transform.position = new Vector2(transform.position.x, transform.position.y + velocity.y * Time.deltaTime);

        if (transform.position.y < originalPos.y)
        {
            transform.position = originalPos;
            wasHit = false;
        }
    }

    void WasBrickHit(int id)
    {
        if (this.id == id)
        {
            wasHit = true;
            velocity.y = bounceForce;
        }
    }
}
