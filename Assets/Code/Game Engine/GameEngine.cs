using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class GameEngine : MonoBehaviour
{
    public Player player1;
    public Player player2;
    public List<Player> playerList;
    public List<GameObject> roomList;
    public int currentTurnOwner; //Should be set to 1 for player 1 and -1 for player 2
    public int goldPool;
    public GameObject selectedRoom;
    public GameObject selectedUnit;
    public GameObject towerPrefab;
    //Assign these prefabs in the editor. Reminder: x is num means that choice value relates to that building type.
    public GameObject camp1Prefab; // Camp is 2
    public GameObject goldMine_mesh; // Mine is 4
    public GameObject farm1Prefab; // Farm is 6
    public int numActions;


    // UI variables
    public string currGoldp1;
    public string currMushroomp1;

    public string currGoldp2;
    public string currMushroomp2;
    public int buildType;
    public int turnNumber;

    public bool GameIsPause;
    public bool enableSelection;

    public bool isTurn;
    public bool isAction;
    public bool isEnd;

    // Update is called every frame
    void Update()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        playerList.Add(player1);
        playerList.Add(player2);
        //Set up first turn parameters.
        // This should update to 2 after the turn switches.
        goldPool = 300;
        currentTurnOwner = 1; //Player 1, (remember -1 is player 2)
        numActions = 2;
        player1.StartTurn();
        PopulateRoomStart();

        GameIsPause = false;
    }
    private void clearSelection()
    {
        selectedRoom = null;
        selectedUnit = null;
    }

    void ChangeTurn()
    {
        print("changing turn");
        if(currentTurnOwner > 0)
        {
            currentTurnOwner *= -1;
            player2.StartTurn();
            clearSelection();
        }
        else
        {
            currentTurnOwner *= -1;
            player1.StartTurn();
            clearSelection();
        }
        this.turnNumber++;
        numActions = 2;
    }

    void PopulateRoomStart()
    {
       //Populates the full list of rooms. 
    }

    public void harvest()
    {
        // This function is going to be called when player presses harvest button on the UI,
        // it simply update the displayed number of mushrooms and gold.
        //goldText.text = player1.goldReserve.ToString();
        // Debug.Log(player1.goldReserve.ToString());
        //mushroomText.text = player1.mushroomReserve.ToString();
    }

    public void overwork (int buildingType)
    {
        if (selectedRoom.GetComponent<Room>().roomOwner == currentTurnOwner)
        {
            switch (buildingType)
            {
                case 2:
                    //double camp value
                    break;
                case 3:
                    //double camp value
                    break;
                case 4:
                    //add 5 gold
                    if (currentTurnOwner == 1)
                    {
                        player1.goldReserve += 5;
                    }
                    else
                    {
                        player2.goldReserve += 5;
                    }
                    break;
                case 5:
                    //add 10 gold
                    if (currentTurnOwner == 1)
                    {
                        player1.goldReserve += 10;
                    }
                    else
                    {
                        player2.goldReserve += 10;
                    }
                    break;
                case 6:
                    //add 1 mushroom
                    if (currentTurnOwner == 1)
                    {
                        player1.mushroomReserve += 1;
                    }
                    else
                    {
                        player2.mushroomReserve += 1;
                    }
                    break;
                case 7:
                    //add 3 mushrooms
                    if (currentTurnOwner == 1)
                    {
                        player1.mushroomReserve += 3;
                    }
                    else
                    {
                        player2.mushroomReserve += 3;
                    }
                    break;
            }
            numActions--;
            if (numActions == 0)
            {
                this.ChangeTurn();
            }
        }
        else
        {
            //quit out
        }
    }
    
    public void SelectRoom(GameObject newRoomSelection)
    {
        selectedRoom = newRoomSelection;
    }

    public void SelectUnit(GameObject newUnitSelection)
    {
        selectedUnit = newUnitSelection;
    }

    // Player actions
    public int Harvest()
    {
        // This function is going to be called when player presses harvest button on the UI,
        // it simply update the displayed number of mushrooms and gold.
        if(currentTurnOwner == 1)
        {
            currGoldp1 = player1.goldReserve.ToString();
            currMushroomp1 = player1.mushroomReserve.ToString();
        }
        else if(currentTurnOwner == -1)
        {
            currGoldp2 = player2.goldReserve.ToString();
            currMushroomp2 = player2.mushroomReserve.ToString();
        }
        else
        {
            print("error inside harvest, game engine");
        }
        return currentTurnOwner;
    }

    public void MoveUnit()
    {
        selectedUnit.transform.position = new Vector3(selectedRoom.transform.position.x, selectedRoom.transform.position.y + 1, selectedRoom.transform.position.z);
        numActions--;
        if (numActions == 0)
        {
            this.ChangeTurn();
        }
    }

    public void Control()
    {
        if (currentTurnOwner == 1)
        {
            if (selectedRoom.GetComponent<Room>().roomOwner != 1)
            {
                if (selectedRoom.GetComponent<Room>().units[0] > selectedRoom.GetComponent<Room>().units[1])
                {
                    selectedRoom.GetComponent<Room>().roomOwner = 1;
                    numActions--;
                    if (numActions == 0)
                    {
                        this.ChangeTurn();
                    }
                }
                else
                {
                    //quit out
                    //can't gain control of this room
                }
            }
            else
            {
                //quit out
                //can't gain control of this room
            }
        }
        else
        {
            if (selectedRoom.GetComponent<Room>().roomOwner != -1)
            {
                if (selectedRoom.GetComponent<Room>().units[1] > selectedRoom.GetComponent<Room>().units[0])
                {
                    selectedRoom.GetComponent<Room>().roomOwner = -1;
                    numActions--;
                    if (numActions == 0)
                    {
                        this.ChangeTurn();
                    }
                }
                else
                {
                    //quit out
                    //can't gain control of this room
                }
            }
            else
            {
                //quit out
                //can't gain control of this room
            }
        }
    }
    public void Build(int choice)//The check for if the room can be built should be done in GameEngine.
    {
        print("Build called, choice: "+ choice);
        //print("selectedRoom.GetComponent<Room>().roomSlots: " + selectedRoom.GetComponent<Room>().roomSlots);
        if (choice % 2 == 0 && selectedRoom.GetComponent<Room>().emptySlots <= 0) //if there are not room slots left
        {
            //quit out
            //cannot build here
        }
        else { //if there are room slots left
            //add room choice to built room list
            selectedRoom.GetComponent<Room>().builtBuildings[selectedRoom.GetComponent<Room>().roomSlots - selectedRoom.GetComponent<Room>().emptySlots] = choice; 
            selectedRoom.GetComponent<Room>().emptySlots--;
        }

        int upgradeIndex = -1;
        if (choice % 2 == 1)
        {
            for (int i = 0; i < roomList.Count; i++)
            {
                if (selectedRoom.GetComponent<Room>().builtBuildings[i] == choice - 1)
                {
                    selectedRoom.GetComponent<Room>().builtBuildings[i] = choice;
                    upgradeIndex = i;
                    break;
                }
            }
        }
        if (upgradeIndex == -1)
        {
            //quit out
            //not building of that type to upgrade
        }
        Vector3 buildPos = selectedRoom.transform.position;
        switch (choice)
        {
            case 2:
                buildPos.y = 0.6f;
                Instantiate(camp1Prefab, buildPos, Quaternion.identity);
                break;
            case 3:
                //delete old prefab and instantiate new one
            case 4:
                buildPos.y = 0.6f;
                Instantiate(goldMine_mesh, buildPos, Quaternion.identity);
                break;
            case 5:
                //delete old prefab and instantiate new one
            case 6:
                buildPos.y = 0.8f;
                Instantiate(farm1Prefab, buildPos, Quaternion.identity);
                break;
            case 7:
                //delete old prefab and instantiate new one
                break;
        }
        numActions--;
        if (numActions == 0)
        {
            this.ChangeTurn();
        }
    }

    
}
