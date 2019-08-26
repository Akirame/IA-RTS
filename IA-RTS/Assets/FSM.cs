using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM {

    public int[,] fsmMatrix;
    public int currentState;

    public void Create(int states, int events)
    {
        fsmMatrix = new int[states, events];
        for (int i = 0; i < states; i++)
        {
            for (int j = 0; j < events; j++)
            {
                fsmMatrix[i, j] = -1;
            }
        }
    }

    public void SetRelation(int _srcState, int _dstState, int _event){
        fsmMatrix[_srcState, _event] = _dstState;
    }

    public void SendEvent(int _event)
    {
        if (fsmMatrix[currentState,_event] != -1)
        {
            currentState = fsmMatrix[currentState, _event];
        }
    }

    public int GetState()
    {
        return currentState;
    }
}
