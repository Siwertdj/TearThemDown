using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBlast : MonoBehaviour
{
    [SerializeField] Transform explosion;
    
    // Start is called before the first frame update

    private void Awake()
    {
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    private void OnTriggerEnter(Collider other)
    {
        // The object that collides with this should receive a velocity change
        // does this already damage the player?
        
        /*
        if (other.CompareTag("Player"))
        {
            // direction from explosion-epicentre to target-object
            Vector3 direction = (explosion.position - other.gameObject.transform.position);
            float distance = direction.magnitude;
            direction = direction.normalized;

            // scale the direction by a set amount. This may also scale with distance to epicentre instead.
            // For now everyone in the radius is blow back an equal amount
            //Destroy(other.gameObject);
            
            // TODO: Check if this works
            //other.attachedRigidbody.velocity = direction * 300;
            //other.attachedRigidbody.AddForce(direction * 300);
            other.attachedRigidbody.position += direction * 1;
            
            float radius = gameObject.transform.localScale.z;
            if (distance <= radius)
                other.GetComponent<EntityBehavior>().health -= 0; //(radius - distance) * 2.5f;
            // this should kill people with 20 health within half the radius-area in 1 hit
            // people in the outer half of the blast take damage but don't die outright,
            // unless they were already injured
        
        }
        */
    }
    
}
