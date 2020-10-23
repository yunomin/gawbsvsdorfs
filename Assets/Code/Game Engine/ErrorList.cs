using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErrorList : MonoBehaviour
{

    public string[] buildError;
    public string[] moveError;
    public string[] selectionError;
    public string[] attackError;
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
        selectionError = new string[10];
        selectionError[0] = "Selected enemy unity";


        buildError = new string[20];
        buildError[0] = "Room not selected";
        buildError[1] = "Unit not selected";
        buildError[2] = "Selected room does not have empty spot";

        moveError = new string[20];
        moveError[0] = "Unit not selected";
        moveError[1] = "Room not selected";
        moveError[2] = "Unable to move to destination";

        attackError = new string[10];
        attackError[0] = "Room not selected";

    }
}
