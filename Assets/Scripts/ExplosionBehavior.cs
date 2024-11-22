using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBehavior : MonoBehaviour
{
    [SerializeField] GameObject explosionBlast;
    [SerializeField] GameObject explosionGFX;
    
    // Start is called before the first frame update
    void Start()
    {
        // the blast will be disabled at start
        explosionBlast = gameObject.transform.Find("ExplosionBlast").gameObject;
        explosionGFX.SetActive(false);
        
        Explode();
    }

    // Update is called once per frame
    void Update()  
    {
        
    }

    [ContextMenu("Explode")]
    void Explode()
    {
        CreateBlast();
        Invoke("ClearBlast", 2);
        
    }

    void CreateBlast()
    {
        //blast1 = Instantiate(explosionBlast, gameObject.transform);
        explosionBlast.GetComponent<SphereCollider>().enabled = true;
        explosionGFX.SetActive(true);
    }

    void ClearBlast()
    {
        //Destroy(blast1);
        explosionBlast.GetComponent<SphereCollider>().enabled = false;
        explosionGFX.SetActive(false);
        
        Destroy(gameObject);
    }
}
