using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public int index;
    [TextArea(3, 10)]
    public string[] sentences;
    
}
