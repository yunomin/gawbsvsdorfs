using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DorfAnimation : MonoBehaviour
{
    Animator unit;
    public int state; //0: no change, 1: idle, 2: walk, 3:attack
    void Start ()
    {
        unit = GetComponent<Animator>();
    }
    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}
    void update()
    {
       /* if (state != 0)
        {
            switch (state)
            {
                case 1: //switch to idle
                    break;
                case 2: //switch to walk
                    dorf.SetTrigger("toWalk");
                    state = 0;
                    break;
                case 3: //switch/trigger to bonk
                    break;
            }
        }*/
    }

    public void startWalk()
    {
        unit = GetComponent<Animator>();
        unit.SetTrigger("startWalk");
    }

    public void startIdle()
    {
        unit = GetComponent<Animator>();
        unit.SetTrigger("startIdle");
    }

    public void startBonk()
    {
        unit = GetComponent<Animator>();
        unit.SetTrigger("startBonk");
    }
    // OnStateExit is called before OnStateExit is called on any state inside this state machine
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called before OnStateMove is called on any state inside this state machine
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateIK is called before OnStateIK is called on any state inside this state machine
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMachineEnter is called when entering a state machine via its Entry Node
    //override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    //{
    //    
    //}

    // OnStateMachineExit is called when exiting a state machine via its Exit Node
    //override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    //{
    //    
    //}
}
