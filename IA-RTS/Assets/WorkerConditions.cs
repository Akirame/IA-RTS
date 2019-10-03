using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerConditions
{
    private Worker worker;

    public WorkerConditions(Worker w)
    {
        worker = w;
    }
    public bool HasMaxGold()
    {
        return worker.handsEmpty;
    }
}
