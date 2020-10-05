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
    public int turnNumber;
    public GameObject selectedRoom;
    public GameObject selectedUnit;
    //Assign these prefabs in the editor. Reminder: x is num means that choice value relates to that building type.
    public GameObject camp1Prefab; // Camp is 2
    public GameObject mine1Prefab; // Mine is 4
    public GameObject farm1Prefab; // Farm is 6


    // UI variables
    public Text goldText;
    public Text mushroomText;
    public Text turnText;
    public int buildType;

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
        player1.StartTurn();
        PopulateRoomStart();
        
    }

    void ChangeTurn()
    {
        if(currentTurnOwner > 0)
        {
            currentTurnOwner *= -1;
            player2.StartTurn();

        }
        else
        {
            currentTurnOwner *= -1;
            player1.StartTurn();
        }
        turnNumber++;

        // Change the displayed turn number in UI
        turnText.text = turnNumber.ToString();
    }

    void PopulateRoomStart()
    {
       //Populates the full list of rooms. 
    }

    public void harvest()
    {
        // This function is going to be called when player presses harvest button on the UI,
        // it simply update the displayed number of mushrooms and gold.
        goldText.text = player1.goldReserve.ToString();
        // Debug.Log(player1.goldReserve.ToString());
        mushroomText.text = player1.mushroomReserve.ToString();
    }

    public void SelectRoom()
    {

    }

    public void SelectUnit(GameObject newUnitSelection)
    {
        selectedUnit = newUnitSelection;
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
