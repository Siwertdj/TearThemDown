using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    //[SerializeField] PlayerMovement playerCircle;
    [SerializeField] Transform playerCircle;
    [SerializeField] float cameraSpeed;

    public Vector3 offset;     // this is an offset on the y-axis, defined in logic manager

    // Credit to Brackeys:  https://www.youtube.com/watch?v=MFQhpwc6cKE
    void FixedUpdate()
    {
        Vector3 targetPos = playerCircle.position + offset;
        Vector3 smoothPos = Vector3.Lerp(transform.position, targetPos, cameraSpeed * Time.deltaTime);
        transform.position = smoothPos;
    }
}
