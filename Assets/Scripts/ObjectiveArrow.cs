using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveArrow : MonoBehaviour
{
    [SerializeField] private Transform finish;
    [SerializeField] private Transform player;

    [SerializeField] Vector3 offset;
    
    void Update()
    {
        transform.position = player.position + offset;
        
        AimArrow();
    }
    
    void AimArrow()
    {
        // rotate and position arrow
        //Vector3 arrowDirection = arrow.eulerAngles;

        Vector3 objectiveDirection = finish.position - player.position;
        objectiveDirection.y = 0;
        objectiveDirection = objectiveDirection.normalized;

        transform.rotation = Quaternion.LookRotation(objectiveDirection, Vector3.up);
        
    }
    
}
