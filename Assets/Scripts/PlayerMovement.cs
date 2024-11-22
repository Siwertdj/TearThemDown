using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController _controller;
    [SerializeField] private float movementSpeed;

    public Vector3 lastPosition;
    private float updateTimer;
    [SerializeField] private float updateDelay;

    [SerializeField] Vector3[] spawnPoints;

    
    private void Awake()
    {
        FindSpawn();
    }

    void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;
        direction.y = 0;

        if (direction.magnitude >= 0.1f)
        {
            _controller.Move(direction * (movementSpeed * Time.deltaTime));
        }

        // if its time to register our new position, do so
        if (updateTimer >= updateDelay)
        {
            updateTimer = 0;
            RegisterPosition();
        }
        // update timer
        updateTimer += Time.deltaTime;
        
    }

    void FindSpawn()
    {
        Vector3 spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length - 1)];
        transform.position = spawnPoint;
    }
    
    void RegisterPosition()
    {
        lastPosition = transform.position;
    }
}
