using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodoDebug : MonoBehaviour
{
    public Gradient colorRamp;
    public SpriteRenderer sr;
    public int cost;

    public void SetCost(int cost)
    {
        this.cost = cost;
        sr.color = colorRamp.Evaluate(cost * 0.1f);
    }
}
