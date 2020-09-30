using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Build_Button : MonoBehaviour
{
    public GameObject gameEngine;
    public Button myButton;
    // Start is called before the first frame update
    void Start()
    {
        myButton.onClick.AddListener(Build);
    }

    private void Build()
    {
        print("Build button hit");
        gameEngine.GetComponent<GameEngine>().Build(4); //Forces choice 4 for testing purposes, needs to be changed.
    }
}
