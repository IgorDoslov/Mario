using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float xOffset, yOffset;
    [SerializeField] float followSpeed;

    // Update is called once per frame
    void Update()
    {
        TrackPlayer();
    }

    void TrackPlayer()
    {
        float xPlayer = player.position.x + xOffset;
        float yPlayer = player.position.y + yOffset;

        float xNew = Mathf.Lerp(transform.position.x, xPlayer, followSpeed * Time.deltaTime);
        float yNew = Mathf.Lerp(transform.position.y, yPlayer, followSpeed * Time.deltaTime);

        transform.position = new Vector2(xNew, yNew);
    }
}
