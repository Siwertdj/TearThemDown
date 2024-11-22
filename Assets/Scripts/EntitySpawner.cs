using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EntitySpawner : MonoBehaviour
{
    private CrowdManager _crowdManager;
    
    private GameObject _circle;
    public int entitiesToSpawn;
    [SerializeField] GameObject entity1;
    [SerializeField] GameObject entity2;
    [SerializeField] GameObject entity3;
    [SerializeField] GameObject entity4;
    [SerializeField] GameObject entity5;
    [SerializeField] GameObject entity6;
    [SerializeField] GameObject entity7;
    [SerializeField] GameObject entity8;

    private List<GameObject> _people;
    public void Start()
    {
        _crowdManager = FindObjectOfType<CrowdManager>();
        
        _people = new List<GameObject>();
        _people.Add(entity1);
        _people.Add(entity2);
        _people.Add(entity3);
        _people.Add(entity4);
        _people.Add(entity5);
        _people.Add(entity6);
        _people.Add(entity7);
        _people.Add(entity8);
    }
    
    public void Spawn()
    {
        
        _circle = GameObject.FindGameObjectWithTag("PlayerCircle");
        
        // fetch circle-area
        float circleRadius = _circle.transform.localScale.z;
        Vector3 circlePos = _circle.transform.position;
        
        
        
        // spawns a given number of entities within a certain range of the player-circle
        for (int i = 0; i < entitiesToSpawn; i++)
        {
            // add area to spawn them in
            // spawn them in that area randomly
            // choose a random horizontal direction from the center, and add a random distance to it
            float randomRadian = Random.Range(0, 360) * Mathf.Rad2Deg;
            float randomDistance = Random.Range(0, circleRadius);
            Vector3 spawnPos = circlePos  - 
                               new Vector3( Mathf.Cos(randomRadian), 
                                            0, 
                                            Mathf.Sin(randomRadian)
                                          ).normalized * randomDistance;
            spawnPos.y = 1;

            // Fetch a random person from the list. Same behavior, different look.
            GameObject person = _people[Random.Range(0, _people.Count - 1)];
            
            GameObject newPerson = Instantiate(person, spawnPos, _circle.transform.rotation);   
            
            _crowdManager.entities.Add(newPerson);
        }
    }

}
