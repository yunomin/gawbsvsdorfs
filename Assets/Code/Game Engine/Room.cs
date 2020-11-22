using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Room : MonoBehaviour
{

    public string roomName;
    public GameObject roomGameObject;
    public GameObject roomTile;
    public List<GameObject> archStones;
    public List<GameObject> archKeys;
    public int roomOwner; //Usage: should be 0 for neutral, 1 for player 1, -1 for player 2. 
    public List<GameObject> Adjacent; //A list of rooms that count as "Adjacent".
    public int[] roomID;
    public int roomSlots;
    public int emptySlots;
    public List<GameObject> buildingPlacementSlots;
    public int[] builtBuildings; //0 = no building | 1 = dorf Base | 2 = Camp | 3 = Upg. Camp |
                                 //4 = Gold Mine   | 5 = Upg. Mine | 6 = Farm | 7 = Upg. Farm| 8 = gob base
    public int buildingNumber;
    public int roomMushroomIncome;
    public int roomGoldIncome;
    public bool defensePresent; //For if this room has a mercenary camp or if it is a base.
    public int[] units; //0 = player1 , 1 = player2
    public List<GameObject> unitSpawns; //0 = player1 (dorf) , 1 = player2 (gowb)

    //materials
    public Material player1Material;
    public Material player2Material;
    public Material player1Stone;
    public Material player1Key;
    public Material player2Stone;
    public Material player2Key;

    //UI
    public Text UnitNumGawb;
    public Text UnitNumDorf;

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
        builtBuildings = new int[emptySlots];
        units = new int[2];
    }

    // Update is called once per frame
    void Update()
    {
        UnitNumGawb.text = units[0].ToString();
        UnitNumDorf.text = units[1].ToString();
    }

    public void Upgrade(int choice)
    {

    }

    public void SetAdjacent(GameObject room)
    {
        Adjacent.Add(room);
    }

    public bool isAdjacent(GameObject room)
    {
        foreach (GameObject adjacentRoom in Adjacent)
        {
            if (room == adjacentRoom)
            {
                return true;
            }
        }
        return false;
    }

    public void ChangeOwner(int newOwner)
    {
        roomOwner = newOwner;
        if (roomOwner == 1)
        {
            //roomMaterial = Resources.Load<Material>("P1_Own_Hi");
            MeshRenderer meshRenderer = roomTile.GetComponent<MeshRenderer>();
            meshRenderer.material = player1Material;
            for (int i = 0; i < archStones.Count; i++)
            {
                meshRenderer = archStones[i].GetComponent<MeshRenderer>();
                meshRenderer.material = player1Stone;
                meshRenderer = archKeys[i].GetComponent<MeshRenderer>();
                meshRenderer.material = player1Key;
            }
        }
        else
        {
            //roomMaterial = Resources.Load<Material>("Player 2 Owned");
            MeshRenderer meshRenderer = roomTile.GetComponent<MeshRenderer>();
            meshRenderer.material = player2Material;
            for (int i = 0; i < archStones.Count; i++)
            {
                meshRenderer = archStones[i].GetComponent<MeshRenderer>();
                meshRenderer.material = player2Stone;
                meshRenderer = archKeys[i].GetComponent<MeshRenderer>();
                meshRenderer.material = player2Key;
            }
        }
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
                    break;
                case 1: //Base       
                    roomGoldIncome += 5;
                    roomMushroomIncome += 6;
                    break;
                case 2: //Mercenary camp.
                    break;
                case 3: //Upgraded mercenary camp;
                    break; // None of these modify income.
                case 4: //Gold mine
                    roomGoldIncome += 5;
                    if (roomName == "3-2")
                    {
                        roomGoldIncome += 5;
                    }
                    break;
                case 5: //Upgraded Gold Mine;
                    roomGoldIncome += 10;
                    if (roomName == "3-2")
                    {
                        roomGoldIncome += 10;
                    }
                    break;
                case 6: //Mushroom Farm
                    roomMushroomIncome += 1;
                    if (roomName == "3-2")
                    {
                        roomMushroomIncome += 1;
                    }
                    break;
                case 7:
                    roomMushroomIncome += 3;
                    if (roomName == "3-2")
                    {
                        roomMushroomIncome += 3;
                    }
                    break;
                case 8:
                    roomGoldIncome += 5;
                    roomMushroomIncome += 6;
                    break;
            }

        }

    }


}
