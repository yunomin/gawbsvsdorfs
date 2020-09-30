using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{

    public GameObject gameEngine;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Build()
    {
        print("Build button hit");
        gameEngine.GetComponent<GameEngine>().Build(4); //Forces choice 4 for testing purposes, needs to be changed.
    }

    public void MoveUnit()
    {
        gameEngine.GetComponent<GameEngine>().MoveUnit();
    }
}
