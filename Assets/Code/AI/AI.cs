using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{

    public bool myTurn;
    public int goldReserve;
    public int goldIncome;
    public int mushroomReserve;
    public int mushroomIncome;
    public int mushroomUpkeep;
    public int actions;
    public List<Room> ownedRooms;
    // Start is called before the first frame update
    void Start()
    {

    }
    public AI()
    {
        myTurn = false;
        goldReserve = 10;
        goldIncome = 0;
        mushroomReserve = 0;
        mushroomIncome = 6;
        mushroomUpkeep = 6;
        actions = 0;
    }

    public void StartTurn(List<Room> roomList)
    {
        actions = 2;
        myTurn = true;
        CalcIncome();
        MushroomEconomy();
        goldReserve += goldIncome;
        GetActions(roomList);

    }

    void MushroomEconomy()
    {
        int rem = mushroomIncome - mushroomUpkeep;
        if (rem > 0)
        {
            mushroomReserve += rem;
        }
        else if (rem < 0)
        {
            print("Error: Remainder in playerMushroomEconomy less than 0, should not happen.");
        }
    }

    void UsedAction()
    {
        actions--;
        if (actions == 0)
        {
            //endTurn();
        }
    }

    void EndTurn()
    {

    }

    void OwnRoom(Room newOwned)
    {
        ownedRooms.Add(newOwned);
    }

    void CalcIncome()
    {
        int newMushroomIncome = 0;
        int newGoldIncome = 0;
        foreach (var thisRoom in ownedRooms)
        {
            newMushroomIncome += thisRoom.GetMushroomIncome();
            newGoldIncome += thisRoom.GetGoldIncome();
        }
        mushroomIncome = newMushroomIncome;
        goldIncome = newGoldIncome;
    }

    /*List<string>*/ void GetActions(List<Room> roomList)
    {
        List<string> actions;
        for (int a = 0; a < 2; a++)
        {
            List<string> moveAction = GetMove(List < Room > roomList);
            for (int m = 0; m < 2; m++)
            {
                actions.Add(moveAction[m]);
            }
        }
    }

    /*List<string>*/ void GetMove(List<Room> roomList)
    {
        List<string> actions;
        actions.Add("move");
        actions.Add("");
        actions.Add("");
        actions.Add("");
        actions.Add("");
        int maxStateScore = 0;
        for (int r = 0; r < roomList.Count(); r++)
        {
            //for each room that the AI owns
            if (roomList[r].roomOwner == -1)
            {
                //for each room the AI can moved to from the current room
                for (int a = 0; a < roomList[r].Adjacent.count(); a++)
                {
                    //take an action in this room
                    List<string> nonMoveAction = GetNonMove(roomList[r].Adjacent[a]);
                    currentStateScore = int.Parse(nonMoveAction[0]);
                    if (currentStateScore > maxStateScore)
                    {
                        maxStateScore = currentStateScore;
                        actions[1] = roomList[r];
                        actions[2] = roomList[r].Adjacent[a];
                        actions[3] = nonMoveAction[1];
                        actions[4] = nonMoveAction[2];
                    }
                }
            }
        }
    }




}