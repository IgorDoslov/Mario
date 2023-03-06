using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float xOffset, yOffset;
    [SerializeField] float followSpeed;
    PlayerController playerController;
    [SerializeField] Vector2 borderOffset = new Vector2(0.5f, 0.5f);
    Vector2 threshold;

    private void Awake()
    {
        playerController = player.GetComponent<PlayerController>();
        threshold = CalculateThreshold();
    }

    // Update is called once per frame
    void Update()
    {
        float dt = Time.deltaTime;
        TrackPlayer(dt);
    }

    void TrackPlayer(float dt)
    {
        Vector2 trackingTarget = player.position;


        float xDiff = transform.position.x - trackingTarget.x;

        //Debug.Log("target x : " + trackingTarget.x + " border x : " + (border.x * borderOffset.x) + " xDiff : " + xDiff);
        Vector2 newPosition = transform.position;

        if (Mathf.Abs(xDiff) >= threshold.x)
        {
            newPosition.x = trackingTarget.x;
        }

        float moveSpeed = playerController.velocity.magnitude > followSpeed ? playerController.velocity.magnitude : followSpeed;
        transform.position = Vector3.MoveTowards(transform.position, newPosition, Mathf.Abs(playerController.velocity.x) * dt);

    }

    private void OnGUI()
    {

    }

    Vector2 CalculateThreshold()
    {
        Rect aspect = Camera.main.pixelRect;
        Vector2 border = new Vector2(Camera.main.orthographicSize * aspect.width / aspect.height, Camera.main.orthographicSize);
        border = border * borderOffset;
        return border;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Vector2 border = CalculateThreshold();
        Gizmos.DrawSphere(transform.position, border.x);

    }

    private static float simulationFPS;

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
