using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public GameObject slotItem;

    private void OnTriggerEnter(Collider other)
    {
        Inventory inven = other.GetComponent<Inventory>();
        for (int i = 0; i < inven.slots.Count; i++)
        {
            if (inven.slots[i].isEmpty)
            {
                Instantiate(slotItem, inven.slots[i].slotObj.transform);
                inven.slots[i].isEmpty = false;
                Destroy(this.gameObject);
                break;
            }
        }
    }
}
