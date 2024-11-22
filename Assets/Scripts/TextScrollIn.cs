using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextScrollIn : MonoBehaviour
{
    [SerializeField] RectTransform start;
    [SerializeField] RectTransform end;

    private Vector2 thisPos;

    private Vector2 direction;
    private float slideSpeed = 0.5f;

    private int counter;
    
    // Start is called before the first frame update
    void Start()
    {
        thisPos = gameObject.GetComponentInChildren<TextMeshProUGUI>().rectTransform.position;
        thisPos = start.position;

        direction = (start.position - end.position).normalized;

        counter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (thisPos.x != end.position.x)
        {
            thisPos += direction * slideSpeed * Time.deltaTime;
            counter++;
        }
        else if (thisPos.x == end.position.x)
        {
            // once it reaches the end, disable this gameobject
            gameObject.SetActive(false);
            Debug.Log(thisPos.x + ", " + end.position.x);
            Debug.Log("end reached in steps: " + counter);
        }
    }
}
