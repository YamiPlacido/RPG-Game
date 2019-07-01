using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ActionSlot : MonoBehaviour
{
    public string skill { get; private set; }

    public void SetSKill(string newSkill)
    {
        skill = newSkill;

        GetComponent<Image>().sprite = Resources.Load<Sprite>("Skills/" + skill);
        GetComponent<Image>().enabled = true;
    }

    public void ClearSkill()
    {
        skill = null;

        GetComponent<Image>().sprite = null;
        GetComponent<Image>().enabled = false;
    }

}
