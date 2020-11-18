using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{

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
    public Button upgradeButton;

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
    private int upgradeSelection;

    //Action Indicator Images
    public Sprite GawbPic;
    public Sprite DorfPic;
    public GameObject ActionOneIndicator;
    public GameObject ActionTwoIndicator;
    private Color tc;
    private Color sc;

    // Start is called before the first frame update
    void Start()
    {
        buildSelection = 0;
        actionOff();
        endTurnButton.GetComponent<Button>().interactable = false;
        tc = ActionOneIndicator.GetComponent<Image>().color;
        tc.a = 0.0f;
        sc = ActionOneIndicator.GetComponent<Image>().color;
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

        //AI update
        if (gameEngine.GetComponent<GameEngine>().AIMove)
        {
            Harvest();
        }

        // change 
        turnText.text = gameEngine.GetComponent<GameEngine>().turnNumber.ToString();
        if (gameEngine.GetComponent<GameEngine>().ActionUsed)
        {
            if (gameEngine.GetComponent<GameEngine>().currentTurnOwner == 1)
            {
                if (gameEngine.GetComponent<GameEngine>().numActions == 2)
                {
                    ActionOneIndicator.GetComponent<Image>().color = sc;
                    ActionOneIndicator.GetComponent<Image>().sprite = DorfPic;
                    ActionTwoIndicator.GetComponent<Image>().color = sc;
                    ActionTwoIndicator.GetComponent<Image>().sprite = DorfPic;
                    gameEngine.GetComponent<GameEngine>().ActionUsed = false;
                }
                else if (gameEngine.GetComponent<GameEngine>().numActions == 1)
                {
                    ActionOneIndicator.GetComponent<Image>().sprite = null;
                    ActionOneIndicator.GetComponent<Image>().color = tc;
                    ActionTwoIndicator.GetComponent<Image>().sprite = DorfPic;
                    gameEngine.GetComponent<GameEngine>().ActionUsed = false;
                }
                else if (gameEngine.GetComponent<GameEngine>().numActions == 0)
                {
                    ActionOneIndicator.GetComponent<Image>().sprite = null;
                    ActionOneIndicator.GetComponent<Image>().color = tc;
                    ActionTwoIndicator.GetComponent<Image>().sprite = null;
                    ActionTwoIndicator.GetComponent<Image>().color = tc;
                    gameEngine.GetComponent<GameEngine>().ActionUsed = false;
                }
            }
            else
            {
                if (gameEngine.GetComponent<GameEngine>().numActions == 2)
                {
                    ActionOneIndicator.GetComponent<Image>().color = sc;
                    ActionOneIndicator.GetComponent<Image>().sprite = GawbPic;
                    ActionTwoIndicator.GetComponent<Image>().color = sc;
                    ActionTwoIndicator.GetComponent<Image>().sprite = GawbPic;
                    gameEngine.GetComponent<GameEngine>().ActionUsed = false;
                }
                else if (gameEngine.GetComponent<GameEngine>().numActions == 1)
                {
                    ActionOneIndicator.GetComponent<Image>().sprite = null;
                    ActionOneIndicator.GetComponent<Image>().color = tc;
                    ActionTwoIndicator.GetComponent<Image>().sprite = GawbPic;
                    gameEngine.GetComponent<GameEngine>().ActionUsed = false;
                }
                else if (gameEngine.GetComponent<GameEngine>().numActions == 0)
                {
                    ActionOneIndicator.GetComponent<Image>().sprite = null;
                    ActionOneIndicator.GetComponent<Image>().color = tc;
                    ActionTwoIndicator.GetComponent<Image>().sprite = null;
                    ActionTwoIndicator.GetComponent<Image>().color = tc;
                    gameEngine.GetComponent<GameEngine>().ActionUsed = false;
                }
            }
        }
        if (gameEngine.GetComponent<GameEngine>().removingUnits == 1)
        {
            actionOff();
            endTurnButton.GetComponent<Button>().interactable = true;
            gameEngine.GetComponent<GameEngine>().removingUnits = 2;
        }
        else if (gameEngine.GetComponent<GameEngine>().removingUnits == 0)
        {
            actionOn();
            endTurnButton.GetComponent<Button>().interactable = false;
            gameEngine.GetComponent<GameEngine>().removingUnits = 2;
        }
        if (gameEngine.GetComponent<GameEngine>().needToHarvest)
        {
            Harvest();
            gameEngine.GetComponent<GameEngine>().needToHarvest = false;
        }
    }
    private void actionOff()
    {
        attackButton.GetComponent<Button>().interactable = false;
        controlButton.GetComponent<Button>().interactable = false;
        buildButton.GetComponent<Button>().interactable = false;
        moveButton.GetComponent<Button>().interactable = false;
        overworkButton.GetComponent<Button>().interactable = false;
        upgradeButton.GetComponent<Button>().interactable = false;
    }
    private void actionOn()
    {
        buildButton.GetComponent<Button>().interactable = true;
        moveButton.GetComponent<Button>().interactable = true;
        controlButton.GetComponent<Button>().interactable = true;
        attackButton.GetComponent<Button>().interactable = true;
        overworkButton.GetComponent<Button>().interactable = true;
        upgradeButton.GetComponent<Button>().interactable = true;
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

    public void UpdateCamp()
    {
        upgradeSelection = 3;
        upgradeOpt();
    }
    public void UpdateMine()
    {
        upgradeSelection = 5;
        upgradeOpt();
    }
    public void UpdateFarm()
    {
        upgradeSelection = 7;
        upgradeOpt();
    }
    public void Upgrade()
    {
        // need to call upgrade method in GE
        upgradePanel.SetActive(true);
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
        else
        {
            // print error saying no selection room or building
        }
    }
    public void upgradeOpt()
    {
        gameEngine.GetComponent<GameEngine>().Upgrade(upgradeSelection);
        upgradePanel.SetActive(false);
        Harvest();
    }
    public void Build()
    {
        gameEngine.GetComponent<GameEngine>().Build(buildSelection);
        buildOptionPanel.SetActive(false);
        Harvest();
    }
    public void Move()
    {
        Debug.Log("move");
        if (!gameEngine.GetComponent<GameEngine>().unitLifted)
        {
            actionOff();
            moveButton.GetComponent<Button>().interactable = true;
        }
        gameEngine.GetComponent<GameEngine>().MoveUnit();
        
    }
    
    public void Finish()
    {
        gameEngine.GetComponent<GameEngine>().finishClicked = true;
    }
}
