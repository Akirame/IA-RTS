using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapInteraction : MonoBehaviour
{

    public bool dragging = false;
    public Vector3 initDragPos;
    public Vector3 finDragPos;
    public Rect selectionRect;
    public InteractablesBoxSelection selectionBox;
    public List<Unit> unitsSelected = new List<Unit>();
    public UnitPanel unitPanel;

    private void Update()
    {
        GetMouseInput();
    }

    private void GetMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            dragging = true;
            initDragPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        if (dragging && Input.GetMouseButtonUp(0))
        {
            dragging = false;
            finDragPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            CreateSelectionBox();
        }
    }

    public void SetUnitsSelected(List<Unit> unitList)
    {
        unitsSelected = unitList;
        unitPanel.ShowUnits(unitsSelected);
    }

    private void CreateSelectionBox()
    {
        selectionRect = new Rect(initDragPos, new Vector3(finDragPos.x - initDragPos.x, finDragPos.y - initDragPos.y));
        GameObject selection = Instantiate(selectionBox.gameObject, transform.parent);
        selection.GetComponent<InteractablesBoxSelection>().SetRect(selectionRect, this);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(selectionRect.center, selectionRect.size);
    }

}
