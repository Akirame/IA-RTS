using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractablesBoxSelection : MonoBehaviour
{

    public float endTimer = 0;
    public float endTime = 1;
    public List<Unit> unitList = new List<Unit>();
    public MapInteraction map;


    // Update is called once per frame
    void Update()
    {
        if (unitList.Count < 0)
            Destroy(this.gameObject);
        endTimer += Time.deltaTime;
        if (endTimer > endTime)
        {
            foreach (Unit unit in unitList)
                unit.SetSelected(true);
            map.SetUnitsSelected(unitList);
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Unit" && !unitList.Contains(other.transform.GetComponent<Unit>()))
        {
            unitList.Add(other.transform.GetComponent<Unit>());
        }
    }


    public void SetRect(Rect selectionRect, MapInteraction mapInteraction)
    {
        Vector3 boxSize = new Vector3(Mathf.Abs(selectionRect.size.x), Mathf.Abs(selectionRect.size.y), 1);
        transform.localScale = boxSize;
        transform.position = selectionRect.center;
        map = mapInteraction;
    }
}
