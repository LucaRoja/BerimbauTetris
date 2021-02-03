using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBehaviour : MonoBehaviour
{
    public bool moving = false; // se ta mexendo este bloco
    public bool occupied = false; //se ta colorido
    public int corDoBloco = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(occupied == true)
        { 
            if(corDoBloco == 0)
                 gameObject.GetComponent<Renderer>().material.color = Color.blue;
            else if(corDoBloco == 1)
                gameObject.GetComponent<Renderer>().material.color = Color.red;
            else if (corDoBloco == 2)
                gameObject.GetComponent<Renderer>().material.color = Color.green;
            else if (corDoBloco == 3)
                gameObject.GetComponent<Renderer>().material.color = Color.yellow;
            else if (corDoBloco == 4)
                gameObject.GetComponent<Renderer>().material.color = Color.magenta;
        }
        else gameObject.GetComponent<Renderer>().material.color = Color.white;
    }
}
