using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior: MonoBehaviour
{
    public Transform player;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    void LateUpdate()
    {
        if (player != null)
        {
            Vector3 newPosition = new Vector3(player.position.x + offset.x, offset.y, offset.z);
            transform.position = newPosition;

            //transform.LookAt(player);
        }
    }
}

