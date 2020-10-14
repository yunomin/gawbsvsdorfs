using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErrorList : MonoBehaviour
{

    public string[] buildError;
    public string[] moveError;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void populateErrorList()
    {
        buildError = new string[20];
        buildError[0] = "Room not selected";
        buildError[1] = "Unit not selected";
        buildError[2] = "";

    }
}
