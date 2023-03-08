using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class AABB : MonoBehaviour
{
    public delegate void OnAABBCollision();
    public static OnAABBCollision onAABBCollision;

    public Transform player;
    public Transform[] other;
    SpriteRenderer spriteRenderer;
    public float detectionThreshold = 10f;


    private void Update()
    {
        DistanceCheck();
    }

    void AABBCheck(Transform other)
    {
        if (player.position.x < other.position.x + other.localScale.x &&
        player.position.x + player.localScale.x > other.position.x &&
        player.position.y < other.position.y + other.localScale.y &&
        player.position.y + player.localScale.y > other.position.y)
        {
            Debug.Log("collision");
            spriteRenderer = other.GetComponent<SpriteRenderer>();
            spriteRenderer.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
            onAABBCollision?.Invoke();
        }
    }

    void DistanceCheck()
    {
        for (int i = 0; i < other.Length; i++)
        {
            if (Vector2.Distance(player.position, other[i].position) < detectionThreshold)
            {
                AABBCheck(other[i]);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(player.position, detectionThreshold);
    }
}
