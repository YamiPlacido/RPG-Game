using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    public Transform itemsParent;

    InventorySlot[] slots;

    void Start()
    {
        slots = itemsParent.GetComponentsInChildren<InventorySlot>();
        Managers.Inventory.onItemChangedCallback += UpdateInventory;
    }

    void UpdateInventory()
    {
        List<string> itemList = Managers.Inventory.GetItemList();

        for (int i = 0; i < slots.Length; i++)
        {
            if (i < itemList.Count)
            {
                slots[i].AddItem(itemList[i],Managers.Inventory.GetItemCount(itemList[i]));
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }
}
