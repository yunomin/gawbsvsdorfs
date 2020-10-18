using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{

    public GameObject gameEngine;

    // all buttons listed
    public Button harvestButton;
    public Button buildButton;
    public Button moveButton;
    public Button controlButton;
    public Button attackButton;
    public Button endTurnButton;
    public Button undoButton;

    public Button Mine;
    public Button Farm;
    public Button Camp;

    // text
    public Text goldText;
    public Text mushroomText;
    public Text turnText;

    // UI variable
    private int buildSelection;


    // Start is called before the first frame update
    void Start()
    {
        buildSelection = 0; 
}

    // Update is called once per frame
    void Update()
    {
        // if it is player's turn, button glows and enable
        // TODO

        //
    }


    public void Harvest()
    {
        goldText.text = gameEngine.GetComponent<GameEngine>().currGold;
        mushroomText.text = gameEngine.GetComponent<GameEngine>().currMushroom;
        Harvest();
    }
    public void Control()
    {
        print("test");
    }
    public void Attack()
    {
        print("test");
    }
    public void BuildMine()
    {
        buildSelection = 4;
        Build();
    }
    public void BuildFarm()
    {
        buildSelection = 6;
        Build();
    }
    public void BuildCamp()
    {
        buildSelection = 2;
        Build();
    }
    public void Build()
    {
        Debug.Log("build");
        gameEngine.GetComponent<GameEngine>().Build(buildSelection);
    }
    public void MoveUnit()
    {
        Debug.Log("move");
        gameEngine.GetComponent<GameEngine>().MoveUnit();
    }

}
