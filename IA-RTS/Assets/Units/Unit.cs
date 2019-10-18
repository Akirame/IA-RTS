using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{

    public bool selected = false;
    public Sprite unitArtwork;
    public string unitName;
    public string unitProf;
    public string unitHand;

    public void SetSelected(bool val)
    {
        selected = val;
    }
}
