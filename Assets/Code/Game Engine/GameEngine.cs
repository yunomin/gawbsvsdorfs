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
    private GameObject selectedRoom;
    private GameObject selectedUnit;
    private GameObject selectedBuilding;
    public string lastSelection; // save the tag name of the last selection
    //Assign these prefabs in the editor. Reminder: x is num means that choice value relates to that building type.
    public GameObject camp1Prefab; // Camp is 2
    public GameObject mine1Prefab; // Mine is 4
    public GameObject farm1Prefab; // Farm is 6


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
                        print("clicked on room:" + hit.transform.name);
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
        turnNumber++;
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
    }

    public void Build(int choice)//The check for if the room can be built should be done in GameEngine.
    {
        print("Build called, choice: "+ choice);
        print("selectedRoom.GetComponent<Room>().roomSlots: " + selectedRoom.GetComponent<Room>().roomSlots);
        //selectedRoom.GetComponent<Room>().builtBuildings[selectedRoom.GetComponent<Room>().roomSlots - selectedRoom.GetComponent<Room>().emptySlots] = choice; //for instance, builtBuildings[0] will be the first assigned
                                                //as [2 - 2] = 0. Then [2 - 1] = 1, and will be the second assigned.
        switch (choice)
        {
            case 2:
                Instantiate(camp1Prefab, selectedRoom.transform.position, Quaternion.identity);
                break;
            case 4:
                Instantiate(mine1Prefab, selectedRoom.transform.position, Quaternion.identity);
                break;
            case 6:
                Instantiate(farm1Prefab, selectedRoom.transform.position, Quaternion.identity);
                break;

        }
        selectedRoom.GetComponent<Room>().roomSlots--;
    }

    
}
