using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Warning : MonoBehaviour
{
    public Transform shooter;
    private Transform player;
    private Vector3 origin;
    
    // distance between player and warning sign
    [SerializeField] float distance;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("PlayerCircle").transform;
        origin = shooter.position;
    }
    
    void Update()
    {
        PositionWarning();
    }
    
    void PositionWarning()
    {
        // get direction
        Vector3 warningDirection = origin - player.position;
        warningDirection.y = 0;
        warningDirection = warningDirection.normalized;

        // create the offset for the warning
        Vector3 offset = warningDirection * distance;
        offset.y = 20;  // it should be above the ground, etc.

        transform.position = player.position + offset;

    }
}
