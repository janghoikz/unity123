using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Slotdata> slots = new List<Slotdata>();
    private int maxSlot = 5;
    public GameObject slotPrefab;

    private void Start()
    {
        GameObject slotPanel = GameObject.Find("Panel");
        for (int i = 0; i < maxSlot; i++)
        {
            GameObject go = Instantiate(slotPrefab, slotPanel.transform, false);
            go.name = "Slot_" + i;
            Slotdata slot = new Slotdata();
            slot.isEmpty = true;
            slot.slotObj = go;
            slots.Add(slot);
        }
    }

}
