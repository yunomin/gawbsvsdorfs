using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public bool trigger;
    public List<Dialogue> dialogues;
    private int iter;
    public GameObject tm;

    void Start()
    {
        iter = 0;
        if (trigger)
        {
            TriggerNext();
        }
    }
    public void TriggerNext()
    {
        print(iter.ToString());
        tm.GetComponent<TutorialManager>().StartTutorial(dialogues[iter], dialogues[iter].index);
        iter++;
    }
    
}
