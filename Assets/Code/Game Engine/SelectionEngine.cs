﻿using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class SelectionEngine : MonoBehaviour
{
    /// <summary>
    /// TODO:
    /// selected object glow effect
    /// can not select enemy unity
    /// 
    /// </summary>
    public GameObject gameEngine;
    public GameObject selectionLight;
    //public float speed;
    public float lightHeight;

    public bool isEnable;

    // Start is called before the first frame update
    public void enableSelect()
    {
        isEnable = true;
    }
    public void disableSelect()
    {
        isEnable = false;
    }

    void Start()
    {
        // Once move/build button is triggered, 
        isEnable = true;
        //speed = 100.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (isEnable)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //Determines what is clicked
                if (Physics.Raycast(ray, out hit, 100.0f))
                {
                    print("Hit!:" + hit.collider.name);
                    if (hit.collider.gameObject.CompareTag("room")) //Will detect if hit is on a "room" (via tag)
                    {
                        print("clicked on room:" + hit.transform.name);
                        //TODO: Add code to move light over selected room, slowly (animated)
                        //float step = speed * Time.deltaTime; //To be used in steps, not implemented.
                        selectionLight.transform.position = new Vector3(hit.collider.transform.position.x, hit.collider.transform.position.y + lightHeight, hit.collider.transform.position.z);

                        //gameEngine.GetComponent<GameEngine>().SelectRoom(hit.collider.gameObject); //"Selects" the room


                    } // ensure you picked right object
                    else if (hit.collider.gameObject.CompareTag("unit")) //Need to add "if current player";
                    {
                        //gameEngine.GetComponent<GameEngine>().SelectUnit(hit.collider.gameObject);
                    }
                    else if (hit.collider.gameObject.CompareTag("building"))
                    {
                        //gameEngine.GetComponent<GameEngine>().SelectBuilding(hit.collider.gameObject);
                    }
                }

            }
        }
    }
}
