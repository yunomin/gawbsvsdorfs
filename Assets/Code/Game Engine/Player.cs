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
    public int actions;
    public List<GameObject> ownedRooms;
    public int unitCount;
    public List<GameObject> units;
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
        mushroomIncome = 6;
        mushroomUpkeep = 6;
        actions = 0;
    }

    public void StartTurn()
    {
        actions = 2;
        myTurn = true;
        CalcIncome();
        MushroomEconomy();
        goldReserve += goldIncome;

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
            newMushroomIncome += roomscript.GetMushroomIncome();
            newGoldIncome += roomscript.GetGoldIncome();
        }
        mushroomIncome = newMushroomIncome;
        goldIncome = newGoldIncome; 
    }


    


}
