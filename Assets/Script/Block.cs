using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    float timePassed = 0;
    private Transform _tr;
    float velocityY = 0.25f;
    float velocityX = 0.0f;
    bool disable = false;


    // Start is called before the first frame update
    void Start()
    {
        _tr = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        timePassed += Time.deltaTime;
        if (timePassed > 0.5)
         {
            if(!disable)
                _tr.position = new Vector3(_tr.position.x + velocityX, _tr.position.y - velocityY, _tr.position.z);
             timePassed = 0;
            velocityX = 0.0f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        disable = true;
        Debug.Log("Bateu");
    }

    public void goRight()
    {
        if(_tr.position.x < 1.7f)
            velocityX = 0.35f;
    }

    public void goLeft()
    {
        if (_tr.position.x > -1.7f)
            velocityX = -0.35f;
    }
}