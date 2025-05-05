using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public BaseState activeState;

    public void Initialise()
    {
        ChangState(new PatrolState());
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (activeState != null)
        {
            activeState.Perfrom();
        }
    }


    public void ChangState(BaseState newState)
    {
        if(activeState != null)
        {
            // run cleanup on activeState
            activeState.Exit();
        }

        // change to a new state
        activeState = newState;

        if(activeState != null)
        {
            // setup new state
            activeState.stateMachine = this;
            activeState.enemy = GetComponent<Enemy>();
            activeState.Enter();
        }
    }
}
