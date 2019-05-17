using UnityEngine;
using System.Collections;

public class UIController : MonoBehaviour
{
    [SerializeField] CharacterSelectionController equipPopup;

    void Start()
    {
        equipPopup.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            bool isShowing = equipPopup.gameObject.activeSelf;
            equipPopup.gameObject.SetActive(!isShowing);
        }
    }
}
