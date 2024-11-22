using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoldierBehavior : MonoBehaviour
{
    [SerializeField] GameObject bullet;
    [SerializeField] Warning warning;
    private PlayerMovement circle;

    private float _distance;    // distance between player and the soldier
    private Vector3 _direction;   // the direction to aim in

    [SerializeField] float shootRange;
    [SerializeField] int shootAmount;
    private int counter;    // number left to shoot
    [SerializeField] float shootDelay;   // time between bursts
    private float timer;

    private Warning currentSign;
    
    void Start()
    {
        counter = shootAmount;
        circle = GameObject.FindGameObjectWithTag("PlayerCircle").GetComponent<PlayerMovement>();
    }

    void Update()
    {
        // Chooses a target, visible (and closest?)
        //TODO: is visible?
        FindTarget();

        if (_distance < shootRange) // if within range..
        {
            if (counter > 0)
            {
                if (bullet.name == "Rocket")
                    PrepareShot();
                else 
                    ShootBullet(); 
                counter--;
            }
            else
            {
                timer += Time.deltaTime; // update timer while not being able to shoot
                if (timer >= shootDelay) // if the timer exceeds the designated delay-time..
                {
                    counter = shootAmount; // reset counter
                    timer = 0; // reset timer
                }
            }
        }
    }
    
    void FindTarget()
    {
        Vector3 observedPosition = new Vector3(circle.lastPosition.x, 0.5f, circle.lastPosition.z);
        _direction = observedPosition - transform.position;
        _direction.y += 1;  // correct the direction a little, so they dont shoot the ground instead
        _distance = _direction.magnitude;   // distance is the length of the un-normalized direction
        _direction = _direction.normalized; // normalize the direction

        RotateShooter();
    }

    void RotateShooter()
    {
        // rotate based on x and z direction of shoot-target
        Vector3 targetDir = new Vector3(_direction.x, 0, _direction.z).normalized;
        //gameObject.transform.rotation = Quaternion.LookRotation(targetDir);
        //gameObject.transform.LookAt(Vector3.Lerp(gameObject.transform.forward, targetDir, 1f));
        gameObject.transform.rotation =
            Quaternion.LookRotation(Vector3.Lerp(gameObject.transform.forward, targetDir, 2f * Time.deltaTime));
    }

    

    // Sprays bullets in the player's direction, with a slightly-off aim
    void ShootBullet()
    {
        // Create random offset to rotate the direction by
        float randomOffset = Random.Range(-5f, 5f);
        Quaternion randomRotation = Quaternion.Euler(0, randomOffset, 0);

        // Instantiate(entity, spawnPos, _circle.transform.rotation);   

        Quaternion rotation = Quaternion.LookRotation(randomRotation * _direction, Vector3.up);
            
        Vector3 bulletPos = transform.position; // + _direction * 3;
        GameObject newBullet = Instantiate(bullet, bulletPos, rotation);
        newBullet.GetComponent<BulletBehavior>().sign = currentSign;
    }

    void PrepareShot()
    {
        //GameObject _warning = GameObject.Instantiate(warning, transform.position, Quaternion.Euler(Vector3.up));
        Warning sign = Instantiate(warning, transform.position, Quaternion.LookRotation(Vector3.up * -1));
        sign.shooter = gameObject.transform;
        currentSign = sign;
        
        // check if still in range
        if (_distance <= shootRange)
            Invoke("ShootBullet", 3);
        else
            timer = 0;   // nothing to shoot at, reset the timer.
    }

}
