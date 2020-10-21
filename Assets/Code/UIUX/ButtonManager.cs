using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{

    /// <summary>
    /// TODO:
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
    /// Actions start
    ///     clear selection
    ///     enable selection
    /// Move 
    ///     select unit
    ///     select room
    ///     test for error
    ///     in update function: (only allow moving unit, does not hold the thread)
    ///         if isMove, room selection != null
    ///             move to selected
    ///             moved set to true
    ///             enable end action
    ///     press move
    ///     disable action buttons
    ///     isMove set to true
    ///     moved set to false
    ///     enable unit selection
    ///     enable room selection
    ///     
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
        if (gameEngine.GetComponent<GameEngine>().isTurn)
        {
            if (gameEngine.GetComponent<GameEngine>().isAction)
            {
                //action
                harvestButton.GetComponent<Button>().interactable = false;
                actionStart();
            }
            else if (gameEngine.GetComponent<GameEngine>().isEnd)
            {
                //end turn
                harvestButton.GetComponent<Button>().interactable = false;
                attackButton.GetComponent<Button>().interactable = false;
                controlButton.GetComponent<Button>().interactable = false;
                buildButton.GetComponent<Button>().interactable = false;
                moveButton.GetComponent<Button>().interactable = false;
                overworkButton.GetComponent<Button>().interactable = false;
                selectionEngine.GetComponent<SelectionEngine>().disableSelect();
            }
            else
            {
                //harvest
                buildSelection = 0;
                selectionEngine.GetComponent<SelectionEngine>().enableSelect();
                harvestButton.GetComponent<Button>().interactable = true;
            }
        }
        else
        {
            harvestButton.GetComponent<Button>().interactable = false;

            attackButton.GetComponent<Button>().interactable = false;
            controlButton.GetComponent<Button>().interactable = false;
            buildButton.GetComponent<Button>().interactable = false;
            moveButton.GetComponent<Button>().interactable = false;
            overworkButton.GetComponent<Button>().interactable = false;

            endTurnButton.GetComponent<Button>().interactable = false;
        }

        turnText.text = gameEngine.GetComponent<GameEngine>().turnNumber.ToString();
    }
    private void actionStart()
    {
        buildButton.GetComponent<Button>().interactable = true;
        moveButton.GetComponent<Button>().interactable = true;
        controlButton.GetComponent<Button>().interactable = true;
        attackButton.GetComponent<Button>().interactable = true;
        overworkButton.GetComponent<Button>().interactable = true;
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
        // TODO: Upgrade buildings
        gameEngine.GetComponent<GameEngine>().Build(buildSelection);
    }
    public void Move()
    {
        Debug.Log("move");
        gameEngine.GetComponent<GameEngine>().MoveUnit();
    }
  
}
