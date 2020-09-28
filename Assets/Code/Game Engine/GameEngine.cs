using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEngine : MonoBehaviour
{
    Player player1 = new Player();
    Player player2 = new Player();
    Player[] playerList;
    List<Room> roomList;
    public int currentTurnOwner; //Should be set to 1 for player 1 and -1 for player 2
    public int goldPool;




    // Start is called before the first frame update
    void Start()
    {
        playerList[0] = player1;
        playerList[1] = player2;
        //Set up first turn parameters.
        // This should update to 2 after the turn switches.
        goldPool = 300;
        currentTurnOwner = 1; //Player 1, (remember -1 is player 2)
        player1.StartTurn();
        populateRoomStart();
    }

    void ChangeTurn()
    {

    }

    void populateRoomStart()
    {
       //Populates the full list of rooms. 
    }


}
