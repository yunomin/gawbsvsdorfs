using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public bool trigger;
    public List<Dialogue> dialogues;
    private int iter;

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
        FindObjectOfType<TutorialManager>().StartTutorial(dialogues[iter], dialogues[iter].index);
    }
    
}
