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
    public int numActions;
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
        numActions = 0;
        int stateScoreMod = 0;
    }

    public void StartTurn(List<Room> roomList)
    {
        numActions = 2;
        myTurn = true;
        CalcIncome();
        MushroomEconomy();
        goldReserve += goldIncome;
        stateScoreMod = 0;
        List<string> actions;
        List<Room> tempRoomList = roomList;
        while (actions > 0) { 
            List<String> currAction = GetAction(ref tempRoomList, numActions);
            for (int i = 0; i < currAction.Count(); i++)
            {
                actions.Add(currAction[i]);
            }
        }

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

    List<string> GetAction(ref List<Room> roomList, int numActions)
    {
        if (numActions == 2)
        {
            return GetMove(List<Room> roomList);
        }
        else
        {

        }
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

    List<string> GetMove(ref List<Room> roomList)
    {
        List<string> actions;
        actions.Add("move"); //action name
        actions.Add(""); //from room
        actions.Add(""); //to room
        actions.Add(""); //num to move
        int maxStateScore = 0;
        for (int r = 0; r < roomList.Count(); r++)
        {
            //for each room that the AI owns
            if (roomList[r].roomOwner == -1)
            {
                //for each room the AI can move to from the current room
                //need to add moving to owned rooms
                for (int a = 0; a < roomList[r].Adjacent.count(); a++)
                {
                    //move from roomList[r] to roomList[r].Adjacent[a]
                    int currStateScore = getStateScore(roomList);
                    if (currStateScore > maxStateScore)
                    {
                        maxStateScore = currentStateScore;
                        actions[1] = roomList[r];
                        actions[2] = roomList[r].Adjacent[a];
                        actions[3] = "num to move";
                    }
                }
            }
        }
        return actions;
    }

    List<string> GetBuild(ref List<Room> roomList)
    {
        for (int r = 0; r < roomList.Count(); r++)
        {
            //for each room that the AI owns
            if (roomList[r].roomOwner == -1)
            {
            }
        }
    }

    List<string> GetBattle(ref List<Room> roomList)
    {

    }

    List<string> GetOvertime(ref List<Room> roomList)
    {

    }

    List<string> GetControl(ref List<Room> roomList)
    {

    }

    int stateScore(List<Room> roomList)
    {
        int stateScore = 10 * ownedRooms.Count() + mushroomIncome + goldReserve / 2 + mushroomReserve / 2;
        if (1/*there is gold left*/)
        {
            stateScore += goldIncome;
        }
        return stateScore;
    }


}