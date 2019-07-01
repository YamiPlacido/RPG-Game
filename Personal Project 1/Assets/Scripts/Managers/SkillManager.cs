using UnityEngine;
using System.Collections;
using Assets.Scripts.Managers;
using System.Collections.Generic;
using System.Linq;

public class SkillManager : MonoBehaviour, IGameManager
{
    public ManagerStatus status { get; set; }

    public List<SpecialMove> AllSkills;
    private List<string> _currentSkills;
    private SpecialMove _leftAction;
    private SpecialMove _rightAction;

    private NetworkService _network;
    public delegate void OnActionChanged();
    public OnActionChanged onActionChangedCallback;

    public void Startup(NetworkService service)
    {
        Debug.Log("Inventory Manager starting...");

        _network = service;

        UpdateData(new List<string>() { "Flame Of Destiny", "Heaven Lightning Strike", "Earth Shaking Flame", "Nova Strike" });

        status = ManagerStatus.Started;
    }

    private void Start()
    {
        SetLeftAction(_currentSkills.First());
        SetRightAction(_currentSkills.First());
    }

    public void UpdateData(List<string> skills)
    {
        _currentSkills = skills;
    }

    public List<string> GetSkillList()
    {
        return _currentSkills;
    }

    public void AddSkill(string skillName)
    {
        if (!_currentSkills.Contains(skillName))
        {
            _currentSkills.Add(skillName);
        }
        //callback to change action UI
        if(onActionChangedCallback != null)
        {
            onActionChangedCallback.Invoke();
        }
    }

    public void RemoveSkill(string skillName)
    {
        if (!_currentSkills.Contains(skillName))
        {
            _currentSkills.Remove(skillName);
        }
        //callback to change action UI
        if (onActionChangedCallback != null)
        {
            onActionChangedCallback.Invoke();
        }
    }

    public void SetLeftAction(string skillName)
    {
        if (_currentSkills.Contains(skillName))
        {
            var skill = AllSkills.Where(s => s.name == skillName).Single();
            _leftAction = skill;
        }
        //callback to change action UI
        if (onActionChangedCallback != null)
        {
            onActionChangedCallback.Invoke();
        }
    }

    public void SetRightAction(string skillName)
    {
        if (_currentSkills.Contains(skillName))
        {
            var skill = AllSkills.Where(s => s.name == skillName).Single();
            _rightAction = skill;
        }
        //callback to change action UI
        if (onActionChangedCallback != null)
        {
            onActionChangedCallback.Invoke();
        }
    }

    public SpecialMove GetLeftAction()
    {
        return _leftAction;
    }

    public SpecialMove GetRightAction()
    {
        return _rightAction;
    }
}
