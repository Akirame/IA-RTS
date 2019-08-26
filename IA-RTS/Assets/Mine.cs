using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour {

    public int gold = 30;

	public int MineGold(int mineAmount)
    {
        int amount;
        if (gold > mineAmount)
        {
            gold -= mineAmount;
            amount = mineAmount;
        }
        else
        {
            amount = gold;
            gold = 0;
        }
        return amount;
    }

    public bool HasGold()
    {
        return gold > 0;
    }

}
