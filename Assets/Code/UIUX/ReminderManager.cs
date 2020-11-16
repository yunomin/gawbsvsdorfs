using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class ReminderManager : MonoBehaviour
{
    private string preMsg;
    public string currMsg;
    public Text reminder;
    // Start is called before the first frame update
    void Start()
    {
        preMsg = "";
        currMsg = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (preMsg.Equals(currMsg))
        {
            // skip
        }
        else
        {
            // update reminder
            reminder.text = currMsg;
            preMsg = currMsg;
        }
    }
    public void clearMsg()
    {
        currMsg = "";
    }

    public void updateMsg(string m)
    {
        currMsg = m;
    }
}
