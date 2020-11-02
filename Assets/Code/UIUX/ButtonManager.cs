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
    /// inbetween selection
    ///     clear selection(all)
    ///     
    /// Build
    ///     select room, select building
    ///     if last selection is building
    ///         upgrade menu shows up and build 
    ///     if last selection is room 
    ///         build menu shows up
    ///     in the end of upgrade and build , close two menus
    ///     
    /// press end action 
    ///     
    /// </summary>

    public GameObject gameEngine;
    public GameObject selectionEngine;

    // player information
    private int playerID; // 1 for player 1 on the left, -1 for player 2 on the right

    // all buttons listed
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

    // panel
    public GameObject upgradePanel;
    public GameObject buildOptionPanel;

    // UI variable
    private int buildSelection;


    // Start is called before the first frame update
    void Start()
    {
        buildSelection = 0;
        actionOff();
        endTurnButton.GetComponent<Button>().interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Button enable and disable (ok I ve been changing ways of doing this but I am sure this code below is final!)
        if (gameEngine.GetComponent<GameEngine>().isTurn)
        {
            
            if (gameEngine.GetComponent<GameEngine>().isEnd)
            {
                //end turn
                //actionOff();
                buildSelection = 0;
                //gameEngine.GetComponent<GameEngine>().disableSelect();
            }
            else
            {
                //action
                Harvest();
                gameEngine.GetComponent<GameEngine>().enableSelect();
                actionOn();
            }
        }
        else
        {
            actionOff();
            endTurnButton.GetComponent<Button>().interactable = false;
        }

        // change 
        turnText.text = gameEngine.GetComponent<GameEngine>().turnNumber.ToString();
    }
    private void actionOff()
    {
        attackButton.GetComponent<Button>().interactable = false;
        controlButton.GetComponent<Button>().interactable = false;
        buildButton.GetComponent<Button>().interactable = false;
        moveButton.GetComponent<Button>().interactable = false;
        overworkButton.GetComponent<Button>().interactable = false;
    }
    private void actionOn()
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

        goldTextp1.text = gameEngine.GetComponent<GameEngine>().currGoldp1;
        mushroomTextp1.text = gameEngine.GetComponent<GameEngine>().currMushroomp1;
        goldTextp2.text = gameEngine.GetComponent<GameEngine>().currGoldp2;
        mushroomTextp2.text = gameEngine.GetComponent<GameEngine>().currMushroomp2;

        gameEngine.GetComponent<GameEngine>().Harvest();
    }
    public void Control()
    {
        gameEngine.GetComponent<GameEngine>().Control();
    }
    public void Attack()
    {
        gameEngine.GetComponent<GameEngine>().Attack();
    }
    public void Overwork()
    {
        gameEngine.GetComponent<GameEngine>().overwork();
        Harvest();
        print("test");
    }
    public void Upgrade()
    {
        // need to call upgrade method in GE
        gameEngine.GetComponent<GameEngine>().Upgrade(7);
        upgradePanel.SetActive(false);
    }
    public void BuildCamp()
    {
        buildSelection = 2;
        Build();
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
    public void BuildClicked()
    {
        print("button Clicked build" + gameEngine.GetComponent<GameEngine>().lastSelection.Equals("room"));
        if (gameEngine.GetComponent<GameEngine>().lastSelection.Equals("room"))
        {
            buildOptionPanel.SetActive(true);
        }
        else if (gameEngine.GetComponent<GameEngine>().lastSelection.Equals("building"))
        {
            upgradePanel.SetActive(true);
        }
        else
        {
            // print error saying no selection room or building
        }
    }
    public void Build()
    {
        // TODO: Upgrade buildings
        gameEngine.GetComponent<GameEngine>().Build(buildSelection);
        buildOptionPanel.SetActive(false);
        Harvest();
    }
    public void Move()
    {
        Debug.Log("move");
        gameEngine.GetComponent<GameEngine>().MoveUnit();
    }
  
}
