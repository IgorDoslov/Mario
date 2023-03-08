using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Vector2 velocity = Vector2.zero;
    public float force = 10f;
    public float mass = 1f;
    public float drag = 0.98f;
    public float inputX = 0;
    public float jumpForce = 15f;
    public float gravity = -9.81f;
    public float xTarget = 0f;
    public float yTarget = 0f;
    float timer = 0f;
    public float hangTime = 0.2f;
    public float influence = 0.6f;
    public bool isGrounded;
    public float overlapCircleRadius = 1f;

    void Update()
    {
        float dt = Time.deltaTime;

        GetInput();
        HorizontalMovement(dt);
        Jump(dt);
    }

    void HorizontalMovement(float dt)
    {
        float accel = (force / mass) * inputX;

        velocity.x += accel * dt;
        velocity.y += gravity * dt;

        // taily here. Drag is between 0 and 1. Good drag 0.9 - 0.98. Below this floaty.
        velocity.x = TailyTransitionTo(velocity.x, xTarget, dt, drag);
        //velocity.y = TailyTransitionTo(velocity.y, yTarget, dt, drag);



        transform.position = new Vector2(transform.position.x + velocity.x * dt, transform.position.y + velocity.y * dt);

        if (transform.position.y <= -7f)
        {
            transform.position = new Vector2(transform.position.x, -7f);
        }

        // if velocity is close to 0 and no input, clamp to 0
        if (inputX == 0f && Mathf.Abs(velocity.x) < 0.9f)
        {
            velocity.x = 0f;
        }

    }

    void Jump(float dt)
    {

        isGrounded = Physics2D.OverlapCircle(transform.position, overlapCircleRadius);

        if (isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                velocity.y = jumpForce;
            }
            else if (Input.GetKey(KeyCode.Space))
            {
                timer += dt;
                if (timer < hangTime)
                {
                    velocity.y = jumpForce * influence;

                }
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                timer = 0f;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(transform.position, overlapCircleRadius);
    }

    void GetInput()
    {
        if (Input.GetKey(KeyCode.D))
        {
            inputX = 1;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            inputX = -1;
        }
        else
        {
            inputX = 0;
        }
    }



    private static float simulationFPS;

    public static Vector3 TailyTransitionTo(Vector3 from, Vector3 to, float timeElapsed, float friction)
    {
        simulationFPS = 60;

        if (Mathf.Abs(1 - timeElapsed * simulationFPS) < 0.0001f)
        {
            return to + (from - to) * friction;
        }
        return to + (from - to) * Mathf.Pow(friction, timeElapsed * simulationFPS);
    }

    public static float TailyTransitionTo(float from, float to, float timeElapsed, float friction)
    {
        //if (simulationFPS == 0f)
        //{
        //    simulationFPS = Screen.currentResolution.refreshRate;
        //}

        simulationFPS = 60;

        //from Utils.h (lines 117 - 126)
        //assumes a regular timestep

        if (Mathf.Abs(1 - timeElapsed * simulationFPS) < 0.0001f)
        {
            return to + (from - to) * friction;
        }
        return to + (from - to) * Mathf.Pow(friction, timeElapsed * simulationFPS);
    }
}


