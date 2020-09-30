using System.Collections;
using System.Collections.Generic;
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


    // UI variables
    public Text goldText;
    public Text mushroomText;
    public Text turnText;

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
        mushroomText.text = player1.mushroomReserve.ToString();
    }

}
