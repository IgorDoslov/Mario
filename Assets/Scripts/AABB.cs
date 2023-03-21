using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class AABB : MonoBehaviour
{
    public delegate void OnAABBCollision(int id);
    public static OnAABBCollision onAABBCollision;
    PlayerController playerController;
    public BoxCollider2D player;
    public BoxCollider2D[] other;
    SpriteRenderer spriteRenderer;
    public float detectionThreshold = 10f;
    Vector2 lastPosition = Vector2.zero;
    Vector2 currentPosition = Vector2.zero;
    bool isColliding = false;

    private void Start()
    {
        playerController = player.GetComponent<PlayerController>();
    }

    private void Update()
    {
        DistanceCheck();
    }

    void AABBCheck(BoxCollider2D other, int id)
    {
        currentPosition = player.transform.position;
       
        Vector2 diff = other.transform.position - player.transform.position; // vector from target to player
        //float height = (player.size.y + other.size.y) / 2;
        //float width = (player.size.x + other.size.x) / 2;

        float height = player.bounds.extents.y + other.bounds.extents.y;
        float width = player.bounds.extents.x + other.bounds.extents.x;

        //Debug.Log("height: " + height + " width: " + width);
        Vector2 intersect = new Vector2(Mathf.Abs(diff.x) - width, Mathf.Abs(diff.y) - height);
        //Debug.Log("intersect x: " + intersect.x);
        Debug.Log("current x pos: " + currentPosition.x + "last x pos: " + lastPosition.x);
        if (intersect.x > 0 || intersect.y > 0)
        {
            isColliding = false;
        }
        else
        {
            isColliding = true;
        }

        if (intersect.x < intersect.y)
        {
            intersect.x = 0f;
        }
        else
        {
            intersect.y = 0f;
        }
        //Debug.Log("intersect x after check: " + intersect.x);

        //diff.x = diff.x > 0f ? 1 : -1;
        //diff.y = diff.y > 0f ? 1 : -1;


        //player.transform.position = new Vector2(-intersect.x * diff.x, -intersect.y * diff.y);
        if (isColliding)
        {
            if (lastPosition.x < currentPosition.x || lastPosition.y < currentPosition.y)
            {
                player.transform.position = new Vector2(player.transform.position.x + intersect.x, player.transform.position.y + intersect.y);
                playerController.velocity = Vector2.zero;
            }
            else
            {
                player.transform.position = new Vector2(player.transform.position.x - intersect.x, player.transform.position.y - intersect.y);
                playerController.velocity = Vector2.zero;

            }

            //if (player.position.x < other.position.x + other.localScale.x &&
            //player.position.x + player.localScale.x > other.position.x &&
            //player.position.y < other.position.y + other.localScale.y &&
            //player.position.y + player.localScale.y > other.position.y)
            {
                //Debug.Log("collision");
                spriteRenderer = other.GetComponent<SpriteRenderer>();
                spriteRenderer.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), spriteRenderer.color.a);
                onAABBCollision?.Invoke(id + 1);
            }
        }
        lastPosition = currentPosition;

    }

    void DistanceCheck()
    {
        for (int i = 0; i < other.Length; i++)
        {
            //if (Vector2.Distance(player.position, other[i].position) < detectionThreshold)
            //{
            AABBCheck(other[i], i);
            //}
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(player.transform.position, detectionThreshold);
    }
}
