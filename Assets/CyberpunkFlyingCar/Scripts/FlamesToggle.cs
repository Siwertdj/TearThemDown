using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamesToggle : MonoBehaviour {

	public GameObject flameObject;
	public bool activateFlame;

	void Start()
	{
		flameObject.SetActive (true);
		activateFlame = true;
	}

	// Update is called once per frame
	void Update () {
		if (activateFlame == true)
		{
			flameObject.SetActive (false);
	        activateFlame = false;

		}

		if (activateFlame == false)
		{
			flameObject.SetActive (true);
			activateFlame = true;
		}
			
	}
}
