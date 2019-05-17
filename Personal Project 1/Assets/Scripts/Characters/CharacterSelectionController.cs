using UnityEngine;
using System.Collections;

public class CharacterSelectionController : MonoBehaviour
{
    [SerializeField] private Character[] characters;

    public Character SelectedCharacter { get; private set; }

    private void Start()
    {
        SelectCharacter(characters[0]);
    }

    public void SelectCharacter(Character character)
    {
        SelectedCharacter = character;
        foreach (var cha in characters)
        {
            cha.gameObject.SetActive(cha == SelectedCharacter);
        }
    }

    public void EquipCurrentCharacter(Weapon weaponPrefab)
    {
        SelectedCharacter.EquipWeapon(weaponPrefab);
    }
}
