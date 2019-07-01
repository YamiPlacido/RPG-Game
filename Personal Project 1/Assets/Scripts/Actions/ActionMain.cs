using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ActionMain : MonoBehaviour
{
    ActionSlot[] slots;

    public ActionSelectPopup SkillSelect;

    void Awake()
    {
        slots = GetComponentsInChildren<ActionSlot>();
        Managers.Skill.onActionChangedCallback += UpdateAction;

        SkillSelect.gameObject.SetActive(false);
    }

    void UpdateAction()
    {
        slots[0].SetSKill(Managers.Skill.GetLeftAction()?.name);
        slots[1].SetSKill(Managers.Skill.GetRightAction()?.name);
    }

    public void SkillSelectPopup(int slot)
    {
        SkillSelect.gameObject.SetActive(true);
        SkillSelect.AType = (slot == 0) ? ActionType.left : (slot == 1) ? ActionType.right : ActionType.extra;
    }
}
