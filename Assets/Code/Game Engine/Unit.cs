using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public int unitCount; // how many are in this stack
    public int owner; // -1 player 2/ai, +1 for player 1/human 1
    public GameObject location; //where the unit is.
}
