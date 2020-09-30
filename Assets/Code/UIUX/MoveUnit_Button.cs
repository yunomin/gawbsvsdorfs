using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveUnit_Button : MonoBehaviour
{
    public GameObject gameEngine;
    public GameObject myself;
    public Button myButton; 
    // Start is called before the first frame update
    void Start()
    {
        myButton.onClick.AddListener(MoveUnit);
    }

    private void MoveUnit()
    {
        print("Move button hit");
        gameEngine.GetComponent<GameEngine>().MoveUnit();
    }
}
