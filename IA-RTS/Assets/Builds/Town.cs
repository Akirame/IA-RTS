using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Town : MonoBehaviour {

    public int gold = 0;

    public void DepositGold(int v)
    {
        gold += v;
    }
}
