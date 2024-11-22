using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CarTrigger : MonoBehaviour
{
    [SerializeField] private GameObject car;
    [SerializeField] float delay;
    
    [SerializeField] Transform spawnpoint;
    [SerializeField] Transform destination;
    private bool triggered;
    
    void Start()
    {
        // start trigger as untriggered
        triggered = false;
    }

    void OnTriggerEnter(Collider other)
    {   
        // if the player circle moves into the trigger..
        if (other.gameObject.tag == "PlayerCircle")
        {
            // if it hasnt been triggered yet
            // remove this if-statement and have it disable itself instead?
            if (!triggered)
            {
                Invoke("SpawnCar", delay);
                triggered = true;
                // could destroy the trigger here as it should only be used once..
            }
        }
    }

    void SpawnCar()
    {
        //Quaternion rotation = Quaternion.Euler(destination.position - spawnpoint.position);
        Quaternion rotation = Quaternion.LookRotation(destination.position - spawnpoint.position, Vector3.up);
        
        Instantiate(car, spawnpoint.position, rotation);
    }

}
