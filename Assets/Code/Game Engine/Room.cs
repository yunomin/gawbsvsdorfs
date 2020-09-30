﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{

    public string roomName;
    public GameObject roomGameObject;
    public int roomOwner; //Usage: should be 0 for neutral, 1 for player 1, -1 for player 2. 
    public List<GameObject> Adjacent; //A list of rooms that count as "Adjacent".
    public int[] roomID;
    public int roomSlots;
    public int emptySlots;
    public int[] builtBuildings; //0 = no building | 1 = Home Base | 2 = Camp | 3 = Upg. Camp |
                                 //4 = Gold Mine   | 5 = Upg. Mine | 6 = Farm | 7 = Upg. Farm|
   
    public int roomMushroomIncome;
    public int roomGoldIncome;
    public bool defensePresent; //For if this room has a mercenary camp or if it is a base.
    // Start is called before the first frame update
    
    public Room()
    {

    }

    public Room(string name, int owner, int[] ID, int rSlots, int eSlots)
    {
        roomName = name;
        roomOwner = owner;
        roomID = ID;
        roomSlots = rSlots;
        emptySlots = eSlots;
    }

    // Update is called once per frame

    

    public void Upgrade(int choice)
    {

    }

    public void SetAdjacent(GameObject room)
    {
        Adjacent.Add(room);
    }

    public void ChangeOwner(int newOwner)
    {
        roomOwner = newOwner;
    }

    public int GetGoldIncome()
    {
        return roomGoldIncome;
    }

    public int GetMushroomIncome()
    {
        return roomMushroomIncome;
    }

    public void RoomIncomeCalc()
    {
        roomMushroomIncome = 0;
        roomGoldIncome = 0;
        for (int x = 0; x < roomSlots; x++) //Mostly just future proofing for rooms with more than 2 slots.
        {
            switch (builtBuildings[x])
            {
                case 0: //No building built.          
                case 1: //Base                    
                case 2: //Mercenary camp.
                case 3: //Upgraded mercenary camp;
                    break; // None of these modify income.
                case 4: //Gold mine
                    roomGoldIncome += 5;
                    break;
                case 5: //Upgraded Gold Mine;
                    roomGoldIncome += 20;
                    break;
                case 6: //Mushroom Farm
                    roomMushroomIncome += 1;
                    break;
                case 7:
                    roomMushroomIncome += 3;
                    break;

            }

        }

    }


}