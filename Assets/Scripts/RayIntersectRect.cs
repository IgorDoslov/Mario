using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RayIntersectRect : MonoBehaviour
{
    [System.Serializable]
    public class Rectangle
    {
        public Vector2 position;
        public Vector2 size;
        public Vector2 velocity;
        public Vector2 centre;

        public Vector2 GetCentre()
        {
            return centre = new Vector2(position.x + (size.x * 0.5f), position.y + (size.y * 0.5f)); 
        }
    }


    public Vector2 rayOrigin;
    public Vector2 rayDirection;
    public Rectangle targetBox;
    public Rectangle player;
    public Vector2 contactPoint;
    public Vector2 contactNormal;
    public float tHitNear;
    public float tHitFar;
    public Vector2 tNear;
    public Vector2 tFar;
    bool isIntersect = false;
    public bool isColliding = false;
    public float playerSpeed = 2f;

    public Vector2 myRay = Vector2.zero;


    // Update is called once per frame
    void Update()
    {
        float dt = Time.deltaTime;
        isIntersect = RayVsRect(player.GetCentre(), rayDirection, targetBox, dt);
        //Debug.Log("intersect : " + isIntersect);

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        player.velocity = new Vector2(moveHorizontal, moveVertical);
        player.velocity = player.velocity.normalized;
        player.position += player.velocity * playerSpeed * dt;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(player.GetCentre(), player.GetCentre() + rayDirection);
        Gizmos.DrawLine(player.GetCentre(), player.GetCentre() + player.velocity.normalized);
        Gizmos.DrawWireCube(new Vector3(targetBox.GetCentre().x, targetBox.GetCentre().y, 0), new Vector3(targetBox.size.x, targetBox.size.y, 0));
        Gizmos.DrawWireCube(new Vector3(player.GetCentre().x, player.GetCentre().y, 0), new Vector3(player.size.x, player.size.y, 0));
        Gizmos.DrawWireSphere(targetBox.position, 0.2f);
        Gizmos.DrawWireSphere(player.position, 0.2f);


        if (isIntersect)
        {
            Gizmos.color = Color.red;

            Gizmos.DrawWireSphere(contactPoint, 0.2f);
        }
    }


    bool RayVsRect(Vector2 rayOrigin, Vector2 rayDirection, Rectangle target, /*ref Vector2 contactPoint, ref Vector2 contactNormal, ref float tHitNear,*/ float dt)
    {
        tNear = (target.position - rayOrigin) / rayDirection;
        tFar = (target.position + target.size - rayOrigin) / rayDirection;

        if (tNear.x > tFar.x)
        {
            float temp;
            temp = tNear.x;
            tNear.x = tFar.x;
            tFar.x = temp;
        }
        if (tNear.y > tFar.y)
        {
            float temp;
            temp = tNear.y;
            tNear.y = tFar.y;
            tFar.y = temp;
        }

        if (tNear.x > tFar.y || tNear.y > tFar.x)
        {

            return false;
        }

        tHitNear = Mathf.Max(tNear.x, tNear.y);
        tHitFar = Mathf.Min(tFar.x, tFar.y);

        if (tHitFar < 0)
        {

            return false;
        }

        contactPoint = rayOrigin + tHitNear * rayDirection;

        if (tNear.x > tNear.y)
        {
            if (rayDirection.x < 0)
            {
                contactNormal = new Vector2(1, 0);
            }
            else
            {
                contactNormal = new Vector2(-1, 0);
            }
        }
        else if (tNear.x < tNear.y)
        {
            if (rayDirection.y < 0)
            {
                contactNormal = new Vector2(0, 1);
            }
            else
            {
                contactNormal = new Vector2(0, -1);
            }
        }
        //isColliding = DynamicRectVsRect(player, target, contactPoint, contactNormal, dt);

        return true;
    }

    bool DynamicRectVsRect(Rectangle player, Rectangle Target, Vector2 contactPoint, Vector2 contactNormal, float dt)
    {
        if (player.velocity.x == 0 && player.velocity.y == 0)
        {
            return false;
        }

        Rectangle expandedTarget = new();
        expandedTarget.position = Target.position - player.size * 0.5f;
        expandedTarget.size = Target.size + player.size;

        if (RayVsRect((new Vector2(player.position.x, player.position.y) + player.size / 2), player.velocity * dt, expandedTarget, dt))
        {
            if (dt <= 1f)
            {
                return true;
            }
        }

        return false;
    }

}
