using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CrowdManager : MonoBehaviour
{
    public  List<GameObject> entities;
    public int startNumber;
    public int number;

    private int _updateIndex;
    private int _segmentSize;

    private void Start()
    {
        _segmentSize = startNumber / 1;
        _updateIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // update population size based on who is still alive
        CountPopulation();
        
        // update segments of entities
        //UpdateSegment();
        
        // instead of using segments which seems to cause stuttering, we update them all at once..
        // .. but, from a single source. That way we avoid callback-lag
        UpdateAll();
    }

    void UpdateAll()
    {
        foreach (GameObject entity in entities)
        {
            if (entity != null)   // in case it was destroyed
                entity.GetComponent<EntityBehavior>().UpdateThis();
        }
    }
    
    void UpdateSegment()
    {
        int startIndex = _updateIndex * _segmentSize;
        int endIndex = startIndex + _segmentSize;
        for (int i = startIndex; i < endIndex && i < number; i++)
        {
            // in case it was destroyed
            if (entities[i] != null)   
                entities[i].GetComponent<EntityBehavior>().UpdateThis();
        }

        // if we counted the same or a higher amount than the number of entities..
        if (endIndex >= number)
            _updateIndex = 0;   // reset the updateindex to 0
        else
            _updateIndex++;   // otherwise keep counting up
        
    }
    
    void CountPopulation()
    {
        int n = 0;
        foreach (var person in entities)
        {
            if (person != null && person.GetComponent<EntityBehavior>().alive)
            {
                n++;
            }
        }
        number = n;
    }
    
    public void RemoveEntity(GameObject entity)
    {
        //Called from the Entity's "DestroyThis" method.
        //Removes from the crowdmanager list, and then is destroyed.
        entities.Remove(entity);
        Destroy(entity);
    }
}
