using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class ActionSelectPopup : MonoBehaviour
{
    Image[] selectSlots;

    public ActionType AType { get; set; }

    private string _curSelect;

    void Start()
    {
        selectSlots = GetComponentsInChildren<Image>();
        UpdateSkill();
    }

    public void UpdateSkill()
    {
        List<string> skillList = Managers.Skill.GetSkillList();

        int len = selectSlots.Length;
        for (int i = 0; i < len; i++)
        {
            if (i < skillList.Count)
            {
                selectSlots[i].gameObject.SetActive(true);

                selectSlots[i].gameObject.name = skillList[i];
                string skillName = skillList[i];

                Sprite sprite = Resources.Load<Sprite>("Skills/" + skillName);
                selectSlots[i].GetComponent<Image>().sprite = sprite;

                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerClick;    //enable cliking on icons
                entry.callback.AddListener((BaseEventData data) => { OnSelect(skillName,AType); });

                EventTrigger trigger = selectSlots[i].GetComponent<EventTrigger>();
                trigger.triggers.Clear();
                trigger.triggers.Add(entry);
            }
            else
            {
                selectSlots[i].gameObject.SetActive(false);
            }
        }
    }

    public void OnSelect(string skillName, ActionType AType)
    {
        _curSelect = skillName;
        if (AType == ActionType.left)
        {
            Managers.Skill.SetLeftAction(skillName);
        }
        else if(AType == ActionType.right)
        {
            Managers.Skill.SetRightAction(skillName);
        }
        gameObject.SetActive(false);
    }
}

public enum ActionType
{
    left, right, extra
}
