using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleMovement : MonoBehaviour
{
    private Rigidbody _rigidbody;
    
    [SerializeField] float velocity;
    [SerializeField] float _health;

    [SerializeField] GameObject explosion;

    private float _vehicleAge;
    
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        
        // On spawn, drive forward
        Vector3 direction = (transform.rotation * Vector3.forward).normalized;
        _rigidbody.AddForce(direction * velocity, ForceMode.VelocityChange);

    }

    // Update is called once per frame
    void Update()
    {
        if (_health <= 0)
        {
            // TODO: explosion on death
            Destroy(this);
        }

        // if its speed falls below a threshold, freeze all movement (only if its older than 2 seconds
        // the time-check prevents it from standing still on spawn at times
        // this prevents the crowd from pushing the vehicle and hurting themselves.
        if (_vehicleAge > 0.5f && _rigidbody.velocity.magnitude < 2)
        {
            // it can no longer move
            _rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            
        }

        if (_health <= 0)
        {
            Invoke("KillCar", 0.5f);
        }

        _vehicleAge += Time.deltaTime;
    }

    private void OnCollisionEnter(Collision hit)
    {   
        GameObject colObject = hit.gameObject;
        if (colObject.layer == 0)
            return;
        
        if (colObject.layer == 6 && hit.rigidbody.velocity.magnitude > 5) // entities
        {   
            // Decrease health by velocity of object?
            _health -= 2;
        }
        else if (colObject.layer == 10)     // bullets
        {
            _health -= 5;
        }
        else if (colObject.layer == 11)     // explosions
        {
            _health -= 100;
        }
        else if (colObject.layer == 12)     // enemies
        {
            // destroy enemies/soldiers on hit by this vehicle
            Destroy(colObject);
        }
    }
    
    void KillCar()
    {
        // instantiate explosion at the car's location, but not as a child (that would be gameObject.transform)
        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
