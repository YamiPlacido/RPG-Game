using UnityEngine;
using System.Collections;
using Assets.Scripts.Managers;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour,IGameManager
{
    public ManagerStatus status { get; private set; }
    private Dictionary<string, int> _items;
    public string equippedItem { get; private set; }
    private NetworkService _network;

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    public void Startup(NetworkService service)
    {
        Debug.Log("Inventory Manager starting...");

        _network = service;

        UpdateData(new Dictionary<string, int>());

        status = ManagerStatus.Started;
    }

    public void UpdateData(Dictionary<string,int> items)
    {
        _items = items;
    }

    public Dictionary<string,int> GetData()
    {
        return _items;
    }

    private void DisplayItem()
    {
        string itemDisplay = "Item: ";
        foreach (KeyValuePair<string, int> item in _items)
        {
            itemDisplay += item.Key + "(" + item.Value + ") ";
        }
        Debug.Log(itemDisplay);
    }

    public void AddItem(string name)
    {
        if (_items.ContainsKey(name))
        {
            _items[name] += 1;
        }
        else
        {
            _items[name] = 1;
        }
        //call back to change inventory UI
        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();

        DisplayItem();
    }

    public void RemoveItem(string name)
    {
        if (_items.ContainsKey(name))
        {
            _items[name] -= 1;
        }

        //call back to change inventory UI
        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();

        DisplayItem();
    }
    //Return the list of all the dictionary keys
    public List<string> GetItemList()
    {
        List<string> list = new List<string>(_items.Keys);
        return list;
    }

    public int GetItemCount(string name)
    {
        if (_items.ContainsKey(name))
        {
            return _items[name];
        }
        return 0;
    }

    public bool EquipItem(string name)
    {
        if (_items.ContainsKey(name) && equippedItem != name)
        {
            equippedItem = name;
            Debug.Log("Equipped " + name);
            return true;
        }

        equippedItem = null;
        Debug.Log("Unequipped");
        return false;
    }

    public bool ConsumeItem(string name)
    {
        if (_items.ContainsKey(name))
        {
            _items[name]--;
            if (_items[name] == 0)
            {
                _items.Remove(name);
            }
        }
        else
        {
            Debug.Log("cannot consume " + name);
            return false;
        }

        DisplayItem();
        return true;
    }
}
