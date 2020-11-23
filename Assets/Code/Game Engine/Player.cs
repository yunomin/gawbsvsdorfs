using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public string playerName;
    public bool myTurn;
    public int goldReserve;
    public int goldIncome;
    public int mushroomReserve;
    public int mushroomIncome;
    public int mushroomUpkeep;
    public int numActions;
    public List<GameObject> ownedRooms;
    public int unitCount;
    public List<GameObject> units;
    public bool isAI;
    // Start is called before the first frame update

    void Start()
    {
        
    }

    public Player()
    {
        myTurn = false;
        goldReserve = 10;
        goldIncome = 0;
        mushroomReserve = 0;
        mushroomIncome = 0;
        mushroomUpkeep = 0;
        numActions = 0;
    }

    public List<string> StartTurn(List<GameObject> roomList)
    {
        numActions = 2;
        myTurn = true;
        CalcIncome();
        MushroomEconomy();
        goldReserve += goldIncome;
        if (isAI)
        {
            roomList = roomList;
            List<string> AIAction = GetMove(roomList);
            for (int i = 0; i < AIAction.Count; i++)
            {
                print(AIAction[i]);
            }
            return AIAction;
        }
        else
        {
            return new List<string> { };
        }

    }

    void MushroomEconomy()
    {
        int rem = mushroomIncome - unitCount;
        //if (rem > 0)
       // {
            mushroomReserve += rem;
        /*}
        else if (rem < 0)
        {
            print("Error: Remainder in playerMushroomEconomy less than 0, should not happen.");
        }*/
    }

    void UsedAction()
    {
        numActions--;
        if (numActions == 0)
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
     






    //AI stuff
    
    
    List<string> GetAction(List<GameObject> roomList, int numActions)
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

    List<string> GetMove(List<GameObject> roomList)
    {
        List<string> actions = new List<string>();
        actions.Add("move"); //action name
        actions.Add(""); //from room
        actions.Add(""); //to room
        actions.Add(""); //num to move
        actions.Add(""); //non move action
        actions.Add("");
        actions.Add("");
        actions.Add("");
        int maxStateScore = 0;
        for (int r = 0; r < roomList.Count; r++)
        {
            //for each room that the AI owns
            if (roomList[r].GetComponent<Room>().roomOwner == -1)
            {
                //for each room the AI can move to from the current room
                //need to add moving to owned rooms
                for (int a = 0; a < roomList[r].GetComponent<Room>().Adjacent.Count; a++)
                {

                    List<GameObject> tempRoomList = roomList;
                    //adjust tempRoomList to move from roomList[r] to roomList[r].Adjacent[a]
                    tempRoomList[r].GetComponent<Room>().units[1] -= 1;
                    int adjIndex = -1;
                    for (int q = 0; q < tempRoomList.Count; q++)
                    {
                        if (tempRoomList[q].GetComponent<Room>().roomName == roomList[r].GetComponent<Room>().Adjacent[a].GetComponent<Room>().roomName)
                        {
                            adjIndex = q;
                        }
                    }
                    print("here 1");
                    tempRoomList[adjIndex].GetComponent<Room>().units[1] += 1;
                    print("here 2");
                    List<string> nonMoveAction = GetBuild(roomList);
                    print("build");
                    List<string> battleAction = GetBattle(roomList);
                    print("battle");
                    if (battleAction.Count > 0)
                    {
                        if (int.Parse(battleAction[0]) > int.Parse(nonMoveAction[0]))
                        {
                            nonMoveAction = battleAction;
                        }
                    }
                    print("overtime");
                    List<string> overtimeAction = GetOvertime(roomList);
                    if (overtimeAction.Count > 0)
                    {
                        if (int.Parse(overtimeAction[0]) > int.Parse(nonMoveAction[0]))
                        {
                            nonMoveAction = overtimeAction;
                        }
                    }
                    List<string> controlAction = GetControl(roomList);
                    print("control");
                    if (controlAction.Count > 0)
                    {
                        if (int.Parse(controlAction[0]) > int.Parse(nonMoveAction[0]))
                        {
                            nonMoveAction = controlAction;
                        }
                    }
                    print("here 3");
                    if (int.Parse(nonMoveAction[0]) > maxStateScore)
                    {
                        maxStateScore = int.Parse(nonMoveAction[0]);
                        actions[1] = roomList[r].GetComponent<Room>().roomName;
                        actions[2] = roomList[r].GetComponent<Room>().Adjacent[a].GetComponent<Room>().roomName;
                        actions[3] = "1";
                        for (int n = 4; n < 4 + nonMoveAction.Count - 1; n++)
                        {
                            actions[n] = nonMoveAction[n - 3];
                        }
                    }
                }
            }
        }

        print("got actions");
        return actions;
    }

    List<string> GetBuild(List<GameObject> roomList)
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
            if (roomList[r].GetComponent<Room>().roomOwner == -1)
            {
                List<GameObject> tempRoomList = roomList;
                //if can upgrade (priority: Gold, Mushrooms, Barraks)
                //else if can build
                if (roomList[r].GetComponent<Room>().emptySlots > 0)
                {
                    tempRoomList[r].GetComponent<Room>().builtBuildings[tempRoomList[r].GetComponent<Room>().roomSlots - tempRoomList[r].GetComponent<Room>().emptySlots] = 4;
                    int currStateScore = stateScore(tempRoomList);
                    if (currStateScore > maxStateScore)
                    {
                        moveScore[0] = currStateScore.ToString();
                        moveScore[2] = roomList[r].GetComponent<Room>().roomName;
                        moveScore[3] = "4";
                    }
                }

            }
        }
        return moveScore;
    }

    List<string> GetBattle(List<GameObject> roomList)
    {
        List<string> tempRoomList = new List<string>();
        tempRoomList.Add("0");
        return tempRoomList;
        //how do battles work
    }

    List<string> GetOvertime(List<GameObject> roomList)
    {
        List<string> tempRoomList = new List<string>();
        for (int r = 0; r < roomList.Count; r++)
        {
            //for each room that the AI owns
            if (roomList[r].GetComponent<Room>().roomOwner == -1)
            {
                //List<string> tempRoomList;
                //for each building owned by AI in room
                //adjust temp room list to whatever upgrade happens
            }
        }
        return tempRoomList;
    }

    List<string> GetControl(List<GameObject> roomList)
    {
        int roomToControl = -1;
        for (int r = 0; r < roomList.Count; r++)
        {
            //for each room that the AI doesn't own
            if (roomList[r].GetComponent<Room>().roomOwner != -1)
            {
                if (true /*AI can take control*/)
                {
                    roomToControl = r;
                }
            }
        }
        List<GameObject> tempRoomList = roomList;
        //edit tempRoomList to have room r controlled
        List<string> moveScore = new List<string>();
        moveScore.Add(stateScore(tempRoomList).ToString());
        moveScore.Add("control");
        moveScore.Add(roomList.ToString());
        return moveScore;

    }

    int stateScore(List<GameObject> roomList)
    {
        int newMushroomIncome = 0;
        int newGoldIncome = 0;
        foreach (var thisRoom in roomList)
        {
            if (thisRoom.GetComponent<Room>().roomOwner == -1)
            {
                newMushroomIncome += thisRoom.GetComponent<Room>().GetMushroomIncome();
                newGoldIncome += thisRoom.GetComponent<Room>().GetGoldIncome();
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
