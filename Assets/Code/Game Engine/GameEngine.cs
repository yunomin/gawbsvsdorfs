using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using UnityEngine;
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
    private GameObject selectedRoom; // this are not assigned to actual rooms 
    private GameObject selectedUnit;
    private GameObject selectedBuilding;
    public string lastSelection; // save the tag name of the last selection

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
                        selectionLight.transform.position = new Vector3(hit.collider.transform.position.x, hit.collider.transform.position.y + lightHeight, hit.collider.transform.position.z);

                        SelectRoom(hit.collider.gameObject); //"Selects" the room
                        lastSelection = hit.collider.gameObject.tag;
                    }
                    else if (hit.collider.gameObject.CompareTag("unit")) //Need to add "if current player";
                    {
                        SelectUnit(hit.collider.gameObject);
                        lastSelection = hit.collider.gameObject.tag;
                    }
                    else if (hit.collider.gameObject.CompareTag("building"))
                    {
                        SelectBuilding(hit.collider.gameObject);
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
       // when completing this funciton, make sure to initialize the two bases in the two rooms respectlly 

    }

    public void overwork ()
    {

        // shouldn't be needing arguments
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
        isAction = true;
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
                    selectedRoom.GetComponent<Room>().ChangeOwner(1);
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
    public void Upgrade()
    {
        // TODO get information of the selected 
        //Build();
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
            else if (currentTurnOwner == 1 && selectedRoom.GetComponent<Room>().units[0] < 1) //player has no units in room
            {
                //quit out
                //cannot build here
                return 0;
            }
            else if (currentTurnOwner == -1 && selectedRoom.GetComponent<Room>().units[1] < 1) //player has no units in room
            {
                //quit out
                //cannot build here
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
                        buildPos.x += 0.6f;
                        Destroy(selectedRoom.GetComponent<Room>().buildingPlacementSlots[selectedRoom.GetComponent<Room>().roomSlots - selectedRoom.GetComponent<Room>().emptySlots]);
                        selectedRoom.GetComponent<Room>().buildingPlacementSlots[selectedRoom.GetComponent<Room>().roomSlots - selectedRoom.GetComponent<Room>().emptySlots] = Instantiate(camp1Prefab, buildPos, Quaternion.identity);
                        break;
                    case 4:
                        buildPos.y = 0.6f;
                        Destroy(selectedRoom.GetComponent<Room>().buildingPlacementSlots[selectedRoom.GetComponent<Room>().roomSlots - selectedRoom.GetComponent<Room>().emptySlots]);
                        selectedRoom.GetComponent<Room>().buildingPlacementSlots[selectedRoom.GetComponent<Room>().roomSlots - selectedRoom.GetComponent<Room>().emptySlots] = Instantiate(goldMine_mesh, buildPos, Quaternion.identity);
                        break;
                    case 6:
                        buildPos.y = 0.8f;
                        Destroy(selectedRoom.GetComponent<Room>().buildingPlacementSlots[selectedRoom.GetComponent<Room>().roomSlots - selectedRoom.GetComponent<Room>().emptySlots]);
                        selectedRoom.GetComponent<Room>().buildingPlacementSlots[selectedRoom.GetComponent<Room>().roomSlots - selectedRoom.GetComponent<Room>().emptySlots] = Instantiate(farm1Prefab, buildPos, Quaternion.identity);
                        break;
                }
                selectedRoom.GetComponent<Room>().emptySlots--; 
            }
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
            if (upgradeIndex == -1)
            {
                //quit out
                //not building of that type to upgrade
                return 0;
            }
        }
        
        buildPos = selectedRoom.transform.position;
        switch (choice)
        {
            case 3:
                //delete old prefab and instantiate new one
                break;
            case 5:
                //delete old prefab and instantiate new one
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
        return 0;
    }

    public void Attack()
    {

        //TODO, ONLY RETREAT TO BASE RN
        //TODO, SPEND MUSHROOM

        if(selectedRoom == null) // check for room selection
        {
            print("error, no room selected");
            return;
        }
        int attacker;
        int defender; 
        int defeat = -1;
        int winner = -1;

        int removedBuilding = 0;
        int removedUnit = 0;
        int roomOwner = 0;
        int a = currentTurnOwner; // 1 is p1

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

        // check if current room is base
        // DiceVariable = Random.Range(1, 6)
        if (selectedRoom.GetComponent<Room>().builtBuildings[0] == 1 //dorf base p1)
        {
            
        }
        else if(selectedRoom.GetComponent<Room>().builtBuildings[0] == 8 //gob base p2)
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
        removedUnit = Math.Abs(ap - dp);

        // remove
        if(removedUnit == 0)
        {

        }
        else
        {
            if (selectedRoom.GetComponent<Room>().units[defeat] <= removeUnit)
            {
                //remove units only
                selectedRoom.GetComponent<Room>().units[defeat] = selectedRoom.GetComponent<Room>().units[defeat] - removedUnit;
            }
            else
            {
                selectedRoom.GetComponent<Room>().units[defeat] = 0;
                // remove buildings
                if(roomOwner == defeat)
                {
                    removedBuilding = removedUnit - selectedRoom.GetComponent<Room>().units[defeat];
                    // TODO, RANDOM REMOVE BUILDINGS?
                    for (int i = 0; i < removedBuilding; i++)
                    {
                        if (selectedRoom.GetComponent<Room>().buildingNumber == 0)
                        {
                            // no more buildings
                            break;
                        }
                        selectedRoom.GetComponent<Room>().buildingNumber--;
                        // find a 
                        for (int j = 0; j < selectedRoom.GetComponent<Room>().builtBuildings.size(); j++) {
                            if(selectedRoom.GetComponent<Room>().builtBuildings[j] != 0)
                            {
                                selectedRoom.GetComponent<Room>().builtBuildings[j] = 0;
                                break;
                            }
                        }                    
                    }
                }
            }

            if (winner == 0) //p1
            {
                selectedRoom.GetComponent<Room>().roomOwner = 1;
            }
            else
            {
                selectedRoom.GetComponent<Room>().roomOwner = -1;
            }
        }
    }

    
}
