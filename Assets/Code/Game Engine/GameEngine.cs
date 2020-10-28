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
    private GameObject selectedRoom; // this are not assigned to actual rooms 
    private GameObject selectedUnit;
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

    // selection variables
    public GameObject selectionLight;
    public float lightHeight;
    public bool isEnable;

    void Update()
    {
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
                    /*
                    else if (hit.collider.gameObject.CompareTag("unit")) //Need to add "if current player";
                    {
                        SelectUnit(hit.collider.gameObject);
                        lastSelection = hit.collider.gameObject.tag;
                    }
                    else if (hit.collider.gameObject.CompareTag("building"))
                    {
                        SelectBuilding(hit.collider.gameObject);
                        lastSelection = hit.collider.gameObject.tag;
                    }*/
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
        playerList.Add(player1);
        playerList.Add(player2);
        //Set up first turn parameters.
        //This should update to 2 after the turn switches.
        goldPool = 300;
        currentTurnOwner = 1; //Player 1, (remember -1 is player 2)
        numActions = 2;
        player1.StartTurn();
        PopulateRoomStart();
        unitLifted = false;

        GameIsPause = false;

        // testing code
        isTurn = true;
    }
    private void clearSelection()
    {
        selectedRoom = null;
        selectedUnit = null;
    }

    void ChangeTurn()
    {
        print("changing turn");
        isGameOver();
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
        Harvest();
    }

    int isGameOver()
    {
        if (goldPool <= 0)
        {
            //end game
            if (player1.goldReserve > player2.goldReserve)
            {
                return 1;
            }
            else if (player2.goldReserve > player1.goldReserve)
            {
                return -1;
            }
            else
            {
                //tie
                return 0;
            }
        }
        else if (P1Base.GetComponent<Room>().roomOwner == -1)
        {
            //end game
            return -1;
        }
        else if (P2Base.GetComponent<Room>().roomOwner == 1)
        {
            //end game
            return 1;
        }
        return 2;
    }

    void PopulateRoomStart()
    {
       //Populates the full list of rooms. 
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

        isAction = true;
        return currentTurnOwner;
    }

    //Player actions
    public int MoveUnit()
    {
        if (unitLifted)
        {
            //visual
            liftedUnit.transform.position = new Vector3(liftedUnit.transform.position.x, liftedUnit.transform.position.y - 1, liftedUnit.transform.position.z);
            unitLifted = false;
            
            //check if move can be made
            if (!(previouslySelectedRoom.GetComponent<Room>().isAdjacent(selectedRoom)))
            {
                //cannot make move
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
        int multiplier = 1;
        if (selectedRoom.GetComponent<Room>().roomName == "Mushroom Lake")
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
            if (numActions == 0)
            {
                this.ChangeTurn();
            }
            return 0;
        }
        else
        {
            //quit out
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
                    selectedRoom.GetComponent<Room>().ChangeOwner(-1);
                    player1.ownedRooms.Remove(selectedRoom);
                    player2.ownedRooms.Add(selectedRoom);
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
    public int Upgrade(int choice)
    {
        // TODO get information of the selected 
        //Build();
        Vector3 buildPos;
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
            if (upgradeIndex == -1)
            {
                //quit out
                //not building of that type to upgrade
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
                selectedRoom.GetComponent<Room>().buildingPlacementSlots[upgradeIndex] = Instantiate(camp1Prefab, buildPos, Quaternion.identity);
                break;
            case 5:
                //delete old prefab and instantiate new one
                buildPos = selectedRoom.GetComponent<Room>().buildingPlacementSlots[upgradeIndex].transform.position;
                Destroy(selectedRoom.GetComponent<Room>().buildingPlacementSlots[upgradeIndex]);
                selectedRoom.GetComponent<Room>().buildingPlacementSlots[upgradeIndex] = Instantiate(goldMine_mesh, buildPos, Quaternion.identity);
                break;
            case 7:
                buildPos = selectedRoom.GetComponent<Room>().buildingPlacementSlots[upgradeIndex].transform.position;
                System.Threading.Thread.Sleep(50);
                Destroy(selectedRoom.GetComponent<Room>().buildingPlacementSlots[upgradeIndex]);
                System.Threading.Thread.Sleep(50);
                selectedRoom.GetComponent<Room>().buildingPlacementSlots[upgradeIndex] = Instantiate(farm1Prefab, buildPos, Quaternion.identity);
                //delete old prefab and instantiate new one
                break;
        }
        numActions--;
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
                return 0;
            }
            else if (currentTurnOwner == 1 && (selectedRoom.GetComponent<Room>().units[0] < 1 || 
                ((player1.goldReserve < 10 && (choice == 2 || choice == 4)) || (player1.goldReserve < 5 && choice == 6)))) //player has no units in room or doesn't have enough gold
            {
                //quit out
                //cannot build here
                return 0;
            }
            else if (currentTurnOwner == -1 && (selectedRoom.GetComponent<Room>().units[1] < 1 ||
                ((player2.goldReserve < 10 && (choice == 2 || choice == 4)) || (player2.goldReserve < 5 && choice == 6)))) //player has no units in room or doesn't have enough gold
            {
                //quit out
                //cannot build here
                return 0;
            }
            else //build conditions are met
            {
                //add room choice to built room list
                selectedRoom.GetComponent<Room>().builtBuildings[selectedRoom.GetComponent<Room>().roomSlots - selectedRoom.GetComponent<Room>().emptySlots] = choice;
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
            return 0;
        }
    }

    public void Attack()
    {
        if(selectedRoom == null) // check for room selection
        {
            print("error, no room selected");
            return;
        }
        int attacker;
        int defender;

        int aUnit;
        int dUnit;

        int aDies = 0;
        int dDies = 0;

        int removedUnit = 0;


        attacker = currentTurnOwner;
        defender = 0 - attacker;

        if(attacker == 1)
        {
            // attacker is player 1
            //aUnit = selectedRoom.units[0];
            //dUnit = selectedRoom.units[1];
        }
        else
        {
            // attacker is player 2
            //aUnit = selectedRoom.units[1];
            //dUnit = selectedRoom.units[0];
        }

        // check for adjecent clearing
    }

    
}
