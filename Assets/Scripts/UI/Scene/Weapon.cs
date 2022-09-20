using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject slotItem;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.name == "UnityChan")
        {
            Inventory inven = collision.GetComponent<Inventory>();

            if (slotItem == null)
            {
                Debug.Log("¿À·ù¶ä");
            }
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
}
