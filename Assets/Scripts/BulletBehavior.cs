using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    private Vector3 origin;
    [SerializeField] float fallOffDistance;
    [SerializeField] float bulletSpeed;
    public Warning sign;

    [SerializeField] GameObject explosion;

    private Vector3 _direction;
    
    private Rigidbody _rigidbody;

    private float lifeTime;     // how long the bullet has been 'alive'
    [SerializeField] float spawnCorrectionTime;   // delay before starting to check collision
    
    void Start()
    {
        origin = transform.position;
        _rigidbody = GetComponent<Rigidbody>();
        _direction = (transform.rotation * Vector3.forward).normalized;
        
    }

    void Update()
    {
        // these bullets should destroy themselves on hit, ( done in OnCollisionEnter )
        // or after traveling a certain distance 
        float distanceTravelled = Vector3.Distance(origin, transform.position);

        if (distanceTravelled >= fallOffDistance)
        {
            // Destroy the bullet
            KillBullet();
        }

        if (lifeTime >= spawnCorrectionTime)
        {
            // include buildings to collisions after collisiondelay
            gameObject.GetComponent<SphereCollider>().includeLayers += 7;
        }
        MoveBullet();
        
        lifeTime += Time.deltaTime;
    }

    void MoveBullet()
    {
        _rigidbody.AddForce(_direction * bulletSpeed, ForceMode.VelocityChange);
    }
    
    private void OnCollisionEnter(Collision other)
    {
        // Destroy the bullet if it collides with anything other than a shooter
        if (other.gameObject.tag != "Shooter")
        {
            KillBullet();   
        }
    }

    void KillBullet()
    {
        if (gameObject.CompareTag("Explosive"))
        {
            // if its a rocket, create an explosion at final position
            Destroy(sign.gameObject);       // also destroy the warning
            Instantiate(explosion, transform.position, Quaternion.identity);
        }
        
        Destroy(gameObject);
        
    }
}
