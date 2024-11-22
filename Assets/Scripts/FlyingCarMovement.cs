using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingCarMovement : MonoBehaviour
{
    private Transform[] points;
    [SerializeField] private Transform point1;
    [SerializeField] private Transform point2;
    [SerializeField] private Transform point3;
    [SerializeField] private Transform point4;
    [SerializeField] private Transform point5;
    private int currentIndex;
    private Rigidbody _rigidbody;
    [SerializeField] private float movementSpeed;
    [SerializeField] float turnSmoothVelocity;
    [SerializeField] float turnSmoothTime;
    
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();

        points = new Transform[5];
        // Because of the car-model we cant get the points using components in children
        // so now we deal with 5 given points
        points[0] = point1;
        points[1] = point2;
        points[2] = point3;
        points[3] = point4;
        points[4] = point5;
        transform.position = points[0].position;
    }

    // Update is called once per frame
    void Update()
    {
        ApplyMovement();
    }
    
    void ApplyMovement()
    {   
        // TODO: increase index somewhere
        Vector3 direction = points[currentIndex].position - transform.position;
        float distance = direction.magnitude;
        direction = direction.normalized;
        
        // Rotate car towards destination
        // Find the angle of the direction between the x and z axis, and add onto it out camera rotation.
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            
        // Smooth our angle based on our current rotation, target rotation, the speed of rotation
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        // Set the rotation of our GameObject to match the found targetAngle, only across the Y-axis
        transform.rotation = Quaternion.Euler(0f, angle, 0f);

        
        // Move car. 
        _rigidbody.position += direction * (movementSpeed * Time.deltaTime);

        // If car gets close to destination, choose new destination
        if (distance < 5)
            IncreaseIndex();
    }

    void IncreaseIndex()
    {
        currentIndex++;
        if (currentIndex > points.Length - 1)
            currentIndex = 0;   // start back at index 0
    }
}
