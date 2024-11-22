using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finishline : MonoBehaviour
{
    [SerializeField] LogicManager _logicManager;

    private int numberAtFinish;
    private int numberToWin;
    
    // Start is called before the first frame update
    void Start()
    {
        numberToWin = _logicManager.entitiesToWin;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        Debug.Log(numberAtFinish);
        if (numberAtFinish >= numberToWin)
        {
            _logicManager.GameWon();
        }
        else
        {
            numberAtFinish = 0;
        }
        */
    }
    

    void OnTriggerStay(Collider other)
    {
        // if a live person/entity/player enters the finish, you win!
        if (other.gameObject.tag == "Player" && other.GetComponent<EntityBehavior>().alive)
        {
            //Debug.Log("person at finish");
            //numberAtFinish++;
            _logicManager.GameWon();
        }
    }
}
