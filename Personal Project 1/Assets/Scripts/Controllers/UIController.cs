using UnityEngine;
using System.Collections;

public class UIController : MonoBehaviour
{
    [SerializeField] CharacterSelectionController equipPopup;
    [SerializeField] Inventory inventory;

    void Start()
    {
        equipPopup.gameObject.SetActive(false);
        inventory.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            bool isShowing = equipPopup.gameObject.activeSelf;
            equipPopup.gameObject.SetActive(!isShowing);
        }
        else if (Input.GetKeyDown(KeyCode.I))
        {
            bool isShowing = inventory.gameObject.activeSelf;
            inventory.gameObject.SetActive(!isShowing);
        }
    }
}
