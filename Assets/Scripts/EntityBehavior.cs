using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class EntityBehavior : MonoBehaviour
{
    private CrowdManager _crowdManager;
    
    Transform _circle;
    private Rigidbody _rigidbody;

    [SerializeField] private float movementSpeed;
    public float health;

    // Distance to the circle
    private float _distance;
    private Vector3 _direction;

    Vector3 proxyCircPos;
    [SerializeField] float changeDirTime;
    private float timer;
    private float despawnDistance = 200;

    private float lastExploded;
    float explodeDelay = 2; // explosion duration is currently 2s
    
    public bool alive;

    void Start()
    {
        _crowdManager = FindObjectOfType<CrowdManager>();
        
        // Find the circle when the entity is made
        _circle = GameObject.FindGameObjectWithTag("PlayerCircle").GetComponent<Transform>();
        _rigidbody = GetComponent<Rigidbody>();

        proxyCircPos = _circle.position;

        alive = true;
        lastExploded = 0;
    }
    

    public void UpdateThis()
    {
        //Debug.Log("Update entity");
        
        if (alive)
        {
            FindCircle();
            ApplyMovement();
            
            // count the timer up in deltaTime for same-behavior
            timer += Time.deltaTime;
            lastExploded += Time.deltaTime;
        }
    }

    void FindCircle()
    {
        Vector3 entityPos = transform.position;
        
        // Find a goal for the entity somewhere around the center of the circle
        // this should help prevent forming a line, and move a bit more chaotically
        if (timer >= changeDirTime)
        {
            proxyCircPos = _circle.position + RandomDirection() * 5;
            timer = 0;
            //Debug.Log("Change Direction");
        }
        
        // distance from this entity to the center of the circle
        _distance = Vector3.Distance(entityPos, proxyCircPos);
        
        _direction = proxyCircPos - entityPos;
        _direction.y = 0;    // we dont want to move vertically
        _direction = _direction.normalized;   // normalize our horizontal vector direction
    }
    
    void ApplyMovement()
    {
        bool isGrounded = _rigidbody.position.y > 1;
        if (isGrounded)
        {
            
            // if already within the range of the circle, speed becomes 0 (no need to move)
            float currSpeed = movementSpeed;
            if ((_distance < _circle.localScale.z / 4))
            {
                currSpeed = 0;
            }
            // if entity gets to far, it should disconnect from the main group. ( no stragglers to cheat the game with )
            else if (_distance >= despawnDistance)
            {
                Destroy(gameObject);
            }

            // Move entities
            _rigidbody.position += _direction * (currSpeed * Time.deltaTime);
        }
    }
    

    Vector3 RandomDirection()
    {
        float randomRadian = Random.Range(0, 360) * Mathf.Rad2Deg;
        return new Vector3(Mathf.Cos(randomRadian), 0, Mathf.Sin(randomRadian)).normalized;

    }

    void KillEntity()
    {
        alive = false;      // setting this bool disables movement
        gameObject.GetComponent<BoxCollider>().enabled = false;     // this disables collision
        gameObject.GetComponent<Rigidbody>().freezeRotation = false;    // freezes rotation for ragdolling
        Invoke(nameof(DestroyThis), 10);
    }

    void DestroyThis()
    {
        _crowdManager.RemoveEntity(this.gameObject);
    }

    private void OnCollisionEnter(Collision hit)
    {   
        GameObject colObject = hit.gameObject;
        if (colObject.layer == 0)
            return;
        
        if (colObject.layer == 9 && hit.rigidbody.velocity.magnitude > 5)           // cars
        {   
            // Decrease health by velocity of object
            health -= 30;
        }
        else if (colObject.layer == 10)     // bullets
        {
            // average person can survive 4 bullets (with health = 20)
            health -= 5;
        }
        else if (colObject.layer == 12)     // enemies
        {
            // enemies are destroyed upon impact
            Destroy(colObject);
        }
        
        CheckHealth();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Because explosions are 'triggers', we check them as TriggerEnters instead of CollisionEnters
        if (other.gameObject.layer == 11 && lastExploded >= explodeDelay)
        {
            lastExploded = 0;   // time since last exploded is reset
            GameObject explosion = other.gameObject;
            
            // direction from explosion-epicentre to target-object
            Vector3 direction = (explosion.transform.position - transform.position);
            float distance = direction.magnitude;
            direction = direction.normalized;
            
            // radius of the blast is compared to entity's distance to epicentre
            // based on that they receive damage and are tossed away
            float radius = other.gameObject.GetComponent<SphereCollider>().radius;
            if (distance <= radius)
            {
                _rigidbody.velocity = direction * distance * 5;
                health -= (radius - distance) * 6.0f;
                // this should kill people with 20 health within half the radius-area in 1 hit
                // (7-3.5) * 6 = 21  --> 20-21 = death
                // people in the outer half of the blast take damage but don't die outright,
                // unless they were already injured
            }
        }
        
        CheckHealth();
    }

    void CheckHealth()
    {
        
        // if health drops to 0, the entity dies
        if (health <= 0)
        {
            KillEntity();
        }
    }

}
