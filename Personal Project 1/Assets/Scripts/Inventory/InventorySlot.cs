using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image icon;

    public Button itemAmount;

    public string item { get; private set; }

    public void AddItem(string newItem, int itemAmt)
    {
        item = newItem;

        icon.sprite = Resources.Load<Sprite>("Icons/"+item);
        icon.enabled = true;

        itemAmount.transform.GetChild(0).GetComponent<Text>().text = itemAmt.ToString();
        itemAmount.image.enabled = true;
    }

    public void ClearSlot()
    {
        item = null;

        icon.sprite = null;
        icon.enabled = false;
    }
}
