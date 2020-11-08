using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    public string playerName;
    public bool myTurn;
    public int goldReserve;
    public int goldIncome;
    public int mushroomReserve;
    public int mushroomIncome;
    public int mushroomUpkeep;
    public int actions;
    public List<GameObject> ownedRooms;
    public int unitCount;
    public List<GameObject> units;
    public int stateScoreMod;
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
        int secondActionIndex = 0;
        this.actions = 2;
        myTurn = true;
        CalcIncome();
        MushroomEconomy();
        goldReserve += goldIncome;
        stateScoreMod = 0;
        List<string> actions = new List<string>();
        List<Room> tempRoomList = roomList;
        /*
        while (numActions > 0) { 
            List<string> currAction = GetAction(tempRoomList, numActions);
            for (int i = 0; i < currAction.Count; i++)
            {
                actions.Add(currAction[i]);
            }
        }
        */
        
        actions = GetMove(tempRoomList);
        switch (actions[0])
        {
            case "move":
                //move all units in actions[1] to actions[2]
                secondActionIndex = 4;
                break;
        }
        switch (actions[secondActionIndex])
        {
            case "build":
                //build building type actions[secondActionIndex + 2] in room actions[secondActionIndex + 1]
                break;
            case "overtime":
                //harvest building type actions[secondActionIndex + 2] in room actions[secondActionIndex + 1]
                break;
        }
        
        //random move selection
        System.Random rnd = new System.Random();
        //for each room
        List<Room> ownedRoomList = new List<Room>();
        for (int r = 0; r < roomList.Count; r++)
        {
            //if the AI owns it
            if (roomList[r].roomOwner == -1)
            {
                ownedRoomList.Add(roomList[r]);
            }
        }
        int moveFrom = rnd.Next(0, ownedRoomList.Count - 1);
        int moveTo = rnd.Next(0, ownedRoomList[moveFrom].Adjacent.Count - 1);
        //for each room the AI can move to from the current room
        //need to add moving to owned rooms
    }

    void EndTurn()
    {

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

    void ControlRoom(GameObject newOwned)
    {
        ownedRooms.Add(newOwned);

    }

    void CalcIncome()
    {
        int newMushroomIncome = 0;
        int newGoldIncome = 0;
        foreach (var thisRoom in ownedRooms)
        {
            Room roomscript = thisRoom.GetComponent<Room>();
            roomscript.RoomIncomeCalc();
            newMushroomIncome += roomscript.GetMushroomIncome();
            newGoldIncome += roomscript.GetGoldIncome();
        }
        mushroomIncome = newMushroomIncome;
        goldIncome = newGoldIncome;
    }

    List<string> GetAction(List<Room> roomList, int numActions)
    {
        if (numActions == 2)
        {
            return GetMove(roomList);
        }
        else
        {

        }
        List<string> actions = new List<string>();
        for (int a = 0; a < 2; a++)
        {
            List<string> moveAction = GetMove(roomList);
            for (int m = 0; m < 2; m++)
            {
                actions.Add(moveAction[m]);
            }
        }
        return actions;
    }

    List<string> GetMove(List<Room> roomList)
    {
        List<string> actions = new List<string>();
        actions.Add("move"); //action name
        actions.Add(""); //from room
        actions.Add(""); //to room
        actions.Add(""); //num to move
        int maxStateScore = 0;
        for (int r = 0; r < roomList.Count; r++)
        {
            //for each room that the AI owns
            if (roomList[r].roomOwner == -1)
            {
                //for each room the AI can move to from the current room
                //need to add moving to owned rooms
                for (int a = 0; a < roomList[r].Adjacent.Count; a++)
                {
                    
                    List<Room> tempRoomList = roomList;
                    //adjust tempRoomList to move from roomList[r] to roomList[r].Adjacent[a]
                    List<string> nonMoveAction = GetBuild(roomList);
                    List<string> battleAction = GetBattle(roomList);
                    if (false /*int.Parse(battleAction[0]) > int.Parse(nonMoveAction[0])*/)
                    {
                        nonMoveAction = battleAction;
                    }
                    List<string> overtimeAction = GetOvertime(roomList);
                    if (false /*int.Parse(overtimeAction[0]) > int.Parse(nonMoveAction[0])*/)
                    {
                        nonMoveAction = overtimeAction;
                    }
                    List<string> controlAction = GetControl(roomList);
                    if (false /*int.Parse(controlAction[0]) > int.Parse(nonMoveAction[0])*/)
                    {
                        nonMoveAction = controlAction;
                    }
                    if (int.Parse(nonMoveAction[0]) > maxStateScore)
                    {
                        maxStateScore = int.Parse(nonMoveAction[0]);
                        actions[1] = roomList[r].roomName;
                        //actions[2] = roomList[r].Adjacent[a].roomName;
                        actions[3] = "num to move";
                        for (int n = 4; n < 4 + nonMoveAction.Count - 1; n++)
                        {
                            actions[n] = nonMoveAction[n - 3];
                        }
                    }
                }
            }
        }
        return actions;
    }

    List<string> GetBuild(List<Room> roomList)
    {   
        List<string> moveScore = new List<string>();
        moveScore.Add("");
        moveScore.Add("build");
        moveScore.Add("");
        moveScore.Add("");
        int maxStateScore = 0;
        for (int r = 0; r < roomList.Count; r++)
        {
            //for each room that the AI owns
            if (roomList[r].roomOwner == -1)
            {
                List<Room> tempRoomList = roomList;
                //if can upgrade (priority: Gold, Mushrooms, Barraks)
                //else if can build
                if (roomList[r].emptySlots > 0)
                {
                    tempRoomList[r].builtBuildings[tempRoomList[r].roomSlots - tempRoomList[r].emptySlots] = 4;
                    int currStateScore = stateScore(tempRoomList);
                    if (currStateScore > maxStateScore)
                    {
                        moveScore[0] = currStateScore.ToString();
                        moveScore[2] = roomList[r].roomName;
                        moveScore[3] = "4";
                    }
                }

            }
        }
        return moveScore;
    }

    List<string> GetBattle(List<Room> roomList)
    {
        List<string> tempRoomList = new List<string>();
        tempRoomList.Add("0");
        return tempRoomList;
        //how do battles work
    }

    List<string> GetOvertime(List<Room> roomList)
    {
        List<string> tempRoomList = new List<string>();
        for (int r = 0; r < roomList.Count; r++)
        {
            //for each room that the AI owns
            if (roomList[r].roomOwner == -1)
            {
                //List<string> tempRoomList;
                //for each building owned by AI in room
                //adjust temp room list to whatever upgrade happens
            }
        }
        return tempRoomList;
    }

    List<string> GetControl(List<Room> roomList)
    {
        int roomToControl = -1;
        for (int r = 0; r < roomList.Count; r++)
        {
            //for each room that the AI doesn't own
            if (roomList[r].roomOwner != -1)
            {
                if (true /*AI can take control*/)
                {
                    roomToControl = r;
                }
            }
        }
        List<Room> tempRoomList = roomList;
        //edit tempRoomList to have room r controlled
        List<string> moveScore = new List<string>();
        moveScore.Add(stateScore(tempRoomList).ToString());
        moveScore.Add("control");
        moveScore.Add(roomList.ToString());
        return moveScore;

    }

    int stateScore(List<Room> roomList)
    {
        int newMushroomIncome = 0;
        int newGoldIncome = 0;
        foreach (var thisRoom in roomList)
        {
            if (thisRoom.roomOwner == -1)
            {
                newMushroomIncome += thisRoom.GetMushroomIncome();
                newGoldIncome += thisRoom.GetGoldIncome();
            }
        }
        int stateScore = 10 * ownedRooms.Count + newMushroomIncome + goldReserve / 4 + mushroomReserve / 4 + unitCount; //+ buildings score
        if (true/*there is gold left*/)
        {
            stateScore += newGoldIncome;
        }
        return stateScore;
    }


}