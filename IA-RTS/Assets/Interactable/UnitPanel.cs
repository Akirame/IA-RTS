using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitPanel : MonoBehaviour
{

    public Image unitArtwork;
    public Text unitName;
    public Text unitProf;
    public Text unitHand;

    public void ShowUnits(List<Unit> unitsSelected)
    {
        unitArtwork.sprite = unitsSelected[0].unitArtwork;
        unitName.text = unitsSelected[0].unitName;
        unitProf.text = unitsSelected[0].unitProf;
        unitHand.text = unitsSelected[0].unitHand;
    }
}
