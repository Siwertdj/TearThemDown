using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class EntityMovement : MonoBehaviour
{
    Transform _circle;
    private CharacterController _controller;

    [SerializeField] private float movementSpeed;
    [SerializeField] private float health;

    // Distance to the circle
    private float _distance;
    private Vector3 _direction;

    Vector3 proxyCircPos;
    [SerializeField] float changeDirTime;
    private float timer;
    
    void Start()
    {
        // Find the circle when the entity is made
        _circle = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        _controller = GetComponent<CharacterController>();


        proxyCircPos = _circle.position;
    }

    void Update()
    {
        // if health drops to 0, the entity dies
        if (health <= 0)
        {
            KillEntity();
        }
        
        FindCircle();
        ApplyMovement();
        ApplyGravity();

        // count the timer up in deltaTime for same-behavior
        timer += Time.deltaTime;
        
    }

    void FindCircle()
    {
        // Find a goal for the entity somewhere around the center of the circle
        // this should help prevent forming a line, and move a bit more chaotically
        if (timer >= changeDirTime)
        {
            proxyCircPos = _circle.position + RandomDirection() * 5;
            timer = 0;
            //Debug.Log("Change Direction");
        }
        
        // distance from this entity to the center of the circle
        _distance = Vector3.Distance(transform.position, proxyCircPos);
        
        _direction = proxyCircPos - transform.position;
        _direction.y = 0;    // we dont want to move vertically
        _direction = _direction.normalized;   // normalize our horizontal vector direction
    }
    
    void ApplyMovement()
    {
        // Should move to be somewhere within the circle
        // TODO: add random variable for realistic movement
        // TODO: change movement speed based on distance to center elsewhere
            // at center: 0x,  at 4x distance: 2x (?)
        // TODO: pathfinding to walk around buildings, etc.
        
        // if already within the range of the circle, _direction ...?
        float currSpeed = movementSpeed;
        if ((_distance < _circle.localScale.z / 4))
        {
            currSpeed = 0;
        }
        
        _controller.Move(_direction * (currSpeed * Time.deltaTime));
    }

    void ApplyGravity()
    {
        if (!_controller.isGrounded)
        {
            _controller.Move(Vector3.down * 0.5f);
        }
    }

    Vector3 RandomDirection()
    {
        float randomRadian = Random.Range(0, 360) * Mathf.Rad2Deg;
        return new Vector3(Mathf.Cos(randomRadian), 0, Mathf.Sin(randomRadian)).normalized;

    }

    void KillEntity()
    {
        // this is simple for now, we can add a Death-sequence later
        // ..which would disable movement, create blood-splatter, etc.
        // TODO: blood-splatter, scream, replace with corpse?
        Destroy(gameObject);
    }
    
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        GameObject colObject = hit.gameObject;
        if (colObject.layer == 0)
            return;
        
        if (colObject.layer == 9 && hit.rigidbody.velocity.magnitude > 1)           // cars
        {   
            // Decrease health by velocity of object
            health -= hit.rigidbody.velocity.magnitude * 5;
        }
        else if (colObject.layer == 10)     // bullets
        {
            Debug.Log("Entity hit with bullet");
            health -= 10;
        }
        else if (colObject.layer == 11)     // explosions
        {
            health -= 300;
        }
    }
}
