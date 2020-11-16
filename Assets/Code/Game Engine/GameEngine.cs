﻿using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameEngine : MonoBehaviour
{
    public Player player1; // Dorf
    public Player player2; // Gob
    private string p1Identity;
    private string p2Identity;
    public List<Player> playerList;
    public List<GameObject> roomList;
    public int currentTurnOwner; //Should be set to 1 for player 1 and -1 for player 2
    public int goldPool;
    public GameObject selectedRoom; // this are not assigned to actual rooms 
    public GameObject selectedUnit;
    private GameObject selectedBuilding;
    public string lastSelection; // save the tag name of the last selection

    //player bases
    public GameObject P1Base;
    public GameObject P2Base;

    //movement variables
    public GameObject liftedUnit;
    public bool unitLifted;
    public int numToMove;
    public GameObject previouslySelectedRoom;
    public GameObject previouslySelectedUnit;

    public GameObject towerPrefab;
    //Assign these prefabs in the editor. Reminder: x is num means that choice value relates to that building type.
    public GameObject camp1Prefab; // Camp is 2
    public GameObject goldMine_mesh; // Mine is 4
    public GameObject farm1Prefab; // Farm is 6
    public GameObject upgradedCampPrefab; //3
    public GameObject upgradedMinePrefab; //5
    public GameObject upgradedFarmPrefab; //7

    public int numActions;


    // UI variables
    public GameObject ResultPanel;
    public Text WinnerText;
    public string currGoldp1;
    public string currMushroomp1;
    public bool ActionUsed;
    public bool AIMove;
    public string currGoldp2;
    public string currMushroomp2;
    public int buildType;
    public int turnNumber;
    public bool removingUnits;
    public bool needToHarvest;
    public bool finishClicked;
    public bool GameIsPause;
    public bool enableSelection;

    public bool isTurn;
    public bool isEnd;

    // selection variables
    public GameObject selectionLight;
    public float lightHeight;
    public bool isEnable;

    // Error display
    public GameObject err;

    void Update()
    {
        camp1Prefab.gameObject.tag = "building";
        goldMine_mesh.gameObject.tag = "building";
        farm1Prefab.gameObject.tag = "building";
        // Selection
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
                        //print("clicked on room:" + hit.transform.name);
                        //TODO: Add code to move light over selected room, slowly (animated)
                        //float step = speed * Time.deltaTime; //To be used in steps, not implemented.
                        previouslySelectedRoom = selectedRoom;
                        previouslySelectedUnit = selectedUnit;
                        selectionLight.transform.position = new Vector3(hit.collider.transform.position.x, hit.collider.transform.position.y + lightHeight, hit.collider.transform.position.z);

                        SelectRoom(hit.collider.gameObject); //"Selects" the room
                        if (currentTurnOwner == 1)
                        {
                            SelectUnit(selectedRoom.GetComponent<Room>().unitSpawns[0]);
                        }
                        else
                        {
                            SelectUnit(selectedRoom.GetComponent<Room>().unitSpawns[1]);
                        }
                        lastSelection = hit.collider.gameObject.tag;
                    }

                }
            }
        }
    }

    public void enableSelect()
    {
        isEnable = true;
    }
    public void disableSelect()
    {
        isEnable = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        lightHeight = 5;
        playerList.Add(player1);
        playerList.Add(player2);
        //Set up first turn parameters.
        //This should update to 2 after the turn switches.
        goldPool = 200;
        currentTurnOwner = 1; //Player 1, (remember -1 is player 2)
        numActions = 2;
        player1.StartTurn(roomList);
        PopulateRoomStart();
        unitLifted = false;

        GameIsPause = false;
        ActionUsed = true;
        // testing code
        isTurn = true;
        isEnable = true;

    }
    private void clearSelection()
    {
        selectedRoom = null;
        selectedUnit = null;
        lastSelection = null;
    }

    void ChangeTurn()
    {
        isEnd = true;
        print("changing turn");
        isGameOver();
        List<string> AIActions = new List<string> { };
        if (currentTurnOwner > 0)
        {
            currentTurnOwner *= -1;
            AIActions = player2.StartTurn(roomList);
            clearSelection();
            goldPool -= player2.goldIncome;
            if (player2.isAI)
            {
                ProcessActions(AIActions);
            }
            else
            {
                StartCoroutine(unitUpkeep());
            }
        }
        else
        {
            currentTurnOwner *= -1;
            player1.StartTurn(roomList);
            clearSelection();
            goldPool -= player1.goldIncome;
            StartCoroutine(unitUpkeep());
        }
        this.turnNumber++;
        numActions = 2;
        ActionUsed = true;
        Harvest();
    }

    IEnumerator unitUpkeep()
    {
        removingUnits = true;
        if (currentTurnOwner == 1)
        {
            while (player1.mushroomReserve < 0)
            {
                //wait for player to select room
                sendError("you are low on mushrooms. please select a room to remove a dorf from. you must remove " + (player1.mushroomReserve * -1).ToString());
                while (true)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (selectedRoom != null)
                        {
                            break;
                        }
                    }
                    yield return null;
                }
                if (removeUnit())
                {
                    player1.mushroomReserve++;
                    selectedRoom = null;
                    needToHarvest = true;
                }
                yield return null;
            }
            while (player1.mushroomReserve > 0 && !finishClicked)
            {
                sendError("you have extra mushrooms. please click on your base to add another dorf there or click finish");
                //make done button active
                if (Input.GetMouseButtonDown(0))
                {
                    if (selectedRoom != null)
                    {
                        if (selectedRoom.GetComponent<Room>().roomName == "5-1")
                        {
                            player1.mushroomReserve--;
                            selectedRoom.GetComponent<Room>().units[0]++;
                            player1.unitCount++;
                            needToHarvest = true;
                            clearSelection();
                        }
                    }
                    
                }
                yield return null;
            }
            finishClicked = false;
            removingUnits = false;
            sendError("");
        }

        else
        {
            while (player2.mushroomReserve < 0)
            {
                //wait for player to select room
                sendError("please select a room to remove a gawb from. you must remove " + (player2.mushroomReserve * -1).ToString());
                while (true)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (selectedRoom != null)
                        {
                            break;
                        }
                    }
                    yield return null;
                }
                if (removeUnit())
                {
                    player2.mushroomReserve++;
                    selectedRoom = null;
                    needToHarvest = true;
                }
                yield return null;
            }
            while (player2.mushroomReserve > 0)
            {
                sendError("you have extra mushrooms. please click on your base to add another gawb there or click finish");
                //make done button active
                if (Input.GetMouseButtonDown(0) && !finishClicked)
                {
                    if (selectedRoom != null)
                    {
                        if (selectedRoom.GetComponent<Room>().roomName == "1-1")
                        {
                            player2.mushroomReserve--;
                            selectedRoom.GetComponent<Room>().units[1]++;
                            player2.unitCount++;
                            needToHarvest = true;
                            clearSelection();
                        }
                    }
                }
                yield return null;
            }
            finishClicked = false;
            removingUnits = false;
            sendError("");
        }
    }

    bool removeUnit()
    {
        if (currentTurnOwner == 1)
        {
            if (selectedRoom.GetComponent<Room>().units[0] == 1)
            {player2.unitCount--;
                selectedRoom.GetComponent<Room>().units[0]--;
                player1.unitCount--;
                selectedRoom.GetComponent<Room>().unitSpawns[0].active = false;
                return true;
            }
            if (selectedRoom.GetComponent<Room>().units[0] > 1)
            {
                selectedRoom.GetComponent<Room>().units[0]--;
                player1.unitCount--;
                return true;
            }
            else
            {
                sendError("you don't own any units in this room");
                return false;
            }
        }
        else
        {
            if (selectedRoom.GetComponent<Room>().units[1] == 1)
            {
                selectedRoom.GetComponent<Room>().units[1]--;
                player2.unitCount--;
                selectedRoom.GetComponent<Room>().unitSpawns[1].active = false;
                return true;
            }
            if (selectedRoom.GetComponent<Room>().units[1] > 1)
            {
                selectedRoom.GetComponent<Room>().units[1]--;
                player2.unitCount--;
                return true;
            }
            else
            {
                sendError("you don't own any units in this room");
                return false;
            }
        }
    }
    //make AI moves
    void ProcessActions(List<string> AIActions)
    {
        switch (AIActions[0])
        {
            case "move":
                foreach (GameObject room1 in roomList) {
                    if (room1.GetComponent<Room>().roomName == AIActions[1])
                    {
                        SelectRoom(room1);
                        SelectUnit(room1.GetComponent<Room>().unitSpawns[1]);
                        MoveUnit();
                        break;
                    }
                }
                
                StartCoroutine(new WaitForSecondsRealtime(2));
                foreach (GameObject room2 in roomList)
                {
                    if (room2.GetComponent<Room>().roomName == AIActions[2])
                    {
                        previouslySelectedRoom = selectedRoom;
                        previouslySelectedUnit = selectedUnit;
                        SelectRoom(room2);
                        SelectUnit(room2.GetComponent<Room>().unitSpawns[1]);
                        MoveUnit();
                        break;
                    }
                }
                break;
        }
        switch (AIActions[4])
        {
            case "build":
                foreach (GameObject room1 in roomList)
                {
                    if (room1.GetComponent<Room>().roomName == AIActions[5])
                    {
                        SelectRoom(room1);
                        Build(int.Parse(AIActions[6]));
                        break;
                    }
                }
                break;
        }
        AIMove = true;
        ChangeTurn();
    }
    int isGameOver()
    {
        string winner = "";
        if (goldPool <= 0)
        {
            //end game
            if (player1.goldReserve > player2.goldReserve)
            {
                winner = "The winner is: Dorf";
            }
            else if (player2.goldReserve > player1.goldReserve)
            {
                winner = "The winner is: Gawb";
            }
            else
            {
                //tie
                winner = "Dorfs and gawbs have found their peace..";
            }
        }
        else if (P1Base.GetComponent<Room>().roomOwner == -1)
        {
            //end game
            winner = "The winner is: Gawb";
        }
        else if (P2Base.GetComponent<Room>().roomOwner == 1)
        {
            //end game
            winner = "The winner is: Dorf";
        }
        WinnerText.text = winner;
        ResultPanel.SetActive(true);
        return 2;
    }

    void PopulateRoomStart()
    {
       //Populates the full list of rooms. 
       // when completing this funciton, make sure to initialize the two bases in the two rooms respectlly 

    }

    private void SelectRoom(GameObject newRoomSelection)
    {
        selectedRoom = newRoomSelection;
    }

    private void SelectUnit(GameObject newUnitSelection)
    {
        selectedUnit = newUnitSelection;
    }

    private void SelectBuilding(GameObject newBuildingSelection)
    {
        selectedBuilding = newBuildingSelection;
    }

    public int Harvest()
    {
        // This function is going to be called when player presses harvest button on the UI,
        // it simply update the displayed number of mushrooms and gold.

        currGoldp1 = player1.goldReserve.ToString();
        currMushroomp1 = player1.mushroomReserve.ToString();
        currGoldp2 = player2.goldReserve.ToString();
        currMushroomp2 = player2.mushroomReserve.ToString();
        return currentTurnOwner;
    }

    //Player actions
    public int MoveUnit()
    {
        if (unitLifted)
        {
            //visual
            //liftedUnit.transform.position = new Vector3(liftedUnit.transform.position.x, liftedUnit.transform.position.y - 1, liftedUnit.transform.position.z);
            unitLifted = false;
            
            //check if move can be made
            if (!(previouslySelectedRoom.GetComponent<Room>().isAdjacent(selectedRoom)))
            {
                //cannot make move
                sendError("Can not move there..");
                return 0;
            }

            //mechanical
            if (currentTurnOwner == 1)
            {
                previouslySelectedRoom.GetComponent<Room>().units[0] -= 1;
                if (previouslySelectedRoom.GetComponent<Room>().units[0] == 0)
                {
                    previouslySelectedRoom.GetComponent<Room>().unitSpawns[0].active = false;
                }
                if (selectedRoom.GetComponent<Room>().units[0] == 0)
                {
                    selectedRoom.GetComponent<Room>().unitSpawns[0].active = true;
                }
                selectedRoom.GetComponent<Room>().units[0] += 1;
            }
            else
            {
                previouslySelectedRoom.GetComponent<Room>().units[1] -= 1;
                if (previouslySelectedRoom.GetComponent<Room>().units[1] == 0)
                {
                    previouslySelectedRoom.GetComponent<Room>().unitSpawns[1].active = false;
                }
                if (selectedRoom.GetComponent<Room>().units[1] == 0)
                {
                    selectedRoom.GetComponent<Room>().unitSpawns[1].active = true;
                }
                selectedRoom.GetComponent<Room>().units[1] += 1;
            }

            numActions--;
            ActionUsed = true;
            if (numActions == 0)
            {
                this.ChangeTurn();
            }
        }
        else
        {
            //visual
            if (selectedUnit.active == true)
            {
                selectedUnit.transform.position = new Vector3(selectedUnit.transform.position.x, selectedUnit.transform.position.y + 1, selectedUnit.transform.position.z);
                liftedUnit = selectedUnit;
                unitLifted = true;
            }

            //mechanical
            if (currentTurnOwner == 1)
            {
                numToMove = 1; //selectedRoom.GetComponent<Room>().units[0];
            }
            else
            {
                numToMove = 1; //selectedRoom.GetComponent<Room>().units[1];
            }
        }
        return 0;
    }

    public int overwork ()
    {
        if (selectedRoom.GetComponent<Room>().emptySlots == selectedRoom.GetComponent<Room>().roomSlots)
        {
            sendError("There are no buildings built in the selected room..");
            return 0; 
        }
            int multiplier = 1;
        if (selectedRoom.GetComponent<Room>().roomName == "3-2")
        {
            multiplier = 2;
        }
        if (selectedRoom.GetComponent<Room>().roomOwner == currentTurnOwner)
        {
            int[] roomBuildings = selectedRoom.GetComponent<Room>().builtBuildings;
            foreach (int building in roomBuildings)
            {
                switch (building)
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
                            player1.goldReserve += 5 * multiplier;
                        }
                        else
                        {
                            player2.goldReserve += 5 * multiplier;
                        }
                        goldPool -= 5 * multiplier;
                        break;
                    case 5:
                        //add 10 gold
                        if (currentTurnOwner == 1)
                        {
                            player1.goldReserve += 10 * multiplier;
                        }
                        else
                        {
                            player2.goldReserve += 10 * multiplier;
                        }
                        goldPool -= 10 * multiplier;
                        break;
                    case 6:
                        //add 1 mushroom
                        if (currentTurnOwner == 1)
                        {
                            player1.mushroomReserve += 1 * multiplier;
                        }
                        else
                        {
                            player2.mushroomReserve += 1 * multiplier;
                        }
                        break;
                    case 7:
                        //add 3 mushrooms
                        if (currentTurnOwner == 1)
                        {
                            player1.mushroomReserve += 3 * multiplier;
                        }
                        else
                        {
                            player2.mushroomReserve += 3 * multiplier;
                        }
                        break;
                }
            }
            
            numActions--;
            ActionUsed = true;
            if (numActions == 0)
            {
                this.ChangeTurn();
            }
            return 0;
        }
        else
        {
            //quit out
            sendError("The selected room is not owned..");
            return 0;
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
                    selectedRoom.GetComponent<Room>().ChangeOwner(1);
                    player2.ownedRooms.Remove(selectedRoom);
                    player1.ownedRooms.Add(selectedRoom);
                    numActions--;
                    ActionUsed = true;
                    if (numActions == 0)
                    {
                        this.ChangeTurn();
                    }
                }
                else
                {
                    //quit out
                    //can't gain control of this room
                    sendError("You need more units in the room..");
                }
            }
            else
            {
                //quit out
                //can't gain control of this room
                sendError("You already controled the room..");
            }
        }
        else
        {
            if (selectedRoom.GetComponent<Room>().roomOwner != -1)
            {
                if (selectedRoom.GetComponent<Room>().units[1] > selectedRoom.GetComponent<Room>().units[0])
                {
                    selectedRoom.GetComponent<Room>().ChangeOwner(-1);
                    player1.ownedRooms.Remove(selectedRoom);
                    player2.ownedRooms.Add(selectedRoom);
                    numActions--;
                    ActionUsed = true;
                    if (numActions == 0)
                    {
                        this.ChangeTurn();
                    }
                }
                else
                {
                    //quit out
                    //can't gain control of this room
                    sendError("You need more units in the room..");
                }
            }
            else
            {
                //quit out
                //can't gain control of this room
                sendError("You already controled the room..");
            }
        }
    }
    public int Upgrade(int choice)
    {
        // TODO get information of the selected 
        //Build();
        Vector3 buildPos;
        int upgradeIndex = -1;
        if (choice % 2 == 1)
        {
            for (int i = 0; i < selectedRoom.GetComponent<Room>().roomSlots; i++)
            {
                if (selectedRoom.GetComponent<Room>().builtBuildings[i] == choice - 1)
                {
                    selectedRoom.GetComponent<Room>().builtBuildings[i] = choice;
                    upgradeIndex = i;
                    break;
                }
            }
            if (upgradeIndex == -1)
            {
                //quit out
                //not building of that type to upgrade
                sendError("No existing building to update..");
                return 0;
            }
            else if (currentTurnOwner == 1 && (selectedRoom.GetComponent<Room>().units[0] < 1 ||
                ((player1.goldReserve < 20 && (choice == 3 || choice == 5)) || (player1.goldReserve < 10 && choice == 7)))) //player has no units in room or doesn't have enough gold
            {
                //quit out
                //cannot build here
                sendError("You do not have enough gold..");
                return 0;
            }
            else if (currentTurnOwner == -1 && (selectedRoom.GetComponent<Room>().units[1] < 1 ||
                ((player2.goldReserve < 20 && (choice == 3 || choice == 5)) || (player2.goldReserve < 10 && choice == 7)))) //player has no units in room or doesn't have enough gold
            {
                //quit out
                //cannot build here
                sendError("You do not have enough gold..");
                return 0;
            }
        }

        selectedRoom.GetComponent<Room>().builtBuildings[upgradeIndex] = choice;
        switch (choice)
        {
            case 3:
                //delete old prefab and instantiate new one
                buildPos = selectedRoom.GetComponent<Room>().buildingPlacementSlots[upgradeIndex].transform.position;
                Destroy(selectedRoom.GetComponent<Room>().buildingPlacementSlots[upgradeIndex]);
                selectedRoom.GetComponent<Room>().buildingPlacementSlots[upgradeIndex] = Instantiate(upgradedCampPrefab, buildPos, Quaternion.identity);
                if (currentTurnOwner == 1)
                {
                    player1.goldReserve -= 20;
                }
                else
                {
                    player2.goldReserve -= 20;
                }
                break;
            case 5:
                //delete old prefab and instantiate new one
                buildPos = selectedRoom.GetComponent<Room>().buildingPlacementSlots[upgradeIndex].transform.position;
                Destroy(selectedRoom.GetComponent<Room>().buildingPlacementSlots[upgradeIndex]);
                selectedRoom.GetComponent<Room>().buildingPlacementSlots[upgradeIndex] = Instantiate(upgradedMinePrefab, buildPos, Quaternion.identity);
                if (currentTurnOwner == 1)
                {
                    player1.goldReserve -= 20;
                }
                else
                {
                    player2.goldReserve -= 20;
                }
                break;
            case 7:
                buildPos = selectedRoom.GetComponent<Room>().buildingPlacementSlots[upgradeIndex].transform.position;
                System.Threading.Thread.Sleep(50);
                Destroy(selectedRoom.GetComponent<Room>().buildingPlacementSlots[upgradeIndex]);
                System.Threading.Thread.Sleep(50);
                selectedRoom.GetComponent<Room>().buildingPlacementSlots[upgradeIndex] = Instantiate(upgradedFarmPrefab, buildPos, Quaternion.identity);
                if (currentTurnOwner == 1)
                {
                    player1.goldReserve -= 10;
                }
                else
                {
                    player2.goldReserve -= 10;
                }
                //delete old prefab and instantiate new one
                break;
        }
        numActions--;
        ActionUsed = true;
        if (numActions == 0)
        {
            this.ChangeTurn();
        }
        return 0;
    }
    public int Build(int choice)//The check for if the room can be built should be done in GameEngine.
    {
        //a player can build in a room if there are slots left, they control the room, and they have at least unit in the room
        print("Build called, choice: "+ choice);
        //print("selectedRoom.GetComponent<Room>().roomSlots: " + selectedRoom.GetComponent<Room>().roomSlots);
        Vector3 buildPos;
        if (choice % 2 == 0) { //if action is build
            if (selectedRoom.GetComponent<Room>().emptySlots <= 0 || selectedRoom.GetComponent<Room>().roomOwner != currentTurnOwner)
            { //no slots left or does not own room
                //quit out
                //cannot build here
                sendError("You are not the owner of the room.. / No empty slot.."); // TODO: MAYBE SEPARATE THE TWO CONDITIONS (NO SLOTS LEFT/ NOT OWN ROOM) TO GIVE PLAYER MORE INFO
                return 0;
            }
            else if (currentTurnOwner == 1 && (selectedRoom.GetComponent<Room>().units[0] < 1 || 
                ((player1.goldReserve < 10 && (choice == 2 || choice == 4)) || (player1.goldReserve < 5 && choice == 6)))) //player has no units in room or doesn't have enough gold
            {
                //quit out
                //cannot build here
                sendError("You do not have enough gold..");
                return 0;
            }
            else if (currentTurnOwner == -1 && (selectedRoom.GetComponent<Room>().units[1] < 1 ||
                ((player2.goldReserve < 10 && (choice == 2 || choice == 4)) || (player2.goldReserve < 5 && choice == 6)))) //player has no units in room or doesn't have enough gold
            {
                //quit out
                //cannot build here
                sendError("You do not have enough gold..");
                return 0;
            }
            else //build conditions are met
            {
                //add room choice to built room list
                selectedRoom.GetComponent<Room>().builtBuildings[selectedRoom.GetComponent<Room>().roomSlots - selectedRoom.GetComponent<Room>().emptySlots] = choice;
                selectedRoom.GetComponent<Room>().buildingNumber++;
                buildPos = selectedRoom.GetComponent<Room>().buildingPlacementSlots[selectedRoom.GetComponent<Room>().roomSlots - selectedRoom.GetComponent<Room>().emptySlots].transform.position;
                switch (choice)
                {
                    case 2:
                        buildPos.y = 0.6f;
                        Destroy(selectedRoom.GetComponent<Room>().buildingPlacementSlots[selectedRoom.GetComponent<Room>().roomSlots - selectedRoom.GetComponent<Room>().emptySlots]);
                        selectedRoom.GetComponent<Room>().buildingPlacementSlots[selectedRoom.GetComponent<Room>().roomSlots - selectedRoom.GetComponent<Room>().emptySlots] = Instantiate(camp1Prefab, buildPos, Quaternion.identity);
                        if (currentTurnOwner == 1)
                        {
                            player1.goldReserve -= 10;
                        }
                        else
                        {
                            player2.goldReserve -= 10;
                        }
                        break;
                    case 4:
                        buildPos.y = 0.6f;
                        Destroy(selectedRoom.GetComponent<Room>().buildingPlacementSlots[selectedRoom.GetComponent<Room>().roomSlots - selectedRoom.GetComponent<Room>().emptySlots]);
                        selectedRoom.GetComponent<Room>().buildingPlacementSlots[selectedRoom.GetComponent<Room>().roomSlots - selectedRoom.GetComponent<Room>().emptySlots] = Instantiate(goldMine_mesh, buildPos, Quaternion.identity);
                        if (currentTurnOwner == 1)
                        {
                            player1.goldReserve -= 10;
                        }
                        else
                        {
                            player2.goldReserve -= 10;
                        }
                        break;
                    case 6:
                        buildPos.y = 0.8f;
                        Destroy(selectedRoom.GetComponent<Room>().buildingPlacementSlots[selectedRoom.GetComponent<Room>().roomSlots - selectedRoom.GetComponent<Room>().emptySlots]);
                        selectedRoom.GetComponent<Room>().buildingPlacementSlots[selectedRoom.GetComponent<Room>().roomSlots - selectedRoom.GetComponent<Room>().emptySlots] = Instantiate(farm1Prefab, buildPos, Quaternion.identity);
                        if (currentTurnOwner == 1)
                        {
                            player1.goldReserve -= 5;
                        }
                        else
                        {
                            player2.goldReserve -= 5;
                        }
                        //Upgrade(7);
                        break;
                }
                selectedRoom.GetComponent<Room>().emptySlots--;
                Harvest();
                numActions--;
                ActionUsed = true;
                if (numActions == 0)
                {
                    this.ChangeTurn();
                }
                return 0;
            }
        }
        else
        {
            //invalid building choice
            sendError(""); // TODO: DO NOT KNOW WHAT IS THIS..
            return 0;
        }
    }

    public void Attack()
    {
        // errors:
        // no room selected
        // no unit of the turn owner 
        
        //TODO, ONLY RETREAT TO BASE RN
        //TODO, SPEND MUSHROOM

        ////////// situations:
        /// attacker to be the room owner,
        ///     tie
        ///         both remove unit and building
        ///         nobody retreat
        ///     attacker win 
        ///         defender remove unit
        ///         remining unit retreat
        ///     defender win
        ///         attacker remove unit
        ///         attacker remove building
        ///         attacker retreat

        
        int attacker;
        int defender; 
        int defeat = -1;
        int winner = -1;

        int removedBuilding = 0;
        int removedUnit = 0;
        int roomOwner = 0;
        int a = currentTurnOwner; // 1 is p1

        if (selectedRoom == null) // check for room selection
        {
            sendError("No room is selected..");
            return;
        }
        if (a == 1)
        {
            attacker = 0; 
            defender = 1;
        }
        else
        {
            attacker = 1;
            defender = 0;
        }

        if (selectedRoom.GetComponent<Room>().roomOwner == a)
        {
            roomOwner = attacker;
        }
        else
        {
            roomOwner = defender;
        }

        if(selectedRoom.GetComponent<Room>().units[attacker] == 0)
        {
            sendError("No units in the selected room..");
            return;
        }

        // check if current room is base
        // DiceVariable = Random.Range(1, 6)
        if (selectedRoom.GetComponent<Room>().builtBuildings[0] == 1) //dorf base p1
        {
            
        }
        else if(selectedRoom.GetComponent<Room>().builtBuildings[0] == 8) //gob base p2
        {

        }
        else // not base
        {
            
        }

        // roling dices
        int ap = 0;
        int dp = 0;
        for(int i = 0; i < selectedRoom.GetComponent<Room>().units[attacker]; i++)
        {
            if(Random.Range(1, 6) > 4)
                ap++;
        }
        for (int i = 0; i < selectedRoom.GetComponent<Room>().units[defender]; i++)
        {
            if (Random.Range(1, 6) > 5)
                dp++;
        }
        if(ap > dp)
        {
            defeat = defender;
            winner = attacker;
        }
        else if (dp > ap)
        {
            defeat = attacker;
            winner = defender;
        }
        else
        {
            //no body win
            defeat = 2;
            winner = 2;
        }

        print(defender.ToString());

        //remove unit
        if (selectedRoom.GetComponent<Room>().units[defender] >= ap)
        {
            selectedRoom.GetComponent<Room>().units[defender] = selectedRoom.GetComponent<Room>().units[defender] - ap;
        }
        else
        {
            int remainp = ap - selectedRoom.GetComponent<Room>().units[defender];
            if (roomOwner == defender)
            {
                for (int i = 0; i < remainp; i++)
                {
                    if (selectedRoom.GetComponent<Room>().buildingNumber == 0)
                    {
                        // no more buildings
                        break;
                    }
                    selectedRoom.GetComponent<Room>().buildingNumber--;

                    for (int j = 0; j < selectedRoom.GetComponent<Room>().roomSlots; j++)
                    {
                        if (selectedRoom.GetComponent<Room>().builtBuildings[j] != 0)
                        {
                            selectedRoom.GetComponent<Room>().builtBuildings[j] = 0;
                            Destroy(selectedRoom.GetComponent<Room>().buildingPlacementSlots[j]);

                            break;
                        }
                    }
                }
            }
        }

        //
        if (selectedRoom.GetComponent<Room>().units[attacker] >= dp)
        {
            selectedRoom.GetComponent<Room>().units[attacker] = selectedRoom.GetComponent<Room>().units[attacker] - dp;
        }
        else
        {
            int remainp = dp - selectedRoom.GetComponent<Room>().units[attacker];
            if (roomOwner == attacker)
            {
                for (int i = 0; i < remainp; i++)
                {
                    if (selectedRoom.GetComponent<Room>().buildingNumber == 0)
                    {
                        // no more buildings
                        break;
                    }
                    selectedRoom.GetComponent<Room>().buildingNumber--;

                    for (int j = 0; j < selectedRoom.GetComponent<Room>().roomSlots; j++)
                    {
                        if (selectedRoom.GetComponent<Room>().builtBuildings[j] != 0)
                        {
                            selectedRoom.GetComponent<Room>().builtBuildings[j] = 0;
                            Destroy(selectedRoom.GetComponent<Room>().buildingPlacementSlots[j]);

                            break;
                        }
                    }
                }
            }
        }
        
        // retreat
        if(defeat == 2)
        {
            // no winner
        }
        else if(selectedRoom.GetComponent<Room>().units[defeat] == 0)
        {

        }
        else
        {
            
        }
        
        numActions--;
        ActionUsed = true;
        if (numActions == 0)
        {
            this.ChangeTurn();
        }
    }

    private void sendError(string m)
    {
        err.GetComponent<ReminderManager>().updateMsg(m);
    }
}
