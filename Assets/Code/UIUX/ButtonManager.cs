using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{

    /// <summary>
    /// UI flow:
    /// game begin
    ///     enter player 1's turn
    ///         enter turn initiation, enable harvest (press to change income), disable harvest
    ///         enter action state 1, enable action bar, apply selected funtion, action -1, change image on turn indicator
    ///         enter action state 2, enable action bar 
    ///         enter turn ending, enable endturn button
    ///         tell game engine turn is end
    ///     enter player 2's turn
    ///     ...
    ///     print out result
    ///
    /// Action flow
    /// Move 
    ///     clear selection
    ///     press move
    ///     disable action buttons
    ///     isMove set to true
    ///     moved set to false
    ///     enable unit selection
    ///     enable room selection
    ///     in update function: (only allow moving unit, does not hold the thread)
    ///         if isMove, room selection != null
    ///             move to selected
    ///             moved set to true
    ///             enable end action
    ///     press end action 
    ///     
    /// </summary>

    public GameObject gameEngine;
    public GameObject selectionEngine;

    // player information
    private int playerID; // 1 for player 1 on the left, -1 for player 2 on the right

    // all buttons listed
    public Button harvestButton;
    public Button buildButton;
    public Button moveButton;
    public Button controlButton;
    public Button attackButton;
    public Button overworkButton;

    public Button endTurnButton;
    public Button undoButton;

    public Button Mine;
    public Button Farm;
    public Button Camp;

    // text
    public Text goldTextp1;
    public Text mushroomTextp1;
    public Text goldTextp2;
    public Text mushroomTextp2;
    public Text turnText;

    // UI variable
    private int buildSelection;


    // Start is called before the first frame update
    void Start()
    {
        buildSelection = 0;

        harvestButton.GetComponent<Button>().interactable = false;
        attackButton.GetComponent<Button>().interactable = false;
        controlButton.GetComponent<Button>().interactable = false;
        buildButton.GetComponent<Button>().interactable = false;
        moveButton.GetComponent<Button>().interactable = false;
        overworkButton.GetComponent<Button>().interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        // if it is player's turn, button glows and enable
        // TODO

        //

        turnText.text = gameEngine.GetComponent<GameEngine>().turnNumber.ToString();
    }

    public void initialize()
    {
        selectionEngine.GetComponent<SelectionEngine>().enableSelect();
        harvestButton.GetComponent<Button>().interactable = true;
    }
    public void Harvest()
    {
        int turnOwner = gameEngine.GetComponent<GameEngine>().Harvest(); // information updated inside game engine returned the turn owner

        if (turnOwner == 1)
        {
            goldTextp1.text = gameEngine.GetComponent<GameEngine>().currGoldp1;
            mushroomTextp1.text = gameEngine.GetComponent<GameEngine>().currMushroomp1;
        }
        else
        {
            goldTextp2.text = gameEngine.GetComponent<GameEngine>().currGoldp2;
            mushroomTextp2.text = gameEngine.GetComponent<GameEngine>().currMushroomp2;
        }
        harvestButton.GetComponent<Button>().interactable = false;
        actionOne();
    }
    private void actionOne()
    {
        buildButton.GetComponent<Button>().interactable = true;
        moveButton.GetComponent<Button>().interactable = true;
        controlButton.GetComponent<Button>().interactable = true;
        attackButton.GetComponent<Button>().interactable = true;
        overworkButton.GetComponent<Button>().interactable = true;
    }
    public void Control()
    {
        print("test");
    }
    public void Attack()
    {
        print("test");
    }
    public void Overwork()
    {
        print("test");
    }
    public void BuildMine()
    {
        buildSelection = 1;
        Build();
    }
    public void BuildFarm()
    {
        buildSelection = 2;
        Build();
    }
    public void BuildCamp()
    {
        buildSelection = 3;
        Build();
    }
    public void Build()
    {
        gameEngine.GetComponent<GameEngine>().Build(buildSelection);
    }
    public void Move()
    {
        gameEngine.GetComponent<GameEngine>().MoveUnit();
    }
    private void endActionOne()
    {

    }

    private void finishTurn()
    {
        selectionEngine.GetComponent<SelectionEngine>().disableSelect();

    }

  
}
